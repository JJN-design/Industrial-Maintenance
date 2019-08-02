using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Generator Variables
//TODO: Variables here

#endregion
public class Generator : BaseMachine
{
	private bool m_isGenerated = false;

	override public void GenerateVariables(MachineManager manager)
	{
		if (m_isGenerated) //if this machine's already been generated before, don't do it again
			return;

		m_machineManager = manager;

		//TODO variables

		//confirm that this machine has been generated
		m_isGenerated = true;
	}
}
