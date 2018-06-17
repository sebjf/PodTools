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

        public int Left { get; set; }

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public int Index { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadFixedString(32);
            Left = loader.ReadInt32();
            Top = loader.ReadInt32();
            Right = loader.ReadInt32();
            Bottom = loader.ReadInt32();
            Index = loader.ReadInt32();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteFixedString(Name, 32);
            saver.WriteInt32(Left);
            saver.WriteInt32(Top);
            saver.WriteInt32(Right);
            saver.WriteInt32(Bottom);
            saver.WriteInt32(Index);
        }
    }
}
