using System.Collections.Generic;
using System.Diagnostics;
using Syroot.Maths;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a collection of <see cref="Mesh"/> instances which form a complete 3-dimensional model.
    /// </summary>
    public class Model
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public bool HasNamedFaces { get; set; }
        public IList<Mesh> Meshes { get; set; }
    }

    /// <summary>
    /// Represents the surface of a logically connected part in a <see cref="Model"/>.
    /// </summary>
    public class Mesh
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public IList<Vector3U> Positions { get; set; }
        public IList<Face> Faces { get; set; }
        public IList<Vector3F> Normals { get; set; }
        public uint Unknown { get; set; }
        public IList<byte> VertexLights { get; set; }
        public Vector3F BoundingBoxMin { get; set; } // Z -= 2
        public Vector3F BoundingBoxMax { get; set; } // Z += 10
    }

    /// <summary>
    /// Represents a polygon connected by multiple vertices, forming the surface of a <see cref="Mesh"/>.
    /// </summary>
    [DebuggerDisplay(nameof(Face) + " {Name}")]
    public class Face
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        public Face()
        {
            Indices = new uint[4];
            TexCoords = new Vector2U[4];
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }
        public uint[] Indices { get; }
        public Vector3F Normal { get; set; }
        public string MaterialType { get; set; }
        public uint ColorOrTexIndex { get; set; }
        public Vector2U[] TexCoords { get; }
        public uint Reserved { get; set; }
        public Vector3F QuadReserved { get; set; }
        public uint Unknown { get; set; }
        public uint Properties { get; set; }
    }
}
