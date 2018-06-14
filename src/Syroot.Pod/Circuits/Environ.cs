using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Environ : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public IList<Macro> Macros { get; set; }

        public IList<Decoration> Decorations { get; set; }

        public IList<DecorationInstance> DecorationInstances { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadPodString();
            if (Name != "NEANT")
            {
                Macros = loader.LoadMany<Macro>(loader.ReadInt32(), 3).ToList();
                Decorations = loader.LoadMany<Decoration>(loader.ReadInt32()).ToList();
                DecorationInstances = loader.LoadMany<DecorationInstance>(loader.ReadInt32()).ToList();
                // TODO
            }
        }
    }
}
