using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents 2-dimensional image data possibly storing multiple <see cref="TextureArea"/> instances.
    /// </summary>
    public class Texture
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public IList<TextureArea> Areas { get; set; }

        public ushort[] Data { get; set; } // 256x256, RGB565

    }

    public class Texture<T> : Texture, IData<T> where T : PbdfFile, IData<T>
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<T>.Load(DataLoader<T> loader, object parameter)
        {
            Areas = loader.LoadMany<TextureArea<T>>(loader.ReadInt32()).Cast<TextureArea>().ToList();
        }

        void IData<T>.Save(DataSaver<T> saver, object parameter)
        {
            throw new System.NotImplementedException();
        }
    }

}
