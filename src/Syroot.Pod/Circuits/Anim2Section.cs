using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim2Section : ISectionData
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const string _animeSecteurName = "ANIME SECTEUR";

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public IList<Anim2Texture> Textures { get; set; }

        public uint Unknown1 { get; set; }

        public IList<Anim2Object> Objects { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            if (String.Compare(Name, _animeSecteurName, true, CultureInfo.InvariantCulture) == 0)
                throw new NotImplementedException();

            Textures = loader.LoadMany<Anim2Texture>(loader.ReadInt32()).ToList();
            Unknown1 = loader.ReadUInt32();
            Objects = loader.LoadMany<Anim2Object>(loader.ReadInt32()).ToList();
        }
    }
}