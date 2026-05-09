using System.Numerics;
using OpenDreamClient.Interface;
using OpenDreamShared.Network.Messages;
using NUnit.Framework;
using Robust.Shared.GameObjects;

namespace Content.Tests;

[TestFixture]
[TestOf(typeof(SoundCommandParser))]
public sealed class SoundCommandTests {
    [Test]
    public void ParsesQuotedResourceAndCommonParameters() {
        Assert.That(SoundCommandParser.TryParse("'bounce.ogg' channel=7 volume=25 repeat=1 offset=1.5 x=2 y=-3 z=4", out var command, out var error), Is.True, error);

        Assert.Multiple(() => {
            Assert.That(command.ResourcePath, Is.EqualTo("bounce.ogg"));
            Assert.That(command.Format, Is.EqualTo(MsgSound.FormatType.Ogg));
            Assert.That(command.Stop, Is.False);
            Assert.That(command.SoundData.Channel, Is.EqualTo(7));
            Assert.That(command.SoundData.Volume, Is.EqualTo(25));
            Assert.That(command.SoundData.Repeat, Is.EqualTo(1));
            Assert.That(command.SoundData.Offset, Is.EqualTo(1.5f));
            Assert.That(command.SoundData.OffsetPosition, Is.EqualTo(new Vector3(2, -3, 4)));
            Assert.That(command.SoundData.Atom, Is.EqualTo(NetEntity.Invalid));
        });
    }

    [Test]
    public void ParsesTransformTranslationOffset() {
        Assert.That(SoundCommandParser.TryParse("bounce.wav x=1 y=2 z=3 transform=1,0,4,0,1,5,0,0,6", out var command, out var error), Is.True, error);

        Assert.Multiple(() => {
            Assert.That(command.Format, Is.EqualTo(MsgSound.FormatType.Wav));
            Assert.That(command.SoundData.OffsetPosition, Is.EqualTo(new Vector3(5, 7, 9)));
        });
    }

    [Test]
    public void ParsesAnimationAtomPlaceholderAndFalloff() {
        var atom = new NetEntity(123);

        Assert.That(SoundCommandParser.TryParse("bounce.ogg atom=[[*]] falloff=10", out var command, out var error, atom), Is.True, error);

        Assert.Multiple(() => {
            Assert.That(command.SoundData.Atom, Is.EqualTo(atom));
            Assert.That(command.SoundData.Falloff, Is.EqualTo(10));
        });
    }

    [Test]
    public void ParsesAnimationAtomRefContext() {
        var atom = new NetEntity(123);

        Assert.That(SoundCommandParser.TryParse("bounce.ogg atom=[0x200001]", out var command, out var error, atom, "[0x200001]"), Is.True, error);

        Assert.That(command.SoundData.Atom, Is.EqualTo(atom));
    }

    [Test]
    public void ParsesNullAsStopCommand() {
        Assert.That(SoundCommandParser.TryParse("null channel=3", out var command, out var error), Is.True, error);

        Assert.Multiple(() => {
            Assert.That(command.Stop, Is.True);
            Assert.That(command.ResourcePath, Is.Null);
            Assert.That(command.SoundData.Channel, Is.EqualTo(3));
        });
    }
}
