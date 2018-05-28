using System;
using System.IO;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod
{
    /// <summary>
    /// Represents BMD files.
    /// </summary>
    public class MessageDictionary : PbdfFile
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        private const int _blockSize = 0x0000200;
        private const uint _key = 0x0000EA1E;

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        protected override void LoadData(Stream stream)
        {
            uint typeCount = stream.ReadUInt32();
            uint textCount = stream.ReadUInt32();
            uint textBufferSize = stream.ReadUInt32();
            byte[] textBuffer = stream.ReadBytes((int)textBufferSize);
        }

        protected override void SaveData(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
