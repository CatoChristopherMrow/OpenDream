using NUnit.Framework;
using OpenDreamClient.Interface;
using OpenDreamShared.Interface.DMF;

namespace Content.Tests;

[TestFixture]
public sealed class BrowserBridgeTests {
    [Test]
    public void WildcardWingetJsonUsesRawStringValues() {
        var json = BrowserBridgeJson.EncodePropertyMap([
            ("size", new DMFPropertySize(640, 456)),
            ("pos", new DMFPropertyPos(12, 34))
        ], rawStringValues: true);

        Assert.Multiple(() => {
            Assert.That(json, Does.Contain("\"size\":\"640x456\""));
            Assert.That(json, Does.Contain("\"pos\":\"12,34\""));
            Assert.That(json, Does.Not.Contain("\"size\":{\"x\""));
            Assert.That(json, Does.Not.Contain("\"pos\":{\"x\""));
        });
    }

    [Test]
    public void ExplicitJsonWingetKeepsTypedValues() {
        var json = BrowserBridgeJson.EncodePropertyMap([
            ("size", new DMFPropertySize(640, 456)),
            ("pos", new DMFPropertyPos(12, 34))
        ], rawStringValues: false);

        Assert.Multiple(() => {
            Assert.That(json, Does.Contain("\"size\":{\"x\":640"));
            Assert.That(json, Does.Contain("\"y\":456"));
            Assert.That(json, Does.Contain("\"pos\":{\"x\":12"));
            Assert.That(json, Does.Contain("\"y\":34"));
        });
    }

    [Test]
    public void TguiOutputMessageBecomesBrowserUpdateCall() {
        const string message = "%7b%22type%22%3a%22props%22%2c%22payload%22%3a%7b%22size%22%3a%22640x456%22%7d%7d";

        var script = BrowserBridgeScript.FormatOutputCall("update", message);

        Assert.That(script, Is.EqualTo("update(\"{\\\"type\\\":\\\"props\\\",\\\"payload\\\":{\\\"size\\\":\\\"640x456\\\"}}\")"));
    }

    [Test]
    public void OutputParamsBecomeSeparateBrowserArguments() {
        var arguments = BrowserBridgeScript.FormatOutputArguments("first%20arg&second%22arg");

        Assert.That(arguments, Is.EqualTo("\"first arg\",\"second\\\"arg\""));
    }

}
