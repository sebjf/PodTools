using System;
using System.IO;
using System.Text;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.Circuits;

namespace Syroot.Pod
{
    /// <summary>
    /// Represents extension methods for <see cref="Stream"/> instances.
    /// </summary>
    internal static class StreamExtensions
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private static readonly char[] _fixedStringTerminator = new[] { '\0' };

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        internal static string ReadFixedString(this Stream self, int length, Encoding encoding = null)
        {
            byte[] buffer = new byte[length];
            int readBytes = self.Read(buffer, 0, length);
            if (readBytes < length)
                throw new InvalidDataException("Incomplete fixed string.");
            return (encoding ?? Encoding.ASCII).GetString(buffer).TrimEnd(_fixedStringTerminator);
        }

        internal static string ReadPodString(this Stream self)
        {
            char[] chars = new char[self.ReadByte()];
            for (int i = 0; i < chars.Length; i++)
                chars[i] = (char)((byte)(self.ReadByte() ^ ~i));
            return new string(chars);
        }

        internal static Single ReadSingle16x16(this Stream self)
        {
            return self.ReadInt32() / (Single)(1 << 16);
        }

        internal static Vector2U ReadVector2U(this Stream self)
        {
            return new Vector2U(self.ReadUInt32(), self.ReadUInt32());
        }

        internal static Vector3U ReadVector3U(this Stream self)
        {
            return new Vector3U(self.ReadUInt32(), self.ReadUInt32(), self.ReadUInt32());
        }

        internal static Vector3F ReadVector3F16x16(this Stream self)
        {
            return new Vector3F(ReadSingle16x16(self), ReadSingle16x16(self), ReadSingle16x16(self));
        }

        internal static void WriteFixedString(this Stream self, string value, int length, Encoding encoding = null)
        {
            byte[] buffer = new byte[length];
            (encoding ?? Encoding.ASCII).GetBytes(value, 0, value.Length, buffer, 0);
            self.Write(buffer, 0, buffer.Length);
        }

        internal static void WritePodString(this Stream self, string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Length > Byte.MaxValue)
                throw new ArgumentException($"String must not be longer than {Byte.MaxValue} characters.");

            self.Write((byte)value.Length);
            for (int i = 0; i < value.Length; i++)
                self.Write((byte)(value[i] ^ ~i));
        }
    }
}
