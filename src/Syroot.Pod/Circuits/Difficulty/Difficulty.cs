using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Difficulty : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public LevelSection LevelSection { get; set; }

        public DifficultyConfigSection ConfigSection { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadPodString();
            LevelSection = loader.LoadSection<LevelSection>();
            ConfigSection = loader.LoadSection<DifficultyConfigSection>();
        }
    }
}