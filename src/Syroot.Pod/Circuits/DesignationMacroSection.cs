using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DesignationMacroSection : ISectionData
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public IList<Macro> Macros { get; set; }

        public IList<DesignationMacro> DesignationMacros { get; set; }

        public IList<DesignationValue> DesignationValues { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Macros = loader.LoadMany<Macro>(loader.ReadInt32(), 3).ToList();
            DesignationMacros = loader.LoadMany<DesignationMacro>(loader.ReadInt32()).ToList();
            DesignationValues = loader.LoadMany<DesignationValue>(loader.ReadInt32()).ToList();
        }
    }
}