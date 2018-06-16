using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DifficultyConfigSection : ISectionData
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public IList<DifficultyConfig1> Config1s { get; set; }

        public IList<DifficultyConfig2> Config2s { get; set; }

        public IList<DifficultyConfig3> Config3s { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Unknown1 = loader.ReadInt32();
            int config1Count = loader.ReadInt32();
            int config2Count = loader.ReadInt32();
            int config3Count = loader.ReadInt32();
            Unknown2 = loader.ReadInt32();
            Config1s = loader.LoadMany<DifficultyConfig1>(config1Count).ToList();
            Config2s = loader.LoadMany<DifficultyConfig2>(config2Count).ToList();
            Config3s = loader.LoadMany<DifficultyConfig3>(config3Count).ToList();
        }
    }
}