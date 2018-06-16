using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a frame on a <see cref="Texture"/> which forms its own visual image.
    /// </summary>
    public class TextureArea : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; } // 32 characters

        public uint Left { get; set; }

        public uint Top { get; set; }

        public uint Right { get; set; }

        public uint Bottom { get; set; }

        public uint Index { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadFixedString(32);
            Left = loader.ReadUInt32();
            Top = loader.ReadUInt32();
            Right = loader.ReadUInt32();
            Bottom = loader.ReadUInt32();
            Index = loader.ReadUInt32();
        }
    }
}
