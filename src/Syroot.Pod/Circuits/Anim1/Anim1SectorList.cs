using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim1SectorList : List<Anim1Sector>, IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public uint Unknown1 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            int count = loader.ReadInt32();
            Unknown1 = loader.ReadUInt32();
            AddRange(loader.LoadMany<Anim1Sector>(count).ToList());
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(Count);
            saver.Write(Unknown1);
            saver.SaveMany(this);
        }
    }
}
