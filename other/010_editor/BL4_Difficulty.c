typedef struct // Difficulty
{
	uint numStartPoint;
	struct StartPoint
	{
		ubyte data[36];
	} startPoints[numStartPoint];
} Difficulty;

typedef struct // DifficultyLevel
{
	PbdfString difficultyName;
	PbdfString levName;
	if (levName.decData != "NEANT")
	{
		uint numPosition; // Enr_Positions
		Vector3F16x16 positions[numPosition];
		uint numSection;
		struct Section
		{
			uint numTrack;
			struct Track
			{
				uint value4; // ushort
				uint value5; // ushort
				uint idxPosition;
				uint value7;
			} tracks[numTrack] <optimize = true>;
		} sections[numSection] <optimize = false>;
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
	PbdfString levName2;
	if (levName2.decData != "NEANT")
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
			uint valueA[numValueA];
			uint numValueB;
			uint valueB[numValueB];
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
		struct SubC
		{
			uint valueK;
			uint valueL;
			int valueM;
		} subCs[numSubC] <optimize = true>;
	}
} DifficultyLevel <read = DifficultyLevelRead>;
string DifficultyLevelRead(DifficultyLevel& value)
{
	return PbdfStringRead(value.difficultyName);
}