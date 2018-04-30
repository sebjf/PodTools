using System;
using System.IO;
using System.Text;
using Syroot.BinaryData;
using Syroot.Maths;

namespace Syroot.Pod.Core
{
    /// <summary>
    /// Represents extension methods for <see cref="Stream"/> instances.
    /// </summary>
    internal static class StreamExtensions
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private static readonly char[] _fixedStringTerminator = new[] { '\0' };

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        internal static string ReadFixedString(this Stream stream, int length, Encoding encoding = null)
        {
            byte[] buffer = new byte[length];
            int readBytes = stream.Read(buffer, 0, length);
            if (readBytes < length)
                throw new InvalidDataException("Incomplete fixed string.");
            return (encoding ?? Encoding.ASCII).GetString(buffer).TrimEnd(_fixedStringTerminator);
        }

        internal static string ReadPodString(this Stream stream)
        {
            char[] chars = new char[stream.ReadByte()];
            for (int i = 0; i < chars.Length; i++)
                chars[i] = (char)((byte)(stream.ReadByte() ^ ~i));
            return new string(chars);
        }

        internal static Single ReadSingle16x16(this Stream stream)
        {
            return stream.ReadInt32() / (Single)(1 << 16);
        }

        internal static Vector2U ReadVector2U(this Stream stream)
        {
            return new Vector2U(stream.ReadUInt32(), stream.ReadUInt32());
        }

        internal static Vector3U ReadVector3U(this Stream stream)
        {
            return new Vector3U(stream.ReadUInt32(), stream.ReadUInt32(), stream.ReadUInt32());
        }

        internal static Vector3F ReadVector3F16x16(this Stream stream)
        {
            return new Vector3F(ReadSingle16x16(stream), ReadSingle16x16(stream), ReadSingle16x16(stream));
        }

        internal static void WriteFixedString(this Stream stream, string value, int length, Encoding encoding = null)
        {
            byte[] buffer = new byte[length];
            (encoding ?? Encoding.ASCII).GetBytes(value, 0, value.Length, buffer, 0);
            stream.Write(buffer, 0, buffer.Length);
        }

        internal static void WritePodString(this Stream stream, string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Length > Byte.MaxValue)
                throw new ArgumentException($"String must not be longer than {Byte.MaxValue} characters.");

            stream.Write((byte)value.Length);
            for (int i = 0; i < value.Length; i++)
                stream.Write((byte)(value[i] ^ ~i));
        }
    }
}
