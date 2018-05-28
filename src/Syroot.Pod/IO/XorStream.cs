using System;
using System.IO;

namespace Syroot.Pod.IO
{
    /// <summary>
    /// Represents a <see cref="Stream"/> overlay using a simple byte-wise XOR encryption applied on every byte.
    /// </summary>
    public class XorStream : Stream
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        public XorStream(Stream baseStream, byte key)
        {
            BaseStream = baseStream;
            Key = key;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public Stream BaseStream { get; set; }

        public byte Key { get; set; }

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => BaseStream.CanSeek;

        public override bool CanWrite => BaseStream.CanWrite;

        public override long Length => BaseStream.Length;

        public override long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------
        
        public override void Flush() => BaseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = BaseStream.Read(buffer, offset, count);
            for (int i = offset; i < offset + bytesRead; i++)
                buffer[i] ^= Key;
            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);

        public override void SetLength(long value) => BaseStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
        {
            // Do not modify the passed-in buffer, create a copy and apply XOR on it.
            byte[] xorBuffer = new byte[count];
            Buffer.BlockCopy(buffer, offset, xorBuffer, 0, count);
            for (int i = 0; i < count; i++)
                xorBuffer[i] ^= Key;
            BaseStream.Write(xorBuffer, 0, count);
        }
    }
}
