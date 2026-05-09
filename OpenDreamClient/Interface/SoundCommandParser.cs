using System.Globalization;
using System.Text;
using OpenDreamShared.Network.Messages;
using Robust.Shared.GameObjects;

namespace OpenDreamClient.Interface;

public readonly record struct SoundCommand(
    string? ResourcePath,
    SoundData SoundData,
    MsgSound.FormatType? Format,
    bool Stop);

public static class SoundCommandParser {
    public static bool TryParse(string command, out SoundCommand soundCommand, out string? error, NetEntity? atomContext = null, string? atomRefContext = null) {
        soundCommand = default;
        error = null;

        var tokens = Tokenize(command);
        if (tokens.Count == 0) {
            error = "Missing sound resource";
            return false;
        }

        var resourcePath = Unquote(tokens[0]);
        var data = new SoundData {
            Channel = 0,
            Volume = 100,
            Offset = 0,
            Length = 0,
            Repeat = 0,
            File = resourcePath,
            Atom = NetEntity.Invalid,
            OffsetPosition = default,
            Falloff = 0
        };

        var x = 0f;
        var y = 0f;
        var z = 0f;
        var transformOffset = Vector3.Zero;

        for (var i = 1; i < tokens.Count; i++) {
            var token = tokens[i];
            var equals = token.IndexOf('=');
            if (equals <= 0)
                continue;

            var key = token[..equals].Trim().ToLowerInvariant();
            var value = Unquote(token[(equals + 1)..].Trim());

            switch (key) {
                case "channel" when TryParseFloat(value, out var channel):
                    data.Channel = (ushort)Math.Clamp((int)channel, 0, 1024);
                    break;
                case "volume" when TryParseFloat(value, out var volume):
                    data.Volume = (ushort)Math.Clamp((int)volume, 0, ushort.MaxValue);
                    break;
                case "repeat" when TryParseFloat(value, out var repeat):
                    data.Repeat = (byte)Math.Clamp((int)repeat, 0, 2);
                    break;
                case "offset" when TryParseFloat(value, out var offset):
                    data.Offset = offset;
                    break;
                case "x" when TryParseFloat(value, out var xValue):
                    x = xValue;
                    break;
                case "y" when TryParseFloat(value, out var yValue):
                    y = yValue;
                    break;
                case "z" when TryParseFloat(value, out var zValue):
                    z = zValue;
                    break;
                case "transform":
                    transformOffset = ParseTransformOffset(value);
                    break;
                case "atom" when TryResolveAtom(value, atomContext, atomRefContext, out var atom):
                    data.Atom = atom;
                    break;
                case "falloff" when TryParseFloat(value, out var falloff):
                    data.Falloff = falloff;
                    break;
            }
        }

        data.OffsetPosition = new Vector3(x, y, z) + transformOffset;

        if (resourcePath.Equals("null", StringComparison.OrdinalIgnoreCase)) {
            data.File = string.Empty;
            soundCommand = new SoundCommand(null, data, null, true);
            return true;
        }

        var format = GetFormat(resourcePath);
        if (format == null) {
            error = $"Unsupported sound file type \"{resourcePath}\"";
            return false;
        }

        soundCommand = new SoundCommand(resourcePath, data, format, false);
        return true;
    }

    private static bool TryResolveAtom(string value, NetEntity? atomContext, string? atomRefContext, out NetEntity atom) {
        atom = NetEntity.Invalid;
        if (!atomContext.HasValue)
            return false;

        if (value == "[[*]]") {
            atom = atomContext.Value;
            return true;
        }

        if (atomRefContext is not null && value.Equals(atomRefContext, StringComparison.OrdinalIgnoreCase)) {
            atom = atomContext.Value;
            return true;
        }

        return false;
    }

    private static MsgSound.FormatType? GetFormat(string resourcePath) {
        if (resourcePath.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
            return MsgSound.FormatType.Ogg;

        if (resourcePath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            return MsgSound.FormatType.Wav;

        return null;
    }

    private static Vector3 ParseTransformOffset(string value) {
        var components = value.Split(',', StringSplitOptions.TrimEntries);
        if (components.Length != 9)
            return Vector3.Zero;

        return new Vector3(
            TryParseFloat(components[2], out var x) ? x : 0,
            TryParseFloat(components[5], out var y) ? y : 0,
            TryParseFloat(components[8], out var z) ? z : 0);
    }

    private static bool TryParseFloat(string value, out float result) {
        return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
    }

    private static string Unquote(string value) {
        value = value.Trim();
        if (value.Length >= 2 && ((value[0] == '\'' && value[^1] == '\'') || (value[0] == '"' && value[^1] == '"')))
            return value[1..^1];

        return value;
    }

    private static List<string> Tokenize(string command) {
        List<string> tokens = new();
        StringBuilder current = new();
        char quote = '\0';

        foreach (var c in command) {
            if (quote != '\0') {
                current.Append(c);
                if (c == quote)
                    quote = '\0';

                continue;
            }

            if (c is '\'' or '"') {
                quote = c;
                current.Append(c);
                continue;
            }

            if (char.IsWhiteSpace(c)) {
                if (current.Length > 0) {
                    tokens.Add(current.ToString());
                    current.Clear();
                }

                continue;
            }

            current.Append(c);
        }

        if (current.Length > 0)
            tokens.Add(current.ToString());

        return tokens;
    }
}
