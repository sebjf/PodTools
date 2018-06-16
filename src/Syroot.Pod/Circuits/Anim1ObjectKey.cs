using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents the transformation of a <see cref="Mesh"/> in a <see cref="Anim1Object"/> at a given frame.
    /// </summary>
    public class Anim1ObjectKey : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the rotation matrix of the <see cref="Mesh"/> at this frame.
        /// </summary>
        public Matrix3 Rotation { get; set; }

        /// <summary>
        /// Gets or sets the translation of the <see cref="Mesh"/> at this frame.
        /// </summary>
        public Vector3F Position { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Rotation = loader.ReadMatrix3F16x16();
            Position = loader.ReadVector3F16x16();
        }
    }
}