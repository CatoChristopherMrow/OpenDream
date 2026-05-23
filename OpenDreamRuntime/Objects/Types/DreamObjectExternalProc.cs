namespace OpenDreamRuntime.Objects.Types;

internal sealed class DreamObjectExternalProc(DreamObjectDefinition objectDefinition, string library, string function) : DreamObject(objectDefinition) {
    public string Library { get; } = library;
    public string Function { get; } = function;

    protected override bool TryGetVar(string varName, out DreamValue value) {
        value = DreamValue.Null;
        return false;
    }

    protected override void SetVar(string varName, DreamValue value) {
        throw new Exception($"Cannot set var \"{varName}\"");
    }
}
