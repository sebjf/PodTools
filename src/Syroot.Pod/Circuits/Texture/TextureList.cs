using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a list of <see cref="Texture"/> instances which all have the same dimensions.
    /// </summary>
    public class TextureList : List<Texture>, IData<Circuit>
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            int textureSize = (int)parameter;

            int count = loader.ReadInt32();
            uint reserved = loader.ReadUInt32();
            AddRange(loader.LoadMany<Texture>(count));
            foreach (Texture texture in this)
                texture.Data = loader.ReadUInt16s(textureSize * textureSize);
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(Count);
            saver.WriteInt32(0);
            saver.SaveMany(this);
            foreach (Texture texture in this)
                saver.WriteUInt16s(texture.Data);
        }
    }
}
