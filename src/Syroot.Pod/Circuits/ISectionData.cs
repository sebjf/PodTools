using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public interface ISectionData : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        string Name { get; set; }
    }
}