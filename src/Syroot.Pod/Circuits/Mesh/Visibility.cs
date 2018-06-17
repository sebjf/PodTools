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
        /// May be null to write -1 count instead of 0.
        /// </summary>
        public IList<uint> VisibleMeshes { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            int count = loader.ReadInt32();
            if (count > -1)
                VisibleMeshes = loader.ReadUInt32s(count);
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            if (VisibleMeshes == null)
            {
                saver.WriteInt32(-1);
            }
            else
            {
                saver.WriteInt32(VisibleMeshes.Count);
                saver.WriteUInt32s(VisibleMeshes);
            }
        }
    }
}
