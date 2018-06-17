using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class RepairZone : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public Vector3F[] Positions { get; set; }

        public Vector3F CenterPosition { get; set; }

        public float Height { get; set; }

        public float Delay { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Positions = loader.ReadMany(4, () => loader.ReadVector3F16x16());
            CenterPosition = loader.ReadVector3F16x16();
            Height = loader.ReadSingle16x16();
            Delay = loader.ReadSingle16x16();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteMany(Positions, x => saver.WriteVector3F16x16(x));
            saver.WriteVector3F16x16(CenterPosition);
            saver.WriteSingle16x16(Height);
            saver.WriteSingle16x16(Delay);
        }
    }
}