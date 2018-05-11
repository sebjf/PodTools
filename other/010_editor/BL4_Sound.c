struct SoundList; struct Sound;

typedef struct // SoundList
{
	PbdfString fileName;
	if (!PbdfStringCompare(fileName, "NEANT"))
	{
		uint num;
		Sound sounds[num] <optimize = true>;
	}
} SoundList <read = SoundListRead>;
string SoundListRead(SoundList& value)
{
	return PbdfStringRead(value.fileName);
}

typedef struct // Sound
{
	uint unknown[14];
} Sound;
