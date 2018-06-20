using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a list of indices referencing <see cref="Sector"/> instances of a <see cref="SectorList"/> which are
    /// shown if this visibility group is active.
    /// </summary>
    public class Visibility : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the indices of other sectors visible when this one is active, e.g. the player car is colliding with it.
        /// If <see langword="null"/>, collisions with this sector and resulting other sectors are completely ignored.
        /// If 0, only this sector is visible when active.
        /// </summary>
        public IList<int> VisibleSectorIndices { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            int count = loader.ReadInt32();
            if (count > -1)
                VisibleSectorIndices = loader.ReadInt32s(count);
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            if (VisibleSectorIndices == null)
            {
                saver.WriteInt32(-1);
            }
            else
            {
                saver.WriteInt32(VisibleSectorIndices.Count);
                saver.WriteInt32s(VisibleSectorIndices);
            }
        }
    }
}
