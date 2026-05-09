using OpenDreamShared.Dream;
using OpenDreamShared.Rendering;
using Robust.Shared.Maths;

namespace OpenDreamClient.Rendering;

[RegisterComponent]
internal sealed partial class DMISpriteComponent : SharedDMISpriteComponent {
    [ViewVariables] public DreamIcon Icon { get; set; }
    [ViewVariables] public ScreenLocation? ScreenLocation { get; set; }
    [ViewVariables] public Vector2i BoundOffset { get; set; }
}
