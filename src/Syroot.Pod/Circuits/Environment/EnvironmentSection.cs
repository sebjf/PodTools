using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class EnvironmentSection : ISectionData
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public IList<Macro> Macros { get; set; }

        public IList<Decoration> Decorations { get; set; }

        public IList<DecorationInstance> DecorationInstances { get; set; }

        public IList<IList<DecorationInstance>> SectorDecorationInstances { get; set; }
        
        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Macros = loader.LoadMany<Macro>(loader.ReadInt32(), 3).ToList();
            Decorations = loader.LoadMany<Decoration>(loader.ReadInt32()).ToList();
            DecorationInstances = loader.LoadMany<DecorationInstance>(loader.ReadInt32()).ToList();

            SectorDecorationInstances = new List<IList<DecorationInstance>>(loader.Instance.Sectors.Count);
            for (int i = 0; i < loader.Instance.Sectors.Count; i++)
                SectorDecorationInstances.Add(loader.LoadMany<DecorationInstance>(loader.ReadInt32()).ToList());
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(Macros.Count);
            saver.SaveMany(Macros);
            saver.WriteInt32(Decorations.Count);
            saver.SaveMany(Decorations);
            saver.WriteInt32(DecorationInstances.Count);
            saver.SaveMany(DecorationInstances);

            foreach (IList<DecorationInstance> sectorDecorations in SectorDecorationInstances)
            {
                saver.WriteInt32(sectorDecorations.Count);
                saver.SaveMany(sectorDecorations);
            }
        }
    }
}
