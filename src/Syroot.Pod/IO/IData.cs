namespace Syroot.Pod.IO
{

    /// <summary>
    /// Represents data loadable by a <see cref="DataLoader{T}"/> and saveable by a <see cref="DataSaver{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the instance passed by the data loader and saver.</typeparam>
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

        /// <summary>
        /// Saves data from the instance into the <paramref name="saver"/>.
        /// </summary>
        /// <param name="saver">The <see cref="DataSaver{T}"/> to save data with.</param>
        /// <param name="parameter">The optional parameter passed in to the instance.</param>
        void Save(DataSaver<T> saver, object parameter = null);
    }
}
