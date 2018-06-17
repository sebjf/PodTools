using System.Diagnostics;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents extension methods for <see cref="DataSaver{Circuit}"/> instances.
    /// </summary>
    [DebuggerStepThrough]
    public static class DataSaverExtensions
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private const string _noneName = "NEANT";

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Saves an <see cref="ISectionData"/> instance of type <typeparamref name="T"/>, writing "NEANT" if the
        /// instance is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ISectionData"/> to save.</typeparam>
        /// <param name="self">The extended <see cref="DataSaver{T}"/> instance.</param>
        /// <param name="value">The instance to save.</param>
        /// <param name="parameter">The optional parameter to pass in to the saving instance.</param>
        public static void SaveSection<T>(this DataSaver<Circuit> self, T value, object parameter = null)
            where T : ISectionData, new()
        {
            if (value == default)
            {
                self.WritePodString(_noneName);
            }
            else
            {
                self.WritePodString(value.Name);
                value.Save(self, parameter);
            }
        }

        /// <summary>
        /// Saves an <see cref="IDifficultySectionData"/> instance of type <typeparamref name="T"/>, writing "NEANT" if
        /// the instance is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IDifficultySectionData"/> to save.</typeparam>
        /// <param name="self">The extended <see cref="DataSaver{T}"/> instance.</param>
        /// <param name="value">The instance to save.</param>
        /// <param name="difficultyName">The name of the difficulty.</param>
        public static void SaveDifficultySection<T>(this DataSaver<Circuit> self, T value, string difficultyName)
            where T : IDifficultySectionData, new()
        {
            if (value == null)
            {
                self.WritePodString(difficultyName);
                self.WritePodString(_noneName);
            }
            else
            {
                self.WritePodString(value.DifficultyName);
                self.WritePodString(value.Name);
                value.Save(self);
            }
        }
    }
}
