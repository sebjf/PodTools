using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim2Texture : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public IList<Anim2TextureKey> Keys { get; set; }

        public float TotalTime { get; set; }

        public IList<Anim2TextureFrame> Frames { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadPodString();
            Keys = loader.LoadMany<Anim2TextureKey>(loader.ReadInt32()).ToList();
            TotalTime = loader.ReadSingle16x16();
            Frames = loader.LoadMany<Anim2TextureFrame>(loader.ReadInt32()).ToList();
        }
    }
}