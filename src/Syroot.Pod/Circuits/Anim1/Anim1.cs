using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a collection of related <see cref="Anim1Object"/> and <see cref="Anim1Texture"/> animations.
    /// </summary>
    public class Anim1 : IData<Circuit>
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const string _wrongWayName = "wrongway.ani";

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public uint WrongWayValue1 { get; set; }

        public uint WrongWayValue2 { get; set; }

        public uint Unknown { get; set; }

        /// <summary>
        /// Gets or sets the list of animated <see cref="Anim1Object"/> instances.
        /// </summary>
        public IList<Anim1Object> Objects { get; set; }

        /// <summary>
        /// Gets or sets the list of animated <see cref="Anim1Texture"/> instances.
        /// </summary>
        public IList<Anim1Texture> Textures { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadPodString();
            if (String.Compare(Name, _wrongWayName, true, CultureInfo.InvariantCulture) == 0)
            {
                WrongWayValue1 = loader.ReadUInt32();
                WrongWayValue2 = loader.ReadUInt32();
            }
            int texturesCount = loader.ReadInt32();
            int objectCount = loader.ReadInt32();
            Unknown = loader.ReadUInt32();
            Objects = loader.LoadMany<Anim1Object>(objectCount).ToList();
            Textures = loader.LoadMany<Anim1Texture>(texturesCount).ToList();
        }
    }
}