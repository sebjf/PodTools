using System.Collections.Generic;
using System.IO;
using Syroot.BinaryData;

namespace Syroot.Pod
{
    /// <summary>
    /// Represents CIRCUIT.BIN files.
    /// </summary>
    public class CircuitsScript
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitsScript"/> class.
        /// </summary>
        public CircuitsScript()
        {
            Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitsScript"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to create the instance from.</param>
        public CircuitsScript(string fileName)
        {
            Load(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitsScript"/> class from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to create the instance from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after creating the instance.
        /// </param>
        public CircuitsScript(Stream stream, bool leaveOpen = false)
        {
            Load(stream, leaveOpen);
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the circuits available in the game. Should not exceed 96 to prevent runtime errors.
        /// </summary>
        public IList<CircuitInfo> CircuitInfos { get; set; }

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
            // Get the unencrypted number of circuit infos stored in this file.
            uint count = stream.ReadUInt32();
            CircuitInfos = new List<CircuitInfo>((int)count);
            // Read the XOR byte encrypted circuit infos.
            while (count-- > 0)
            {
                CircuitInfos.Add(new CircuitInfo(stream));
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
            // Write the unencrypted number of circuit infos stored in this file.
            stream.Write(CircuitInfos.Count);
            // Write the XOR byte encrypted circuit infos.
            foreach (CircuitInfo circuitInfo in CircuitInfos)
            {
                stream.Write(circuitInfo.ToBytes());
            }
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private void Reset()
        {
            CircuitInfos = null;
        }
    }
}
