using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim1Section : ISectionData
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public IList<Macro> Macros { get; set; }

        public IList<Anim1> Animations { get; set; }

        public Anim1SectorList GlobalSector { get; set; }

        public IList<Anim1SectorList> Sectors { get; set; }

        public uint Unknown { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Macros = loader.LoadMany<Macro>(loader.ReadInt32(), 3).ToList();
            Animations = loader.LoadMany<Anim1>(loader.ReadInt32()).ToList();
            Unknown = loader.ReadUInt32();
            if (loader.Instance.Sectors.Count > 0)
                GlobalSector = loader.Load<Anim1SectorList>();
            Sectors = loader.LoadMany<Anim1SectorList>(loader.Instance.Sectors.Count).ToList();
        }
    }
}
