using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class LevelSection : ISectionData
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public IList<LevelConfig1> LevelConfig1s { get; set; }

        public IList<LevelConfig2> LevelConfig2s { get; set; }

        public IList<LevelConfig3> LevelConfig3s { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Unknown1 = loader.ReadInt32();
            int levelConfig1Count = loader.ReadInt32();
            int levelConfig2Count = loader.ReadInt32();
            int levelConfig3Count = loader.ReadInt32();
            Unknown2 = loader.ReadInt32();
            LevelConfig1s = loader.LoadMany<LevelConfig1>(levelConfig1Count).ToList();
            LevelConfig2s = loader.LoadMany<LevelConfig2>(levelConfig2Count).ToList();
            LevelConfig3s = loader.LoadMany<LevelConfig3>(levelConfig3Count).ToList();
        }
    }
}