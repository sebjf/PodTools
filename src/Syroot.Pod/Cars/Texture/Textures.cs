using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Cars
{
    /// <summary>
    /// Represents 2-dimensional image data possibly storing multiple <see cref="TextureArea"/> instances.
    /// </summary>
    public class Texture : IData<Car>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public IList<TextureArea> Areas { get; set; }

        public ushort[] Data { get; set; } // 256x256, RGB565

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Car>.Load(DataLoader<Car> loader, object parameter)
        {
            Areas = loader.LoadMany<TextureArea>(loader.ReadInt32()).ToList();
        }

        void IData<Car>.Save(DataSaver<Car> saver, object parameter)
        {
            saver.WriteInt32(Areas.Count);
            saver.SaveMany(Areas);
        }
    }
}
