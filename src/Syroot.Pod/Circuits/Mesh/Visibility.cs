using System;
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

        public IList<uint> VisibleMeshes { get; set; } = new List<uint>();

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            VisibleMeshes = loader.ReadUInt32s(Math.Max(0, loader.ReadInt32()));
        }
    }
}
