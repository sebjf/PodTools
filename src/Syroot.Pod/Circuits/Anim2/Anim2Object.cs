using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim2Object : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int AnimIndex { get; set; }

        public IList<Anim2ObjectFrame> Frames { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            AnimIndex = loader.ReadInt32();
            Frames = loader.LoadMany<Anim2ObjectFrame>(loader.ReadInt32()).ToList();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(AnimIndex);
            saver.WriteInt32(Frames.Count);
            saver.SaveMany(Frames);
        }
    }
}