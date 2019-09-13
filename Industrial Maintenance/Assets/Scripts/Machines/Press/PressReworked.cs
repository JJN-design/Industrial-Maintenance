using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Press Variables

public enum SpokeState
{
	STRAIGHT,
	CURVED
}

public enum SpokeNumber
{
	FOUR_SPOKES,
	EIGHT_SPOKES,
	SIXTEEN_SPOKES,
}

public enum PressLeakLiquid
{
	INK,
	WATER
}

public enum PressLeakVelocity
{
	HIGH,
	LOW
}

#endregion //Press Variables

public class PressReworked : BaseMachine
{

	#region Wheel Variables

	//The state of the wheel spokes
	private SpokeState m_spokeState;

	//How many spokes there are
	private SpokeNumber m_spokeNumber;

	[Header("Wheel GameObjects")]
	[Tooltip("The object for the 4 curved spoke wheel")]
	[SerializeField] private GameObject m_4CurvedSpokeWheel;
	[Tooltip("The object for the 8 curved spoke wheel")]
	[SerializeField] private GameObject m_8CurvedSpokeWheel;
	[Tooltip("The object for the 16 curved spoke wheel")]
	[SerializeField] private GameObject m_16CurvedSpokeWheel;
	[Tooltip("The object for the 4 straight spoke wheel")]
	[SerializeField] private GameObject m_4StraightSpokeWheel;
	[Tooltip("The object for the 8 straight spoke wheel")]
	[SerializeField] private GameObject m_8StraightSpokeWheel;
	[Tooltip("The object for the 16 straight spoke wheel")]
	[SerializeField] private GameObject m_16StraightSpokeWheel;

	/// <summary>
	/// Gets the state of spokes on the wheel
	/// </summary>
	/// <returns>The state of the spokes</returns>
	public SpokeState GetSpokeState() { return m_spokeState; }

	/// <summary>
	/// Gets the number of spokes on the wheel
	/// </summary>
	/// <returns>The amount of spokes</returns>
	public SpokeNumber GetSpokeNumber() { return m_spokeNumber; }

	/// <summary>
	/// 
	/// </summary>
	private void GenerateWheel()
	{

	}

	#endregion //Wheel Variables

	/// <summary>
	/// Generates the variables for the machine
	/// </summary>
	/// <param name="manager">The machine manager</param>
	public override void GenerateVariables(MachineManager manager)
	{
		if (m_isGenerated) //if already generated, don't generate again
			return;

		//set manager
		m_machineManager = manager;

		//set generated state
		m_isGenerated = true;
	}

	/// <summary>
	/// Breaks the machine
	/// </summary>
	public override void BreakMachine()
	{
		base.BreakMachine();
	}

	/// <summary>
	/// Fixes the machine
	/// </summary>
	public override void FixMachine()
	{
		base.FixMachine();
	}
}
