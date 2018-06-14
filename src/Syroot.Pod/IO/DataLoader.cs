using System.Collections.Generic;
using System.IO;

namespace Syroot.Pod.IO
{
    /// <summary>
    /// Represents a generic data loader which passes the constructed instance to all <see cref="IData{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of the constructed instance.</typeparam>
    public class DataLoader<T> : StreamWrapper
        where T : IData<T>
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private readonly Stream _stream;

        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoader{T}"/> reading data from the given
        /// <paramref name="stream"/>, storing data in the provided <paramref name="instance"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load data from.</param>
        /// <param name="instance">The instance to store data in.</param>
        public DataLoader(Stream stream, T instance)
            : base(stream)
        {
            _stream = stream;
            Instance = instance;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the constructed instance.
        /// </summary>
        public T Instance { get; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Starts loading the instance of type <typeparamref name="T"/>.
        /// </summary>
        public void Execute()
        {
            Instance.Load(this);
        }

        /// <summary>
        /// Loads and returns the <see cref="IData{T}"/> instance.
        /// </summary>
        /// <typeparam name="TData">The type of the <see cref="IData{T}"/> to load.</typeparam>
        /// <param name="parameter">The optional parameter to pass in to the loading instance.</param>
        /// <returns>The loaded instance.</returns>
        public TData Load<TData>(object parameter = null)
            where TData : IData<T>, new()
        {
            TData data = new TData();
            data.Load(this, parameter);
            return data;
        }

        /// <summary>
        /// Loads and returns <paramref name="count"/> instances <see cref="IData{T}"/> instances.
        /// </summary>
        /// <typeparam name="TData">The type of the <see cref="IData{T}"/> to load.</typeparam>
        /// /// <param name="parameter">The optional parameter to pass in to all loading instances.</param>
        /// <returns>The loaded instances.</returns>
        public IEnumerable<TData> LoadMany<TData>(int count, object parameter = null)
            where TData : IData<T>, new()
        {
            while (count-- > 0)
                yield return Load<TData>(parameter);
        }
    }

    /// <summary>
    /// Represents data loadable by a <see cref="DataLoader{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the instance passed by the data loader.</typeparam>
    public interface IData<T>
        where T : IData<T>
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        /// <summary>
        /// Loads data from the <paramref name="loader"/> into the instance.
        /// </summary>
        /// <param name="loader">The <see cref="DataLoader{T}"/> to load data with.</param>
        /// <param name="parameter">The optional parameter passed in to the instance.</param>
        void Load(DataLoader<T> loader, object parameter = null);
    }
}
