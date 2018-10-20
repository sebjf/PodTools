using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim1Texture : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the frame at which to start the animation.
        /// </summary>
        public int StartFrame { get; set; }

        public uint Unknown { get; set; }

        /// <summary>
        /// Gets or sets the name of the animated texture.
        /// </summary>
        /// <remarks>If a texture with the same name has not been loaded previously, specific Pod versions crash upon
        /// loading this instance.</remarks>
        public string Name { get; set; }

        public IList<Texture> Textures { get; set; }

        public IList<Anim1TextureConfig> Configs { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Anim1TextureFrame"/> instances which animate each texture by an
        /// <see cref="Anim1TextureFrameKey"/>.
        /// </summary>
        public IList<Anim1TextureFrame> Frames { get; set; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            StartFrame = loader.ReadInt32();
            int frameCount = loader.ReadInt32();
            int configCount = loader.ReadInt32();
            Unknown = loader.ReadUInt32();
            Name = loader.ReadPodString();
            Textures = loader.Load<TextureList<Circuit>>(256);
            Configs = loader.LoadMany<Anim1TextureConfig>(configCount).ToList();
            Frames = loader.LoadMany<Anim1TextureFrame>(frameCount).ToList();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}
