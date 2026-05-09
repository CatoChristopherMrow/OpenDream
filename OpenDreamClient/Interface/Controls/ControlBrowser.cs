using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using OpenDreamShared.Interface.Descriptors;
using OpenDreamClient.Resources;
using OpenDreamShared.Network.Messages;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.WebView;
using Robust.Shared.Console;
using Robust.Shared.ContentPack;
using Robust.Shared.Network;
using Robust.Shared.Utility;

namespace OpenDreamClient.Interface.Controls;

internal sealed partial class ControlBrowser : InterfaceControl {
    private const string BrowserStorageShim = """
        <script>
        (function() {
            if (window.hubStorage)
                return;

            function key(name) {
                return "opendream:byondstorage:" + String(name);
            }

            function notifyUpdated() {
                if (typeof Event === "function") {
                    document.dispatchEvent(new Event("byondstorageupdated"));
                    return;
                }

                var event = document.createEvent("Event");
                event.initEvent("byondstorageupdated", false, false);
                document.dispatchEvent(event);
            }

            window.hubStorage = {
                getItem: function(name) {
                    try {
                        return Promise.resolve(window.localStorage.getItem(key(name)));
                    } catch (err) {
                        return Promise.resolve(null);
                    }
                },
                setItem: function(name, value) {
                    try {
                        window.localStorage.setItem(key(name), String(value));
                    } finally {
                        notifyUpdated();
                    }

                    return Promise.resolve();
                },
                removeItem: function(name) {
                    try {
                        window.localStorage.removeItem(key(name));
                    } finally {
                        notifyUpdated();
                    }

                    return Promise.resolve();
                },
                clear: function() {
                    try {
                        var prefix = key("");
                        for (var i = window.localStorage.length - 1; i >= 0; i--) {
                            var storageKey = window.localStorage.key(i);
                            if (storageKey && storageKey.indexOf(prefix) === 0)
                                window.localStorage.removeItem(storageKey);
                        }
                    } finally {
                        notifyUpdated();
                    }

                    return Promise.resolve();
                }
            };

            notifyUpdated();
        })();
        </script>
        """;

    private const string BrowserGeometryShimTemplate = """
        <script>
        (function() {
            if (window.__opendreamGeometryShim)
                return;

            var geometry = { x: __OPENDREAM_GEOMETRY_X__, y: __OPENDREAM_GEOMETRY_Y__ };

            window.__opendreamGeometryShim = {
                set: function(x, y) {
                    geometry.x = Number(x) || 0;
                    geometry.y = Number(y) || 0;
                }
            };

            function defineWindowNumber(name, getter) {
                try {
                    Object.defineProperty(window, name, {
                        configurable: true,
                        get: getter
                    });
                } catch (err) {
                }
            }

            defineWindowNumber("screenLeft", function() { return geometry.x; });
            defineWindowNumber("screenTop", function() { return geometry.y; });
            defineWindowNumber("screenX", function() { return geometry.x; });
            defineWindowNumber("screenY", function() { return geometry.y; });

            try {
                Object.defineProperty(MouseEvent.prototype, "screenX", {
                    configurable: true,
                    get: function() { return geometry.x + this.clientX; }
                });
                Object.defineProperty(MouseEvent.prototype, "screenY", {
                    configurable: true,
                    get: function() { return geometry.y + this.clientY; }
                });
            } catch (err) {
            }
        })();
        </script>
        """;

    private static readonly Dictionary<string, string> FileExtensionMimeTypes = new() {
        { "css", "text/css" },
        { "html", "text/html" },
        { "htm", "text/html" },
        { "png", "image/png" },
        { "svg", "image/svg+xml" },
        { "jpeg", "image/jpeg" },
        { "jpg", "image/jpeg" },
        { "js", "application/javascript" },
        { "json", "application/json" },
        { "ttf", "font/ttf" },
        { "woff2", "font/woff2" },
        { "txt", "text/plain" }
    };

    [Dependency] private IResourceManager _resourceManager = default!;
    [Dependency] private IClientNetManager _netManager = default!;
    [Dependency] private IDreamResourceManager _dreamResource = default!;

    private readonly ISawmill _sawmill = Logger.GetSawmill("opendream.browser");

    private PanelContainer _panel = default!;
    private WebViewControl _webView = default!;
    private Vector2i _geometryOrigin;

    private Stream AddBrowserShims(Stream stream) {
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: false);
        var html = reader.ReadToEnd();

        if (html.Contains("window.hubStorage", StringComparison.Ordinal) &&
            html.Contains("window.__opendreamGeometryShim", StringComparison.Ordinal))
            return new MemoryStream(Encoding.UTF8.GetBytes(html));

        var shims = string.Empty;
        if (!html.Contains("window.hubStorage", StringComparison.Ordinal))
            shims += BrowserStorageShim;
        if (!html.Contains("window.__opendreamGeometryShim", StringComparison.Ordinal)) {
            shims += BrowserGeometryShimTemplate
                .Replace("__OPENDREAM_GEOMETRY_X__", _geometryOrigin.X.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal)
                .Replace("__OPENDREAM_GEOMETRY_Y__", _geometryOrigin.Y.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);
        }

        var insertIndex = html.IndexOf("<head>", StringComparison.OrdinalIgnoreCase);
        if (insertIndex >= 0) {
            insertIndex += "<head>".Length;
            html = html.Insert(insertIndex, shims);
        } else {
            html = shims + html;
        }

        return new MemoryStream(Encoding.UTF8.GetBytes(html));
    }

    public ControlBrowser(ControlDescriptor controlDescriptor, ControlWindow window)
        : base(controlDescriptor, window) {
        IoCManager.InjectDependencies(this);
    }

    protected override Control CreateUIElement() {
        _panel = new PanelContainer {
            Children = {
                (_webView = new WebViewControl {AlwaysActive = true})
            }
        };

        _webView.AddResourceRequestHandler(RequestHandler);
        _webView.AddBeforeBrowseHandler(BeforeBrowseHandler);
        _webView.OnVisibilityChanged += (args) => {
            if (args.Visible) {
                OnShowEvent();
            } else {
                OnHideEvent();
            }
        };

        if(ControlDescriptor.IsVisible.Value)
            OnShowEvent();
        else
            OnHideEvent();

        return _panel;
    }

    protected override void UpdateElementDescriptor() {
        base.UpdateElementDescriptor();

        _panel.PanelOverride = new StyleBoxFlat(Color.White); // Always white background
    }

    public override void Output(string value, string? jsFunction) {
        if (jsFunction == null) return;

        if (_webView.Disposed)
            return;

        // Insert the values directly into JS and execute it (what could go wrong??)
        try {
            _webView.ExecuteJavaScript(BrowserBridgeScript.FormatOutputCall(jsFunction, value));
        } catch (Exception e) when (e.GetType().Name == "ObjectDisposedException") {
            // The window can close while output messages are still queued.
        } catch (InvalidOperationException e) {
            _sawmill.Debug($"Ignoring browser output to an unavailable web view: {e.Message}");
        }
    }

    public void SetFileSource(ResPath? filepath) {
        if (filepath != null) {
            // hostname must be the localhost IP for TGUI to work properly
            _webView.Url = "http://127.0.0.1/" + filepath;
        } else {
            _webView.Url = "about:blank";
        }
    }

    public void ShutdownBrowser() {
        _webView.Url = "about:blank";
        _webView.AlwaysActive = false;
    }

    public void SetGeometryOrigin(Vector2i position) {
        _geometryOrigin = position;

        if (_webView.Disposed)
            return;

        try {
            _webView.ExecuteJavaScript($"window.__opendreamGeometryShim && window.__opendreamGeometryShim.set({position.X}, {position.Y});");
        } catch (Exception e) when (e.GetType().Name == "ObjectDisposedException") {
        } catch (InvalidOperationException e) {
            _sawmill.Debug($"Ignoring geometry update to an unavailable web view: {e.Message}");
        }
    }

    private void BeforeBrowseHandler(IBeforeBrowseContext context) {
        // An exception in here will freeze up / crash CEF, so catch any
        try {
            if (string.IsNullOrEmpty(_webView.Url))
                return;

            Uri oldUri = new Uri(_webView.Url);
            Uri newUri = new Uri(context.Url);

            bool isLocalTopicNavigation = newUri is { Scheme: "http", Host: "127.0.0.1" } &&
                newUri.Query != string.Empty &&
                (newUri.AbsolutePath == oldUri.AbsolutePath || newUri.AbsolutePath == "/");

            if (newUri.Scheme == "byond" || isLocalTopicNavigation) {
                context.DoCancel();

                switch (newUri.Host) {
                    case "winset":
                        HandleEmbeddedWinset(newUri.Query);
                        return;
                    case "winget":
                        HandleEmbeddedWinget(newUri.Query);
                        return;
                    default: {
                        var msg = new MsgTopic { Query = newUri.Query };
                        _netManager.ClientSendMessage(msg);
                        break;
                    }
                }
            }
        } catch (Exception e) {
            _sawmill.Error($"Exception in BeforeBrowseHandler: {e}");
        }
    }

    private void RequestHandler(IRequestHandlerContext context) {
        // An exception in here will crash RT (and not log it because it's uncaught)
        try {
            Uri newUri = new Uri(context.Url);

            if (newUri is {Scheme: "http", Host: "127.0.0.1"}) {
                Stream stream;
                HttpStatusCode status;
                var path = new ResPath(newUri.AbsolutePath);
                if (path.Filename.Equals("favicon.ico", StringComparison.OrdinalIgnoreCase)) {
                    stream = Stream.Null;
                    status = HttpStatusCode.NotFound;
                } else if (!_dreamResource.EnsureCacheFile(newUri.AbsolutePath)) {
                    stream = Stream.Null;
                    status = HttpStatusCode.NotFound;
                } else {
                    try {
                        stream = _resourceManager.UserData.OpenRead(
                            _dreamResource.GetCacheFilePath(newUri.AbsolutePath));
                        status = HttpStatusCode.OK;
                    } catch (FileNotFoundException) {
                        stream = Stream.Null;
                        status = HttpStatusCode.NotFound;
                    } catch (Exception e) {
                        _sawmill.Error($"Exception while loading file from {newUri}:\n{e}");
                        stream = Stream.Null;
                        status = HttpStatusCode.InternalServerError;
                    }
                }

                var mimeType = FileExtensionMimeTypes.GetValueOrDefault(path.Extension, "application/octet-stream");
                if (status == HttpStatusCode.OK && mimeType == "text/html")
                    stream = AddBrowserShims(stream);

                context.DoRespondStream(stream, mimeType, status);
            }
        } catch (Exception e) {
            _sawmill.Error($"Exception in RequestHandler: {e}");
            context.DoCancel();
        }
    }

    /// <summary>
    /// Handles an embedded winset
    /// <code>byond://winset?command=.quit</code>
    /// </summary>
    /// <param name="query">The query portion of the embedded winset</param>
    private void HandleEmbeddedWinset(string query) {
        // Strip the question mark out before parsing
        // Also replace ';' with '&' because they're both usable here
        var queryParams = HttpUtility.ParseQueryString(query.Substring(1).Replace(';', '&'));

        // We need to extract the control element (if one was included).
        // BYOND's browser helpers use "id" here, older OpenDream callers used "element".
        string? element = queryParams.Get("element") ?? queryParams.Get("id");
        queryParams.Remove("element");
        queryParams.Remove("id");

        // Wrap each parameter in quotes so the entire value is used
        foreach (var paramKey in queryParams.AllKeys) {
            if (paramKey == null)
                continue;

            var paramValue = queryParams[paramKey];
            if (paramValue == null)
                continue;

            queryParams.Set(paramKey, $"\"{paramValue}\"");
        }

        // Reassemble the query params without element then convert to winset syntax
        var modifiedQuery = queryParams.ToString();
        modifiedQuery = HttpUtility.UrlDecode(modifiedQuery);
        modifiedQuery = modifiedQuery!.Replace('&', ';'); // TODO: More robust parsing

        // We can finally call winset
        InterfaceManager.WinSet(element, modifiedQuery);
    }

    /// <summary>
    /// Handles an embedded winget
    /// </summary>
    /// <param name="query">The query portion of the embedded winget</param>
    // Example: byond://winget?id=browseroutput&property=size&callback=JSFunction
    // (Not in the XML comment because '&' breaks that apparently)
    private void HandleEmbeddedWinget(string query) {
        // Strip the question mark out before parsing
        // Also replace ';' with '&' because they're both usable here
        var queryParams = HttpUtility.ParseQueryString(query.Substring(1).Replace(';', '&'));

        if (queryParams.Get("id") is not { } elementId ||
            queryParams.Get("property") is not { } property ||
            queryParams.Get("callback") is not { } callback) {
            _sawmill.Error($"Required arg 'id', 'property', or 'callback' not provided in embedded winget ({query})");
            return;
        }

        // tg/TGUI's Byond.winget('output') asks for "*" but uses only the size field
        // to resize the browseroutput pane. Keep that legacy shape instead of handing
        // the browser a huge descriptor object that changes the panel layout path.
        var outputSizeOnly = elementId == "output" && property == "*";
        if (outputSizeOnly)
            property = "size";

        // Multiple properties can be queried in a single winget with "&property=size,view-size"
        var properties = property.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var jsonBuilder = new StringBuilder(); // Build the JSON object that the callback receives

        if (properties.Length == 1 && properties[0] == "*") {
            jsonBuilder.Append(InterfaceManager.WinGet(elementId, "*", forceJson: true));
        } else {
            jsonBuilder.Append("{ ");
            foreach (var wingetting in properties) {
                if (jsonBuilder.Length > 2)
                    jsonBuilder.Append(", ");

                jsonBuilder.Append('"');
                jsonBuilder.Append(HttpUtility.JavaScriptStringEncode(wingetting));
                jsonBuilder.Append("\": ");
                var result = InterfaceManager.WinGet(elementId, wingetting, forceJson: !outputSizeOnly);

                jsonBuilder.Append(string.IsNullOrEmpty(result)
                    ? "\"\""
                    : outputSizeOnly
                        ? $"\"{HttpUtility.JavaScriptStringEncode(result)}\""
                        : result);
            }

            jsonBuilder.Append(" }");
        }

        // Execute the callback
        var json = jsonBuilder.ToString();
        if (_webView.Disposed)
            return;

        try {
            _webView.ExecuteJavaScript($"{callback}({json})");
        } catch (Exception e) when (e.GetType().Name == "ObjectDisposedException") {
            // The window can close while winget callbacks are still queued.
        } catch (InvalidOperationException e) {
            _sawmill.Debug($"Ignoring embedded winget callback to an unavailable web view: {e.Message}");
        }
    }

    private void OnShowEvent() {
        ControlDescriptorBrowser controlDescriptor = (ControlDescriptorBrowser)ControlDescriptor;
        if (!string.IsNullOrWhiteSpace(controlDescriptor.OnShowCommand.Value)) {
            InterfaceManager.RunCommand(controlDescriptor.OnShowCommand.AsRaw());
        }
    }

    private void OnHideEvent() {
        ControlDescriptorBrowser controlDescriptor = (ControlDescriptorBrowser)ControlDescriptor;
        if (!string.IsNullOrWhiteSpace(controlDescriptor.OnHideCommand.Value)) {
            InterfaceManager.RunCommand(controlDescriptor.OnHideCommand.AsRaw());
        }
    }
}

public sealed class BrowseWinCommand : IConsoleCommand {
    public string Command => "browsewin";
    public string Description => "";
    public string Help => "";

    public void Execute(IConsoleShell shell, string argStr, string[] args) {
        if (args.Length != 1) {
            shell.WriteError("Incorrect amount of arguments! Must be a single one.");
            return;
        }

        var parameters = new BrowserWindowCreateParameters(1280, 720) {Url = args[0]};

        var cef = IoCManager.Resolve<IWebViewManager>();
        cef.CreateBrowserWindow(parameters);
    }
}
