using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim2ObjectFrame : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int SectorIndex { get; set; }

        public IList<Anim2ObjectKey> Keys { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            SectorIndex = loader.ReadInt32();
            Keys = loader.LoadMany<Anim2ObjectKey>(loader.ReadInt32()).ToList();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(SectorIndex);
            saver.WriteInt32(Keys.Count);
            saver.SaveMany(Keys);
        }
    }
}