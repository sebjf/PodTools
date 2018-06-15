using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents the <see cref="Anim1TextureKey"/> instances available at a specific time frame.
    /// </summary>
    public class Anim1TextureFrame : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public Vector3U Unknown { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Anim1TextureKey"/> instances which set the transformation of each
        /// <see cref="Texture"/>.
        /// </summary>
        public IList<Anim1TextureKey> Keys { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Unknown = loader.ReadVector3U();
            Keys = loader.LoadMany<Anim1TextureKey>(loader.ReadInt32()).ToList();
        }
    }
}