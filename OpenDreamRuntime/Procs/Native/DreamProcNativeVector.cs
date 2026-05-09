using OpenDreamRuntime.Objects;
using OpenDreamRuntime.Objects.Types;
using DreamValueTypeFlag = OpenDreamRuntime.DreamValue.DreamValueTypeFlag;

namespace OpenDreamRuntime.Procs.Native;

internal static class DreamProcNativeVector {
    [DreamProc("Cross")]
    [DreamProcParameter("B", Type = DreamValueTypeFlag.DreamObject)]
    public static DreamValue NativeProc_Cross(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
        var vector = (DreamObjectVector)src!;
        if (!bundle.GetArgument(0, "B").TryGetValueAsDreamObject<DreamObjectVector>(out var other))
            return DreamValue.Null;

        return new DreamValue(DreamObjectVector.CreateFromValue(new Vector3(
            (float)(vector.Y * other.Z - vector.Z * other.Y),
            (float)(vector.Z * other.X - vector.X * other.Z),
            (float)(vector.X * other.Y - vector.Y * other.X)), bundle.ObjectTree));
    }

    [DreamProc("Turn")]
    [DreamProcParameter("angle", Type = DreamValueTypeFlag.Float)]
    public static DreamValue NativeProc_Turn(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
        var vector = (DreamObjectVector)src!;
        bundle.GetArgument(0, "angle").TryGetValueAsFloat(out var angle);

        var radians = angle * Math.PI / 180.0;
        var cosine = Math.Cos(radians);
        var sine = Math.Sin(radians);

        return new DreamValue(DreamObjectVector.CreateFromValue(new Vector2(
            (float)(vector.X * cosine - vector.Y * sine),
            (float)(vector.X * sine + vector.Y * cosine)), bundle.ObjectTree));
    }
}
