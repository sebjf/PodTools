using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim2SectionList : List<Anim2Section>, IData<Circuit>
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            int count = (loader.ReadInt32() + 1) / 2;
            while (count-- > 0)
                Add(loader.LoadSection<Anim2Section>());
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(Count * 2);
            foreach (Anim2Section anim2 in this)
                saver.SaveSection(anim2);
        }
    }
}