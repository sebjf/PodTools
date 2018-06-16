using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents the <see cref="Anim1ObjectKey"/> instances available at a specific time frame.
    /// </summary>
    public class Anim1ObjectFrame : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the list of <see cref="Anim1ObjectKey"/> instances which set the transformation of each
        /// <see cref="Mesh"/>.
        /// </summary>
        public IList<Anim1ObjectKey> Keys { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            int meshCount = (int)parameter;

            Keys = new Anim1ObjectKey[meshCount];
            for (int i = 0; i < meshCount; i++)
            {
                if (loader.ReadBoolean(BooleanCoding.Dword))
                    Keys[i] = loader.Load<Anim1ObjectKey>();
            }
        }
    }
}
