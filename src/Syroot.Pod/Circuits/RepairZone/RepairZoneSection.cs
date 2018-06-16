using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class RepairZoneSection : ISectionData
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public IList<RepairZone> RepairZones { get; set; }

        public float Time { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            RepairZones = loader.LoadMany<RepairZone>(loader.ReadInt32()).ToList();
            Time = loader.ReadSingle16x16();
        }
    }
}