struct VisibleList; struct Visible;

typedef struct // VisibleList
{
	uint numVisibles;
	Visible visible[numVisibles] <optimize = false>;
} VisibleList;

typedef struct // Visible
{
	int numSectors; // -1 = no sectors are visible
	if (numSectors > 0) uint sectors[numSectors];
} Visible;
