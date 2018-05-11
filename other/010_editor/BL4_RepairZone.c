struct RepairZoneList;
struct RepairZone;

typedef struct // RepairZoneList
{
	PbdfString name;
	if (name.decData != "NEANT")
	{
		uint num;
		RepairZone repairZones[num] <optimize = true>;
		fixed16x16 time;
	}
} RepairZoneList <read = RepairZoneListRead>;
string RepairZoneListRead(RepairZoneList& value)
{
	return PbdfStringRead(value.name);
}

typedef struct // RepairZone
{
	Vector3F16x16 position1;
	Vector3F16x16 position2;
	Vector3F16x16 position3;
	Vector3F16x16 position4;
	Vector3F16x16 positionCenter;
	fixed16x16 height;
	fixed16x16 delay;
} RepairZone;