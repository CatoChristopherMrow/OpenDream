using NUnit.Framework;
using OpenDreamShared.Dream;
using Robust.Shared.Maths;

namespace Content.Tests;

[TestFixture]
public sealed class AppearanceOffsetTests {
    [Test]
    public void IconOffsetActsAsNegativePixelWZ() {
        MutableAppearance appearance = MutableAppearance.Get();
        try {
            appearance.PixelOffset = new Vector2i(1, 2);
            appearance.PixelOffset2 = new Vector2i(8, 16);
            appearance.IconOffset = new Vector2i(3, 5);

            var immutable = new ImmutableAppearance(appearance, null);

            Assert.That(immutable.GetTotalPixelOffset(MapFormat.Side), Is.EqualTo(new Vector2i(6, 13)));
        } finally {
            appearance.Dispose();
        }
    }

    [Test]
    public void SideMapRootIconsApplyBoundingBoxAnchor() {
        MutableAppearance appearance = MutableAppearance.Get();
        try {
            appearance.PixelOffset2 = new Vector2i(8, 16);
            appearance.IconOffset = new Vector2i(3, 5);

            var immutable = new ImmutableAppearance(appearance, null);

            Assert.That(
                AppearancePositioning.GetPixelOffset(immutable, MapFormat.Side, new Vector2i(32, 32), new Vector2i(4, 7), true),
                Is.EqualTo(new Vector2i(9, 18)));
        } finally {
            appearance.Dispose();
        }
    }

    [Test]
    public void IsometricRootIconsAnchorByLeftCorner() {
        MutableAppearance appearance = MutableAppearance.Get();
        try {
            var immutable = new ImmutableAppearance(appearance, null);

            Assert.That(
                AppearancePositioning.GetPixelOffset(immutable, MapFormat.Isometric, new Vector2i(32, 64), Vector2i.Zero, true),
                Is.EqualTo(new Vector2i(0, -32)));
        } finally {
            appearance.Dispose();
        }
    }

    [Test]
    public void OverlaysDoNotApplyRootMapFormatAnchors() {
        MutableAppearance appearance = MutableAppearance.Get();
        try {
            var immutable = new ImmutableAppearance(appearance, null);

            Assert.That(
                AppearancePositioning.GetPixelOffset(immutable, MapFormat.Isometric, new Vector2i(32, 64), new Vector2i(9, 11), false),
                Is.EqualTo(Vector2i.Zero));
        } finally {
            appearance.Dispose();
        }
    }

    [Test]
    public void IconOffsetIsCopiedIntoMutableAppearances() {
        MutableAppearance appearance = MutableAppearance.Get();
        try {
            appearance.IconOffset = new Vector2i(4, 7);

            var immutable = new ImmutableAppearance(appearance, null);
            MutableAppearance copy = immutable.ToMutable();

            try {
                Assert.That(copy.IconOffset, Is.EqualTo(new Vector2i(4, 7)));
            } finally {
                copy.Dispose();
            }
        } finally {
            appearance.Dispose();
        }
    }
}
