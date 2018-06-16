using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DifficultySection : IDifficultySectionData
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const string _plansContraintesName = "PLANS CONTRAINTES";

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string DifficultyName { get; set; }

        public string Name { get; set; }

        public IList<Vector3F> Positions { get; set; }

        public IList<IList<DifficultyPoint>> Paths { get; set; }

        public string ConstraintName { get; set; }

        public IList<DifficultyConstraint> Constraints { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Positions = loader.ReadMany(loader.ReadInt32(), () => loader.ReadVector3F16x16());

            int pathCount = loader.ReadInt32();
            Paths = new List<IList<DifficultyPoint>>(pathCount);
            while (pathCount-- > 0)
                Paths.Add(loader.LoadMany<DifficultyPoint>(loader.ReadInt32()).ToList());

            ConstraintName = loader.ReadPodString();
            if (String.Compare(ConstraintName, _plansContraintesName, true, CultureInfo.InvariantCulture) == 0)
                loader.LoadMany<DifficultyConstraint>(loader.ReadInt32()).ToList();
        }
    }
}