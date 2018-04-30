using System;
using System.Net;

namespace Syroot.Pod.GameService.Configuration
{
    /// <summary>
    /// Represents the connection details for a single router in a Gs.bin file.
    /// </summary>
    public class Router
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class with the specified details.
        /// </summary>
        /// <param name="name">The name as displayed in the GameService menu.</param>
        /// <param name="ipEndPoint">The IP address under which the router is accessible.</param>
        public Router(string name, IPEndPoint ipEndPoint)
        {
            Name = name;
            IPEndPoint = ipEndPoint;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the name of the router as displayed in the GameService menu.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the IP address under which the router is accessible.
        /// </summary>
        public IPEndPoint IPEndPoint { get; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Gets three lines of text representing the router details as in the Gs.bin file.
        /// </summary>
        /// <returns>The GameService settings file compatible description of this router.</returns>
        public override string ToString()
        {
            return String.Join(Environment.NewLine, new[]
            {
                Name,
                IPEndPoint.Address.ToString(),
                IPEndPoint.Port.ToString()
            });
        }
    }
}
