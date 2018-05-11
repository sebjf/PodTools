struct Designation;
struct DesignationMacro;
struct DesignationValue;
struct DesignationPhase;

typedef struct
{
	uint value1; // bool
	uint reserved2;
	uint value3;
	uint value4; // bool;
	uint reserved5;
	uint reserved6;
	uint reserved7;
	uint reserved8;
	uint reserved9;
	uint valueA[5];
	PbdfString name;
	if (name.decData != "NEANT")
	{
		uint numMacro;
		Macro macros(3)[numMacro] <optimize = true>;
		uint numDesignationMacro;
		DesignationMacro designationMacros[numDesignationMacro] <optimize = true>;
		uint numValue;
		DesignationValue values[numValue] <optimize = true>;
	}
	uint numPhaseMacros;
	Macro phaseMacros(3)[numPhaseMacros] <optimize = true>;
	uint numPhase;
	DesignationPhase phases[numPhase] <optimize = false>;
} Designation <read = DesignationRead>;
string DesignationRead(Designation& value)
{
	return PbdfStringRead(value.name);
}

typedef struct // DesignationMacro
{
	ubyte value2[60];
	int value3; // macro index
	uint value4; // macro index
} DesignationMacro;

typedef struct // DesignationValue
{
	uint data[8];
} DesignationValue;

typedef struct // DesignationPhase
{
	PbdfString name; // Arrivee, Charge, Depart, FinCourse
	int idxMacro;
} DesignationPhase <read = DesignationPhaseRead>;
string DesignationPhaseRead(DesignationPhase& value)
{
	return PbdfStringRead(value.name);
}