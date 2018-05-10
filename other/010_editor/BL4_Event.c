struct EventList; struct Event;

typedef struct // EventList
{
	uint num;
	uint sizBuffer;
	Event events[num] <optimize = false>;
} EventList;

typedef struct // Event
{
	PbdfString name;
	uint sizParam;
	uint numParam;
	if (sizParam * numParam) ubyte paramData[sizParam * numParam];
} Event <read = EventRead>;
string EventRead(Event& value)
{
	return PbdfStringRead(value.name);
}
