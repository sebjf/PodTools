using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class CompetitorSection : IDifficultySectionData
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string DifficultyName { get; set; }

        public string Name { get; set; }

        public IList<Competitor> Competitors { get; set; }
        
        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Competitors = loader.LoadMany<Competitor>(loader.ReadInt32()).ToList();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(Competitors.Count);
            saver.SaveMany(Competitors);
        }
    }
}
