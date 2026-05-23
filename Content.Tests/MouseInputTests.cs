using NUnit.Framework;
using OpenDreamShared.Dream;
using OpenDreamShared.Input;

namespace Content.Tests;

[TestFixture]
public sealed class MouseInputTests {
    [Test]
    public void MouseButtonsUseByondParamNames() {
        Assert.Multiple(() => {
            Assert.That(SharedMouseInputSystem.GetButtonParamName(SharedMouseInputSystem.MouseButton.Left), Is.EqualTo("left"));
            Assert.That(SharedMouseInputSystem.GetButtonParamName(SharedMouseInputSystem.MouseButton.Right), Is.EqualTo("right"));
            Assert.That(SharedMouseInputSystem.GetButtonParamName(SharedMouseInputSystem.MouseButton.Middle), Is.EqualTo("middle"));
            Assert.That(SharedMouseInputSystem.GetButtonParamName(SharedMouseInputSystem.MouseButton.Mouse4), Is.EqualTo("mouse4"));
            Assert.That(SharedMouseInputSystem.GetButtonParamName(SharedMouseInputSystem.MouseButton.Mouse5), Is.EqualTo("mouse5"));
        });
    }

    [Test]
    public void ClickParamsExposeLegacyButtonBooleans() {
        var screenLoc = new ScreenLocation(0, 0, 32);
        var rightClick = new SharedMouseInputSystem.ClickParams(screenLoc, SharedMouseInputSystem.MouseButton.Right, false, false, false, 0, 0);
        var middleClick = new SharedMouseInputSystem.ClickParams(screenLoc, SharedMouseInputSystem.MouseButton.Middle, false, false, false, 0, 0);
        var mouse4Click = new SharedMouseInputSystem.ClickParams(screenLoc, SharedMouseInputSystem.MouseButton.Mouse4, false, false, false, 0, 0);

        Assert.Multiple(() => {
            Assert.That(rightClick.Right, Is.True);
            Assert.That(rightClick.Middle, Is.False);
            Assert.That(middleClick.Right, Is.False);
            Assert.That(middleClick.Middle, Is.True);
            Assert.That(mouse4Click.Right, Is.False);
            Assert.That(mouse4Click.Middle, Is.False);
        });
    }
}
