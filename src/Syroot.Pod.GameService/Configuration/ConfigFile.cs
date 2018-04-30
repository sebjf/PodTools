using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Syroot.Pod.GameService.Configuration
{
    /// <summary>
    /// Represents the gs.bin file storing the available routers hosting Game Service.
    /// </summary>
    public class ConfigFile
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private static readonly byte[] _key = new byte[] { 0x31, 0x07, 0x19, 0x73 };

        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigFile"/> class.
        /// </summary>
        public ConfigFile()
        {
            Username = String.Empty;
            Password = String.Empty;
            RouterName = String.Empty;
            Routers = new List<Router>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class from the given file.
        /// </summary>
        /// <param name="fileName">The file to read the settings from.</param>
        public ConfigFile(string fileName) : this(File.ReadAllBytes(fileName)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class from the given, possibly encrypted
        /// bytes.
        /// </summary>
        /// <param name="bytes">The bytes to read the settings from.</param>
        public ConfigFile(byte[] bytes)
        {
            // Detect if the bytes represent encrypted content.
            bool isEncrypted = false;
            foreach (byte b in bytes)
            {
                // Any byte with a value under 32 which is not a line break is not valid in.
                // decrypted files.
                if (b < 32 && b != '\r' && b != '\n')
                {
                    isEncrypted = true;
                    break;
                }
            }

            // Get the decrypted content.
            string text;
            if (isEncrypted)
                text = Decrypt(bytes);
            else
                text = Encoding.ASCII.GetString(bytes);
            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Detect if the configuration has user settings.
            int numberInLine3 = Int32.Parse(lines[2]);
            bool hasUserConfiguration = numberInLine3 < 17;

            // Parse the possibly existing user configuration.
            int line = 0;
            if (hasUserConfiguration)
            {
                Username = lines[0];
                Password = lines[1];
                Routers = new List<Router>(numberInLine3);
                RouterName = lines[3];
                line = 4;
            }
            else
            {
                Routers = new List<Router>();
            }

            // Parse the router list.
            for (int i = line; i < lines.Length; i += 3)
            {
                string name = lines[i];
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(lines[i + 1]), Int16.Parse(lines[i + 2]));
                Routers.Add(new Router(name, ipEndPoint));
            }
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the pre-defined username used for logging in.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the pre-defined password used for logging in.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the pre-defined router the user will connect to.
        /// </summary>
        public string RouterName { get; set; }

        /// <summary>
        /// Gets or sets the list of router connections in this file.
        /// </summary>
        public IList<Router> Routers { get; set; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the bytes a resulting file would have with the specified options.
        /// </summary>
        /// <param name="includeUserSettings">Determines whether user specific settings will be included in the output.
        /// </param>
        /// <param name="encrypt">Determines whether the contents will be encrypted.</param>
        /// <returns>The bytes a file would have.</returns>
        public byte[] GetBytes(bool includeUserSettings, bool encrypt)
        {
            StringBuilder contentBuilder = new StringBuilder();
            // Write the user specific settings if wanted.
            if (includeUserSettings)
            {
                contentBuilder.AppendLine(Username);
                contentBuilder.AppendLine(Password);
                contentBuilder.AppendLine(Routers.Count.ToString());
                contentBuilder.AppendLine(RouterName);
            }
            // Write the router list.
            foreach (Router router in Routers)
                contentBuilder.AppendLine(router.ToString());
            // Get the content string.
            string content = contentBuilder.ToString().Trim();

            // Encrypt the contents if wanted.
            byte[] bytes;
            if (encrypt)
                bytes = Encrypt(content);
            else
                bytes = Encoding.ASCII.GetBytes(content);

            return bytes;
        }

        /// <summary>
        /// Saves the Gs.bin contents in the file with the specified name and the specified settings. If the target file
        /// exists, it is overwritten.
        /// </summary>
        /// <param name="file">The file in which the contents will be saved.</param>
        /// <param name="includeUserSettings">Determines whether user specific settings will be included in the output.
        /// </param>
        /// <param name="encrypt">Determines whether the contents will be encrypted.</param>
        public void Save(string file, bool includeUserSettings, bool encrypt)
        {
            // Store the bytes into the file.
            File.WriteAllBytes(file, GetBytes(includeUserSettings, encrypt));
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private byte[] Encrypt(string input)
        {
            // Replace line endings with 0xFE.
            input = input.Replace(Environment.NewLine, ((char)0xFE).ToString());

            // Every 4th byte is a check byte, add it to the length.
            int outputLength = input.Length + (input.Length / 4);
            byte[] output = new byte[outputLength];

            // Encode the bytes.
            int size = 0;
            byte checksum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                byte inputByte = (byte)input[i];
                output[size] = (byte)(inputByte ^ _key[size % 4]);
                size++;
                checksum += inputByte;
                // Check byte.
                if ((i + 1) % 4 == 0)
                {
                    output[size] = (byte)(checksum ^ _key[size % 4]);
                    size++;
                    checksum = 0;
                }
            }

            return output;
        }

        private string Decrypt(byte[] input)
        {
            // Every 4th byte is a check byte, add it to the length
            byte[] outputBytes = new byte[input.Length - (input.Length / 5)];

            // Decode the bytes
            int size = 0;
            byte checksum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                byte inputByte = (byte)(input[i] ^ _key[i % 4]);
                if ((i + 1) % 5 == 0)
                {
                    // Validate the checksum
                    if (inputByte != checksum)
                        return Encoding.ASCII.GetString(outputBytes, 0, size - 4);
                    checksum = 0;
                }
                else
                {
                    // Decode a normal byte
                    outputBytes[size++] = inputByte;
                    checksum += inputByte;
                }
            }

            // Replace the 0xFE delimiter with new lines
            for (int i = 0; i < outputBytes.Length; i++)
            {
                if (outputBytes[i] == 0xFE)
                    outputBytes[i] = (byte)'\n';
            }

            // Return with Windows line endings
            return Encoding.ASCII.GetString(outputBytes).Replace("\n", Environment.NewLine);
        }
    }
}
