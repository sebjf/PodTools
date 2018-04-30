namespace Syroot.Pod.Circuits
{
    public class Event
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public byte[][] ParamData { get; set; }
    }

    public class Macro
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Macro"/> class with the given <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters">The list of parameters configuring the macro.</param>
        public Macro(uint[] parameters)
        {
            Parameters = parameters;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets values configuring the macro.
        /// </summary>
        public uint[] Parameters { get; }
    }
}
