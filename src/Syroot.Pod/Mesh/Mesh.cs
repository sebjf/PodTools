using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod
{
    public class Mesh
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public IList<Vector3F> Positions { get; set; }

        public IList<MeshFace> Faces { get; set; }

        public IList<Vector3F> Normals { get; set; }

        public sbyte[] Prism { get; set; }

        /// <summary>
        /// Gets or sets the virtual size of the mesh. If too low, the mesh tends to become invisible too early.
        /// </summary>
        public float Volume { get; set; }
    }

    /// <summary>
    /// Represents the surface of a polygonal model.
    /// </summary>
    public class Mesh<T> : Mesh, IData<T> where T : PbdfFile, IData<T>, IAssetFile
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<T>.Load(DataLoader<T> loader, object parameter)
        {
            MeshFaceParameters parameters = (MeshFaceParameters)parameter;

            int vertexCount = loader.ReadInt32();
            Positions = loader.ReadMany(vertexCount, () => loader.ReadVector3F16x16());
            int faceCount = loader.ReadInt32();
            int triCount = loader.ReadInt32();
            int quadCount = loader.ReadInt32();
            Faces = loader.LoadMany<MeshFace<T>>(faceCount, parameters).Cast<MeshFace>().ToList();
            Normals = loader.ReadMany(vertexCount, () => loader.ReadVector3F16x16());
            Volume = loader.ReadSingle16x16();

            if (parameters.HasPrism)
            {
                Prism = loader.ReadSBytes(28);
            }
        }

        void IData<T>.Save(DataSaver<T> saver, object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}
