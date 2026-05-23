using System;
using System.Numerics;
using Lidgren.Network;
using Robust.Shared.GameObjects;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace OpenDreamShared.Network.Messages {
    public sealed class MsgSound : NetMessage {
        public enum FormatType : byte {
            Ogg,
            Wav
        }

        public override MsgGroups MsgGroup => MsgGroups.EntityEvent;

        public SoundData SoundData;
        public int? ResourceId;
        public FormatType? Format; // TODO: This should probably be sent along with the sound resource instead somehow
        //TODO: Frequency and friends

        public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer serializer) {
            SoundData = new SoundData(buffer);

            if (buffer.ReadBoolean()) {
                ResourceId = buffer.ReadInt32();
                Format = (FormatType)buffer.ReadByte();
            }
        }

        public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer serializer) {
            SoundData.WriteToBuffer(buffer);

            buffer.Write(ResourceId != null);
            if (ResourceId != null) {
                buffer.Write(ResourceId.Value);

                if (Format == null)
                    throw new InvalidOperationException("Format cannot be null if there is a resource");
                buffer.Write((byte)Format);
            }
        }
    }

    public struct SoundData {
        /// <summary>
        /// The DreamSoundChannel channel (out of 1024) that the sound is set to play on
        /// </summary>
        public ushort Channel;

        /// <summary>
        /// Volume as a percentage
        /// </summary>
        public ushort Volume;

        /// <summary>
        /// Current playback position in seconds
        /// </summary>
        public float Offset;

        /// <summary>
        /// Total playtime of the song in seconds, adjusted for frequency
        /// TODO: adjust for freq
        /// </summary>
        public float Length;

        /// <summary>
        /// Set to 0 to not repeat, 1 to repeat indefinitely, or 2 to repeat forwards and backwards
        /// TODO: Implement repeat=2
        /// </summary>
        public byte Repeat;

        /// <summary>
        /// Filepath to the resource, if present
        /// </summary>
        public string File = string.Empty;

        /// <summary>
        /// Movable atom this sound should follow, if present
        /// </summary>
        public NetEntity Atom = NetEntity.Invalid;

        /// <summary>
        /// Positional offset from sound.x/y/z and sound.transform translation
        /// </summary>
        public Vector3 OffsetPosition;

        /// <summary>
        /// Maximum positional range for attenuation. 0 uses the engine default.
        /// </summary>
        public float Falloff;

        public SoundData(NetIncomingMessage buffer) {
            ReadFromBuffer(buffer);
        }

        private void ReadFromBuffer(NetIncomingMessage buffer) {
            Channel = buffer.ReadUInt16();
            Volume = buffer.ReadUInt16();
            Offset = buffer.ReadFloat();
            Length = buffer.ReadFloat();
            Repeat = buffer.ReadByte();
            File = buffer.ReadString();
            Atom = buffer.ReadNetEntity();
            OffsetPosition = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
            Falloff = buffer.ReadFloat();
        }

        public void WriteToBuffer(NetOutgoingMessage buffer) {
            buffer.Write(Channel);
            buffer.Write(Volume);
            buffer.Write(Offset);
            buffer.Write(Length);
            buffer.Write(Repeat);
            buffer.Write(File);
            buffer.Write(Atom);
            buffer.Write(OffsetPosition.X);
            buffer.Write(OffsetPosition.Y);
            buffer.Write(OffsetPosition.Z);
            buffer.Write(Falloff);
        }
    }
}
