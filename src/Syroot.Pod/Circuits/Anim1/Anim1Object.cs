using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a list of animated <see cref="Mesh"/> instances in which each set frame represents a transformation.
    /// </summary>
    public class Anim1Object : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the frame at which to start the animation.
        /// </summary>
        public int StartFrame { get; set; }

        public bool HasNamedFaces { get; set; }

        /// <summary>
        /// Gets or sets the name of the animated object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Mesh"/> instances animated in each <see cref="Anim1ObjectFrame"/>.
        /// </summary>
        public IList<Mesh> Meshes { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Anim1ObjectFrame"/> instances which animate each mesh by an
        /// <see cref="Anim1ObjectKey"/>.
        /// </summary>
        public IList<Anim1ObjectFrame> Frames { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            StartFrame = loader.ReadInt32();
            int frameCount = loader.ReadInt32();
            HasNamedFaces = loader.ReadBoolean(BooleanCoding.Dword);
            int meshCount = loader.ReadInt32();
            Name = loader.ReadPodString();
            Meshes = loader.LoadMany<Mesh<Circuit>>(meshCount, new MeshFaceParameters
            {
                HasNamedFaces = HasNamedFaces,
                HasUnkProperty = false
            }).Cast<Mesh>().ToList();
            Frames = loader.LoadMany<Anim1ObjectFrame>(frameCount, meshCount).ToList();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}
