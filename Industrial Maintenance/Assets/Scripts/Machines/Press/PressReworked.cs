using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressReworked : BaseMachine
{

	/// <summary>
	/// Generates the variables for the machine
	/// </summary>
	/// <param name="manager">The machine manager</param>
	public override void GenerateVariables(MachineManager manager)
	{
		if (m_isGenerated)
			return;
		m_machineManager = manager;

		m_isGenerated = true;
	}

	public override void BreakMachine()
	{
		base.BreakMachine();
	}

	public override void FixMachine()
	{
		base.FixMachine();
	}
}
