using System;
using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Cars
{
    /// <summary>
    /// Represents a list of <see cref="Texture"/> instances which all have the same dimensions.
    /// </summary>
    public class TextureList : List<Texture>, IData<Car>
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Car>.Load(DataLoader<Car> loader, object parameter)
        {
            int count = loader.ReadInt32();
            uint flags = loader.ReadUInt32();
            AddRange(loader.LoadMany<Texture>(count));

            switch ((parameter as CarFileInfo).FileVersion)
            {
                case FileVersion.BV3:
                case FileVersion.BV6:
                    throw new NotImplementedException();
                    break;

                case FileVersion.BV4:
                case FileVersion.BV7:
                    foreach (Texture texture in this)
                    {
                        texture.Data = loader.ReadUInt16s(128 * 128);
                    }
                    break;
            }
        }

        void IData<Car>.Save(DataSaver<Car> saver, object parameter)
        {
            saver.WriteInt32(Count);
            saver.WriteInt32(0);
            saver.SaveMany(this);
            foreach (Texture texture in this)
            {
                saver.WriteUInt16s(texture.Data);
            }
        }
    }
}
