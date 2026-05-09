using System.Threading.Tasks;
using OpenDreamRuntime.Objects;
using OpenDreamRuntime.Objects.Types;
using DreamValueTypeFlag = OpenDreamRuntime.DreamValue.DreamValueTypeFlag;

namespace OpenDreamRuntime.Procs.Native;

internal static class DreamProcNativeClient {
    [DreamProc("SoundQuery")]
    public static async Task<DreamValue> NativeProc_SoundQuery(AsyncNativeProc.AsyncNativeProcState state) {
        var client = (DreamObjectClient)state.Instance!;
        return await client.Connection.SoundQuery();
    }

    [DreamProc("Export")]
    [DreamProcParameter("file")]
    public static DreamValue NativeProc_Export(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
        return DreamValue.Null;
    }

    [DreamProc("MeasureText")]
    [DreamProcParameter("text", Type = DreamValueTypeFlag.String)]
    [DreamProcParameter("style", Type = DreamValueTypeFlag.String)]
    [DreamProcParameter("width", Type = DreamValueTypeFlag.Float, DefaultValue = 0)]
    public static DreamValue NativeProc_MeasureText(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
        var text = bundle.GetArgument(0, "text").Stringify();
        return new DreamValue($"{text.Length * 8}x16");
    }
}
