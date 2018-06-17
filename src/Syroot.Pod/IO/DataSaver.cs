using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Syroot.Pod.IO
{
    /// <summary>
    /// Represents a generic data saver which passes the deserialized instance to all <see cref="IData{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of the deserialized instance.</typeparam>
    [DebuggerDisplay("{GetType().Name,nq}  Position={Position}")]
    [DebuggerStepThrough]
    public class DataSaver<T> : StreamWrapper
        where T : IData<T>
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSaver{T}"/> writing data to the given
        /// <paramref name="stream"/>, retrieiving data from the provided <paramref name="instance"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to save data from.</param>
        /// <param name="instance">The instance to retrieve data from.</param>
        public DataSaver(Stream stream, T instance)
            : base(stream)
        {
            Instance = instance;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the deserialized instance.
        /// </summary>
        public T Instance { get; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Starts saving the instance of type <typeparamref name="T"/>.
        /// </summary>
        public void Execute()
        {
            Instance.Save(this);
        }

        /// <summary>
        /// Saves the <see cref="IData{T}"/> instance.
        /// </summary>
        /// <typeparam name="TData">The type of the <see cref="IData{T}"/> to save.</typeparam>
        /// <param name="value">The instance to save.</param>
        /// <param name="parameter">The optional parameter to pass in to the saving instance.</param>
        public void Save<TData>(TData value, object parameter = null)
            where TData : IData<T>, new()
        {
            value.Save(this, parameter);
        }

        /// <summary>
        /// Saves <paramref name="count"/> <see cref="IData{T}"/> instances.
        /// </summary>
        /// <typeparam name="TData">The type of the <see cref="IData{T}"/> to save.</typeparam>
        /// <param name="values">The instances to save.</param>
        /// <param name="parameter">The optional parameter to pass in to all saving instances.</param>
        /// <returns>The saved instances.</returns>
        public void SaveMany<TData>(IEnumerable<TData> values, object parameter = null)
            where TData : IData<T>, new()
        {
            foreach (TData value in values)
                value.Save(this, parameter);
        }
    }
}
