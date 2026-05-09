using System.Text;
using System.Web;

namespace OpenDreamClient.Interface;

public static class BrowserBridgeScript {
    public static string FormatOutputCall(string jsFunction, string value) {
        return $"{jsFunction}({FormatOutputArguments(value)})";
    }

    public static string FormatOutputArguments(string value) {
        var result = new StringBuilder();
        var parts = value.Split('&');

        for (var i = 0; i < parts.Length; i++) {
            if (i > 0)
                result.Append(',');

            result.Append('"');
            result.Append(HttpUtility.JavaScriptStringEncode(HttpUtility.UrlDecode(parts[i])));
            result.Append('"');
        }

        return result.ToString();
    }
}
