using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents the transformation of a <see cref="Mesh"/> in a <see cref="Anim1Object"/> at a given frame.
    /// </summary>
    public class Anim1TextureKey : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public uint Unknown1 { get; set; }

        public Vector2U Unknown2 { get; set; }

        public Vector2U Unknown3 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Unknown1 = loader.ReadUInt32();
            Unknown2 = loader.ReadVector2U();
            Unknown3 = loader.ReadVector2U();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteUInt32(Unknown1);
            saver.WriteVector2U(Unknown2);
            saver.WriteVector2U(Unknown3);
        }
    }
}