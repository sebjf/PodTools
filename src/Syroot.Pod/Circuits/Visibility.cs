using System.Collections.Generic;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a list of indices referencing <see cref="Mesh"/> instances of a <see cref="Model"/> which are
    /// shown if this visibility group is active.
    /// </summary>
    public class Visibility
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public IList<uint> VisibleMeshes { get; set; }
    }
}
