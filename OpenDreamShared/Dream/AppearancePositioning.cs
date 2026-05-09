using Robust.Shared.Maths;

namespace OpenDreamShared.Dream;

public static class AppearancePositioning {
    public static Vector2i GetPixelOffset(ImmutableAppearance appearance, MapFormat mapFormat, Vector2i iconSize, Vector2i boundOffset, bool applyBoundAnchor) {
        Vector2i offset = appearance.PixelOffset + appearance.PixelOffset2 - appearance.IconOffset;

        if (applyBoundAnchor && mapFormat != MapFormat.TopDown)
            offset += boundOffset;

        if (applyBoundAnchor && mapFormat == MapFormat.Isometric)
            offset.Y -= iconSize.Y / 2;

        return offset;
    }
}
