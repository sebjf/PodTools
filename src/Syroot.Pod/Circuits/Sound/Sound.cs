﻿using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Sound : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public uint[] Data { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Data = loader.ReadUInt32s(14);
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteUInt32s(Data);
        }
    }
}