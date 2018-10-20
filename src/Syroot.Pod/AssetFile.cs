using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syroot.Pod
{
    public enum FileType
    {
        BV3,
        BV4,
        BV6,
        BV7,
        BL4
    }

    public enum PodVersion
    {
        POD1,
        POD2
    }

    public interface IAssetFile
    {
        FileType FileType { get; }
        PodVersion PodVersion { get; set; }
    }

}
