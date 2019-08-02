using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Press Variables
//The type of the nozzle on the machine
enum NozzleType
{
	NARROW,
	WIDE
}

//The possible RPM ratings
enum RPMRating
{
	RPM700, 
	RPM856,
	RPM900,
	RPM902,
	RPM1000
}
#endregion
public class Press : BaseMachine
{
	private bool m_isGenerated = false;

	//Variables
	private NozzleType m_nozzleType;
	private RPMRating m_RPMRating;
	private int m_modelNumber;
	
	/// <summary>
	/// Generates the variables for press
	/// </summary>
	override public void GenerateVariables(MachineManager manager)
	{
		if (m_isGenerated) //if the machine's already had variables generated, don't do it again
			return;

		m_machineManager = manager;

		m_nozzleType = (NozzleType)Random.Range(0, 2);
		m_RPMRating = (RPMRating)Random.Range(0, 5);
		m_modelNumber = Random.Range(0, 100000);

		//confirm that this machine has been generated
		m_isGenerated = true;
	}
}
