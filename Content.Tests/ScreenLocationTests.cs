using NUnit.Framework;
using OpenDreamShared.Dream;

namespace Content.Tests;

[TestFixture]
public sealed class ScreenLocationTests {
    [Test]
    public void Parses516ScreenDirectionalKeywords() {
        ScreenLocation southwest = new("SCREEN_SOUTHWEST");
        Assert.Multiple(() => {
            Assert.That(southwest.AnchorToScreenBounds, Is.True);
            Assert.That(southwest.HorizontalAnchor, Is.EqualTo(HorizontalAnchor.West));
            Assert.That(southwest.VerticalAnchor, Is.EqualTo(VerticalAnchor.South));
            Assert.That(southwest.X, Is.EqualTo(0));
            Assert.That(southwest.Y, Is.EqualTo(0));
        });

        ScreenLocation northeast = new("SCREEN_NORTHEAST");
        Assert.Multiple(() => {
            Assert.That(northeast.AnchorToScreenBounds, Is.True);
            Assert.That(northeast.HorizontalAnchor, Is.EqualTo(HorizontalAnchor.East));
            Assert.That(northeast.VerticalAnchor, Is.EqualTo(VerticalAnchor.North));
        });

        ScreenLocation east = new("SCREEN_EAST");
        Assert.Multiple(() => {
            Assert.That(east.AnchorToScreenBounds, Is.True);
            Assert.That(east.HorizontalAnchor, Is.EqualTo(HorizontalAnchor.East));
            Assert.That(east.VerticalAnchor, Is.EqualTo(VerticalAnchor.Center));
        });
    }

    [Test]
    public void Parses516ScreenAxisKeywords() {
        ScreenLocation loc = new("SCREEN_NORTH+1,SCREEN_WEST-2:8");

        Assert.Multiple(() => {
            Assert.That(loc.AnchorToScreenBounds, Is.True);
            Assert.That(loc.HorizontalAnchor, Is.EqualTo(HorizontalAnchor.West));
            Assert.That(loc.VerticalAnchor, Is.EqualTo(VerticalAnchor.North));
            Assert.That(loc.X, Is.EqualTo(-2));
            Assert.That(loc.Y, Is.EqualTo(1));
            Assert.That(loc.PixelOffsetX, Is.EqualTo(8));
        });
    }

    [Test]
    public void ParsesMiddleAsCenter() {
        ScreenLocation loc = new("MIDDLE");

        Assert.Multiple(() => {
            Assert.That(loc.AnchorToScreenBounds, Is.False);
            Assert.That(loc.HorizontalAnchor, Is.EqualTo(HorizontalAnchor.Center));
            Assert.That(loc.VerticalAnchor, Is.EqualTo(VerticalAnchor.Center));
        });
    }
}
