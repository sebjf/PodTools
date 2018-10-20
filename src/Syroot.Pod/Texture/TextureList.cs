using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a list of <see cref="Texture"/> instances which all have the same dimensions.
    /// </summary>
    public class TextureList<T> : List<Texture>, IData<T> where T : PbdfFile, IData<T>, IAssetFile
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<T>.Load(DataLoader<T> loader, object parameter)
        {
            int textureSize = (int)parameter;

            int count = loader.ReadInt32();
            uint flags = loader.ReadUInt32();

            AddRange(loader.LoadMany<Texture<T>>(count));

            switch (loader.Instance.FileType)
            {
                case FileType.BV3:
                case FileType.BV6:
                    throw new System.NotImplementedException();

                case FileType.BV4:
                case FileType.BV7:
                    foreach (Texture texture in this)
                    {
                        texture.Data = loader.ReadUInt16s(128 * 128);
                        texture._Size = 128;
                    }
                    break;

                case FileType.BL4:
                    foreach (Texture texture in this)
                    {
                        texture.Data = loader.ReadUInt16s(textureSize * textureSize);
                        texture._Size = textureSize;
                    }
                    break;
            }
        }

        void IData<T>.Save(DataSaver<T> saver, object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}
