using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents the surface of a polygonal model.
    /// </summary>
    public class Mesh : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public IList<Vector3F> Positions { get; set; }

        public IList<MeshFace> Faces { get; set; }

        public IList<Vector3F> Normals { get; set; }

        /// <summary>
        /// Gets or sets the virtual size of the mesh. If too low, the mesh tends to become invisible too early.
        /// </summary>
        public float Volume { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            MeshFaceParameters parameters = (MeshFaceParameters)parameter;

            int vertexCount = loader.ReadInt32();
            Positions = loader.ReadMany(vertexCount, () => loader.ReadVector3F16x16());
            int faceCount = loader.ReadInt32();
            int triCount = loader.ReadInt32();
            int quadCount = loader.ReadInt32();
            Faces = loader.LoadMany<MeshFace>(faceCount, parameters).ToList();
            Normals = loader.ReadMany(vertexCount, () => loader.ReadVector3F16x16());
            Volume = loader.ReadSingle16x16();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            MeshFaceParameters parameters = (MeshFaceParameters)parameter;

            saver.WriteInt32(Positions.Count);
            saver.WriteMany(Positions, x => saver.WriteVector3F16x16(x));
            saver.WriteInt32(Faces.Count);
            saver.WriteInt32(Faces.Where(x => x.FaceVertexCount == 3).Count());
            saver.WriteInt32(Faces.Where(x => x.FaceVertexCount == 4).Count());
            saver.SaveMany(Faces, parameters);
            saver.WriteMany(Normals, x => saver.WriteVector3F16x16(x));
            saver.WriteSingle16x16(Volume);
        }
    }
}
