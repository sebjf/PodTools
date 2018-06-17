using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Macro : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets values configuring the macro.
        /// </summary>
        public IList<uint> Parameters { get; set; } = new List<uint>();

        // ---- METHODS ------------------------------------------------------------------------------------------------
        
        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Parameters = loader.ReadUInt32s((int)parameter);
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteUInt32s(Parameters);
        }
    }
}
