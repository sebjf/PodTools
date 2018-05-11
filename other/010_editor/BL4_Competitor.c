struct CompetitorList;
struct Competitor;

typedef struct // CompetitorList
{
	PbdfString difficultyName;
	PbdfString fileName;
	if (!PbdfStringCompare(fileName, "NEANT"))
	{
		uint num;
		Competitor competitors[num] <optimize = false>;
	}
} CompetitorList <read = CompetitorListRead>;
string CompetitorListRead(CompetitorList& value)
{
	return PbdfStringRead(value.difficultyName);
}

typedef struct // Competitor
{
	PbdfString name;
	uint value1;
	uint value2;
	PbdfString number;
	uint numUnk;
	if (numUnk) Vector3U unk[numUnk];
} Competitor <read = CompetitorRead>;
string CompetitorRead(Competitor& value)
{
	return PbdfStringRead(value.name);
}