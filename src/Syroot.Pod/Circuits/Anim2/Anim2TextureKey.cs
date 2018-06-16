using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim2TextureKey : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int TextureIndex { get; set; }

        public Vector2U[] TexCoords { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            TextureIndex = loader.ReadInt32();
            TexCoords = loader.ReadMany(4, () => loader.ReadVector2U());
        }
    }
}