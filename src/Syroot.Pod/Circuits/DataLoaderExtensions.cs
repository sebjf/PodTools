using System;
using System.Diagnostics;
using System.Globalization;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents extension methods for <see cref="DataLoader{Circuit}"/> instances.
    /// </summary>
    [DebuggerStepThrough]
    public static class DataLoaderExtensions
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private const string _noneName = "NEANT";

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Loads an <see cref="ISectionData"/> instance of type <typeparamref name="T"/>, returning <c>null</c> if the
        /// name is "NEANT".
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ISectionData"/> to load.</typeparam>
        /// <param name="self">The extended <see cref="DataLoader{T}"/> instance.</param>
        /// <param name="parameter">The optional parameter to pass in to the loading instance.</param>
        /// <returns>The loaded section instance or <see langword="null"/> if the name was "NEANT".</returns>
        public static T LoadSection<T>(this DataLoader<Circuit> self, object parameter = null)
            where T : ISectionData, new()
        {
            string name = self.ReadPodString();
            if (String.Compare(name, _noneName, true, CultureInfo.InvariantCulture) == 0)
            {
                return default;
            }
            else
            {
                T data = new T();
                data.Name = name;
                data.Load(self, parameter);
                return data;
            }
        }
    }
}
