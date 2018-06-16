using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DesignationMacro : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public byte[] Data { get; set; }

        public int MacroIndex1 { get; set; }

        public int MacroIndex2 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Data = loader.ReadBytes(60);
            MacroIndex1 = loader.ReadInt32();
            MacroIndex2 = loader.ReadInt32();
        }
    }
}