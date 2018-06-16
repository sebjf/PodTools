typedef struct // Difficulty
{
	PbdfString difficultyName;
	PbdfString fileName;
	if (!PbdfStringCompare(fileName, "NEANT"))
	{
		uint numPosition; // Enr_Positions
		Vector3F16x16 positions[numPosition];
		uint numPath;
		struct Path
		{
			uint numPoint;
			struct Point
			{
				uint value4; // ushort
				uint value5; // ushort
				uint idxPosition;
				uint value7;
			} points[numPoint] <optimize = true>;
		} paths[numPath] <optimize = false>;
		PbdfString name;
		if (name.decData == "PLANS CONTRAINTES")
		{
			uint numConstraint;
			struct Constraint
			{
				uint idxDesignation;
				uint valueA;
				uint valueB; // constraint / attack
			} constraints[numConstraint] <optimize = true>;
		}
	}
	PbdfString fileName2;
	if (!PbdfStringCompare(fileName2, "NEANT"))
	{
		uint value1;
		uint numSubA;
		uint numSubB;
		uint numSubC;
		uint value5;
		struct SubA
		{
			Vector3U value6;
			uint value7;
			uint value8;
			uint numValueA;
			if (numValueA) uint valueA[numValueA];
			uint numValueB;
			if (numValueB) uint valueB[numValueB];
		} subAs[numSubA] <optimize = false>;
		struct SubB
		{
			uint valueD;
			uint valueE;
			uint numValueG;
			uint valueG[numValueG];
			uint valueH;
			uint numValueJ;
			uint valueJ[numValueJ];
		} subBs[numSubB] <optimize = false>;
		if (numSubC)
		{
			struct SubC
			{
				uint valueK;
				uint valueL;
				int valueM;
			} subCs[numSubC] <optimize = true>;
		}
	}
} Difficulty <read = DifficultyRead>;
string DifficultyRead(Difficulty& value)
{
	return PbdfStringRead(value.difficultyName);
}