using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Competitor : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public string Number { get; set; }

        public IList<Vector3U> Unknown3 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadPodString();
            Unknown1 = loader.ReadInt32();
            Unknown2 = loader.ReadInt32();
            Number = loader.ReadPodString();
            Unknown3 = loader.ReadMany(loader.ReadInt32(), () => loader.ReadVector3U());
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WritePodString(Name);
            saver.WriteInt32(Unknown1);
            saver.WriteInt32(Unknown2);
            saver.WritePodString(Number);
            saver.WriteInt32(Unknown3.Count);
            saver.WriteMany(Unknown3, x => saver.WriteVector3U(x));
        }
    }
}