using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;
using System;
using Robust.Shared.GameStates;
using OpenDreamShared.Dream;
using Robust.Shared.Maths;

namespace OpenDreamShared.Rendering;

[NetworkedComponent]
public abstract partial class SharedDMISpriteComponent : Component {
    [Serializable, NetSerializable]
    public sealed class DMISpriteComponentState : ComponentState {
        public readonly uint? AppearanceId;
        public readonly ScreenLocation ScreenLocation;
        public readonly Vector2i BoundOffset;

        public DMISpriteComponentState(uint? appearanceId, ScreenLocation screenLocation, Vector2i boundOffset) {
            AppearanceId = appearanceId;
            ScreenLocation = screenLocation;
            BoundOffset = boundOffset;
        }
    }
}
