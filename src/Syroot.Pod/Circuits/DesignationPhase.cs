using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DesignationPhase : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public int MacroIndex { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadPodString();
            MacroIndex = loader.ReadInt32();
        }
    }
}