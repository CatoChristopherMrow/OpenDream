using NUnit.Framework;
using OpenDreamShared.Dream;
using Robust.Shared.Maths;
using System.Numerics;

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

    [Test]
    public void ScreenKeywordsUseExpandedScreenBounds() {
        var view = new ViewRange(11, 11);
        var iconSize = new Vector2i(32, 32);
        ScreenLocation border = new("0,0");
        ScreenLocation screenSouthwest = new("SCREEN_SOUTHWEST");
        ScreenLocation normalSouthwest = new("SOUTHWEST");

        ScreenLocationBounds bounds = new ScreenLocationBounds(0, 0, view.Width, view.Height)
            .Include(border.GetScreenBounds(view, 32, iconSize));

        Assert.Multiple(() => {
            Assert.That(border.GetViewPosition(Vector2.Zero, view, 32, iconSize), Is.EqualTo(new Vector2(-1, -1)));
            Assert.That(screenSouthwest.GetViewPosition(Vector2.Zero, view, 32, iconSize, bounds), Is.EqualTo(new Vector2(-1, -1)));
            Assert.That(normalSouthwest.GetViewPosition(Vector2.Zero, view, 32, iconSize, bounds), Is.EqualTo(Vector2.Zero));
        });
    }

    [Test]
    public void ScreenKeywordsUseNormalBoundsWithoutBorderObjects() {
        var view = new ViewRange(11, 11);
        var iconSize = new Vector2i(32, 32);
        ScreenLocation screenNortheast = new("SCREEN_NORTHEAST");
        ScreenLocation screenRight = new("SCREEN_RIGHT");

        Assert.Multiple(() => {
            Assert.That(screenNortheast.GetViewPosition(Vector2.Zero, view, 32, iconSize), Is.EqualTo(new Vector2(10, 10)));
            Assert.That(screenRight.GetViewPosition(Vector2.Zero, view, 32, new Vector2i(64, 32)), Is.EqualTo(new Vector2(9, 5)));
        });
    }
}
