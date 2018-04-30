using System.Collections.Generic;
using System.IO;
using Syroot.BinaryData;

namespace Syroot.Pod
{
    /// <summary>
    /// Represents VOITUTRES.BIN, VOITURE2.BIN and VOIRESET.BIN files.
    /// </summary>
    public class VehiclesScript
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclesScript"/> class.
        /// </summary>
        public VehiclesScript()
        {
            Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclesScript"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to create the instance from.</param>
        public VehiclesScript(string fileName)
        {
            Load(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclesScript"/> class from the given
        /// <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to create the instance from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after creating the instance.
        /// </param>
        public VehiclesScript(Stream stream, bool leaveOpen = false)
        {
            Load(stream, leaveOpen);
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the vehicles available in the game. Should not exceed 8 for VOITURES.BIN and VOIRESET.BIN, and 32 for
        /// VOIRTURE2.BIN. Should have at least 2 or the game crashes with AI opponents.
        /// </summary>
        public IList<VehicleInfo> VehicleInfos { get; set; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Loads the instance data from the file with the given <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to load the instance data from.</param>
        public void Load(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Load(fileStream);
            }
        }

        /// <summary>
        /// Loads the instance data from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load the instance data from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after loading the instance.
        /// </param>
        public void Load(Stream stream, bool leaveOpen = false)
        {
            Reset();
            // Get the unencrypted number of vehicle infos stored in this file.
            uint count = stream.ReadUInt32();
            VehicleInfos = new List<VehicleInfo>((int)count);
            // Read the XOR byte encrypted vehicle infos.
            while (count-- > 0)
            {
                VehicleInfos.Add(new VehicleInfo(stream));
            }
        }

        /// <summary>
        /// Saves the instance data in the file with the given <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to save the instance data in.</param>
        public void Save(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                Save(fileStream);
            }
        }

        /// <summary>
        /// Saves the instance data in the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to save the instance data in.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after saving the instance.
        /// </param>
        public void Save(Stream stream, bool leaveOpen = false)
        {
            // Write the unencrypted number of vehicle infos stored in this file.
            stream.Write(VehicleInfos.Count);
            // Write the XOR byte encrypted vehicle infos.
            foreach (VehicleInfo circuitInfo in VehicleInfos)
            {
                stream.Write(circuitInfo.ToBytes());
            }
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private void Reset()
        {
            VehicleInfos = null;
        }
    }
}
