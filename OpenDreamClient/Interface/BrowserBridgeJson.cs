using System.Text;
using System.Web;
using OpenDreamShared.Interface.DMF;

namespace OpenDreamClient.Interface;

public static class BrowserBridgeJson {
    public static string EncodePropertyMap(IEnumerable<(string Name, IDMFProperty Value)> properties, bool rawStringValues) {
        var json = new StringBuilder();
        json.Append('{');

        foreach (var (name, value) in properties) {
            if (json.Length > 1)
                json.Append(',');

            json.Append('"');
            json.Append(HttpUtility.JavaScriptStringEncode(name));
            json.Append("\":");

            if (rawStringValues) {
                json.Append('"');
                json.Append(HttpUtility.JavaScriptStringEncode(value.AsRaw()));
                json.Append('"');
            } else {
                json.Append(value.AsJson());
            }
        }

        json.Append('}');
        return json.ToString();
    }
}
