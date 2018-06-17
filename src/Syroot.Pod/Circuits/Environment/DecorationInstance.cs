using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DecorationInstance : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int Index { get; set; }

        public Vector2U[] Vectors { get; set; }

        public Vector3F Position { get; set; }

        public Matrix3 Rotation { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Index = loader.ReadInt32();
            Vectors = loader.ReadMany(loader.ReadInt32(), () => loader.ReadVector2U());
            Position = loader.ReadVector3F16x16();
            Rotation = loader.ReadMatrix3F16x16();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(Index);
            saver.WriteInt32(Vectors.Length);
            saver.WriteMany(Vectors, x => saver.WriteVector2U(x));
            saver.WriteVector3F16x16(Position);
            saver.WriteMatrix3F16x16(Rotation);
        }
    }
}