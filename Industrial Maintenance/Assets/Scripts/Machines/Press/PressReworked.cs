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
	/// Generates variables for the wheel, and makes the correct wheel visible
	/// </summary>
	private void CreateWheel()
	{
		//disables all wheels
		m_4CurvedSpokeWheel.SetActive(false);
		m_4StraightSpokeWheel.SetActive(false);
		m_8CurvedSpokeWheel.SetActive(false);
		m_8StraightSpokeWheel.SetActive(false);
		m_16CurvedSpokeWheel.SetActive(false);
		m_16StraightSpokeWheel.SetActive(false);

		//randomly sets a spoke state and number
		m_spokeState = (SpokeState)Random.Range(0, 2);
		m_spokeNumber = (SpokeNumber)Random.Range(0, 3);

		//enables the proper wheel
		switch(m_spokeState)
		{
			case (SpokeState.CURVED): //if the spokes are curved...
				switch (m_spokeNumber)
				{
					case (SpokeNumber.FOUR_SPOKES): //...and there are four spokes
						m_4CurvedSpokeWheel.SetActive(true);
						break;
					case (SpokeNumber.EIGHT_SPOKES): //...and there are eight spokes
						m_8CurvedSpokeWheel.SetActive(true);
						break;
					case (SpokeNumber.SIXTEEN_SPOKES): //...and there are sixteen spokes
						m_16CurvedSpokeWheel.SetActive(true);
						break;
					default: //...and Random.Range fucked up
						Debug.LogError("Random.Range returned invalid SpokeNumber!");
						break;
				}
				break;
			case (SpokeState.STRAIGHT): //if the spokes are straight...
				switch(m_spokeNumber)
				{
					case (SpokeNumber.FOUR_SPOKES): //...and there are four spokes
						m_4StraightSpokeWheel.SetActive(true);
						break;
					case (SpokeNumber.EIGHT_SPOKES): //...and there are eight spokes
						m_8StraightSpokeWheel.SetActive(true);
						break;
					case (SpokeNumber.SIXTEEN_SPOKES): //...and there are sixteen spokes
						m_16StraightSpokeWheel.SetActive(true);
						break;
					default: //...and Random.Range fucked up
						Debug.LogError("Random.Range returned invalid SpokeNumber!");
						break;
				}
				break;
			default: //if Random.Range fucked up...
				Debug.LogError("Random.Range returned invalid SpokeState!");
				break;
		}
	}

	#endregion //Wheel Variables

	#region Leak Variables

	//the pipes that can leak
	[Header("Leaking pipes")]
	[Tooltip("The first single pipe")]
	[SerializeField] private LeakingPipe m_singlePipe1;
	[Tooltip("The second single pipe")]
	[SerializeField] private LeakingPipe m_singlePipe2;
	[Tooltip("The first y-split pipe")]
	[SerializeField] private LeakingPipe m_ySplitPipe1;
	[Tooltip("The second y-split pipe")]
	[SerializeField] private LeakingPipe m_ySplitPipe2;

	//the pipes that are currently leaking
	private bool m_singlePipe1Leaking = false;
	private bool m_singlePipe2Leaking = false;
	private bool m_ySplitPipe1Leaking = false;
	private bool m_ySplitPipe2Leaking = false;

	//getters for pipe leaking states
	public bool GetSingle1Leaking() { return m_singlePipe1Leaking; }
	public bool GetSingle2Leaking() { return m_singlePipe2Leaking; }
	public bool GetYSplit1Leaking() { return m_ySplitPipe1Leaking; }
	public bool GetYSplit2Leaking() { return m_ySplitPipe2Leaking; }

	[Header("Leak velocities")]
	[Tooltip("The velocity of the low velocity pipe leak")]
	[SerializeField] private float m_lowVelocityLeak;
	[Tooltip("The velocity of the high velocity pipe leak")]
	[SerializeField] private float m_highVelocityLeak;

	[Header("Leak emissions")]
	[Tooltip("The emission rate of the low velocity pipe leak")]
	[SerializeField] private float m_lowVelocityEmission;
	[Tooltip("The emission rate of the high velocity pipe leak")]
	[SerializeField] private float m_highVelocityEmission;

	//the leaks
	private PressLeakLiquid m_leakingLiquid;
	private PressLeakVelocity m_leakingVelocity;

	public PressLeakLiquid GetLeakLiquid() { return m_leakingLiquid; }
	public PressLeakVelocity GetLeakVelocity() { return m_leakingVelocity; }

	/// <summary>
	/// Creates the pipe variables
	/// </summary>
	private void CreatePipes()
	{
		//randomly sets liquid leak and velocity
		m_leakingLiquid = (PressLeakLiquid)Random.Range(0, 2);
		m_leakingVelocity = (PressLeakVelocity)Random.Range(0, 2);

		//Set leaky pipes
		SetLeakyPipes();

		//creates the pipes
		m_singlePipe1.Create(this, m_lowVelocityLeak, m_highVelocityLeak, m_leakingLiquid, m_leakingVelocity, m_lowVelocityEmission, m_highVelocityEmission);
		m_singlePipe2.Create(this, m_lowVelocityLeak, m_highVelocityLeak, m_leakingLiquid, m_leakingVelocity, m_lowVelocityEmission, m_highVelocityEmission);
		m_ySplitPipe1.Create(this, m_lowVelocityLeak, m_highVelocityLeak, m_leakingLiquid, m_leakingVelocity, m_lowVelocityEmission, m_highVelocityEmission);
		m_ySplitPipe2.Create(this, m_lowVelocityLeak, m_highVelocityLeak, m_leakingLiquid, m_leakingVelocity, m_lowVelocityEmission, m_highVelocityEmission);
	}

	/// <summary>
	/// Randomly sets up to four of the pipes to be leaking
	/// </summary>
	private void SetLeakyPipes()
	{
		m_singlePipe1Leaking = (Random.Range(0, 2) == 0);
		m_singlePipe2Leaking = (Random.Range(0, 2) == 0);
		m_ySplitPipe1Leaking = (Random.Range(0, 2) == 0);
		m_ySplitPipe2Leaking = (Random.Range(0, 2) == 0);

		if (!m_singlePipe1Leaking && !m_singlePipe2Leaking && !m_ySplitPipe1Leaking && !m_ySplitPipe2Leaking)
			SetLeakyPipes();
	}

	/// <summary>
	/// Updates the pipes with new variables
	/// </summary>
	private void UpdatePipes()
	{
		m_singlePipe1.UpdateLeak(m_leakingLiquid, m_leakingVelocity);
		m_singlePipe2.UpdateLeak(m_leakingLiquid, m_leakingVelocity);
		m_ySplitPipe1.UpdateLeak(m_leakingLiquid, m_leakingVelocity);
		m_ySplitPipe2.UpdateLeak(m_leakingLiquid, m_leakingVelocity);
	}

	/// <summary>
	/// Start leaking all active pipes
	/// </summary>
	public void StartLeaking()
	{
		if (m_singlePipe1Leaking)
			m_singlePipe1.StartLeaking();
		if (m_singlePipe2Leaking)
			m_singlePipe2.StartLeaking();
		if (m_ySplitPipe1Leaking)
			m_ySplitPipe1.StartLeaking();
		if (m_ySplitPipe2Leaking)
			m_ySplitPipe2.StartLeaking();
	}

	/// <summary>
	/// Stops the pipe leaks
	/// </summary>
	public void StopLeaking()
	{
		m_singlePipe1.StopLeaking();
		m_singlePipe2.StopLeaking();
		m_ySplitPipe1.StopLeaking();
		m_ySplitPipe2.StopLeaking();
	}

	/// <summary>
	/// Randomly sets new leak variables
	/// </summary>
	private void SetNewLeaks()
	{
		m_leakingLiquid = (PressLeakLiquid)Random.Range(0, 2);
		m_leakingVelocity = (PressLeakVelocity)Random.Range(0, 2);
	}

	#endregion //Leak Variables

	#region Interactables

	[Header("Interactables")]
	[Tooltip("The blue lever")]
	[SerializeField] private PressInteractableReworked m_blueLever;
	[Tooltip("The red lever")]
	[SerializeField] private PressInteractableReworked m_redLever;
	[Tooltip("The green lever")]
	[SerializeField] private PressInteractableReworked m_greenLever;
	[Tooltip("The yellow lever")]
	[SerializeField] private PressInteractableReworked m_yellowLever;
	[Tooltip("The reset button")]
	[SerializeField] private PressInteractableReworked m_resetButton;
	[Tooltip("The first bolt")]
	[SerializeField] private PressInteractableReworked m_bolt1;
	[Tooltip("The second bolt")]
	[SerializeField] private PressInteractableReworked m_bolt2;
	[Tooltip("The third bolt")]
	[SerializeField] private PressInteractableReworked m_bolt3;
	[Tooltip("The fourth bolt")]
	[SerializeField] private PressInteractableReworked m_bolt4;
	[Tooltip("The fifth bolt")]
	[SerializeField] private PressInteractableReworked m_bolt5;
	[Tooltip("The sixth bolt")]
	[SerializeField] private PressInteractableReworked m_bolt6;
	[Tooltip("How long before a press is considered a long press")]
	[SerializeField] private float m_pressThreshold;

	/// <summary>
	/// Creates all the interactables
	/// </summary>
	private void CreateInteractables()
	{
		m_blueLever.Create(this, PressInteractableType.BLUE_LEVER, m_incorrectTimeSubtraction, m_pressThreshold);
		m_redLever.Create(this, PressInteractableType.RED_LEVER, m_incorrectTimeSubtraction, m_pressThreshold);
		m_greenLever.Create(this, PressInteractableType.GREEN_LEVER, m_incorrectTimeSubtraction, m_pressThreshold);
		m_yellowLever.Create(this, PressInteractableType.YELLOW_LEVER, m_incorrectTimeSubtraction, m_pressThreshold);
		m_resetButton.Create(this, PressInteractableType.RESET_BUTTON, m_incorrectTimeSubtraction, m_pressThreshold);
		m_bolt1.Create(this, PressInteractableType.BOLT_1, m_incorrectTimeSubtraction, m_pressThreshold);
		m_bolt2.Create(this, PressInteractableType.BOLT_2, m_incorrectTimeSubtraction, m_pressThreshold);
		m_bolt3.Create(this, PressInteractableType.BOLT_3, m_incorrectTimeSubtraction, m_pressThreshold);
		m_bolt4.Create(this, PressInteractableType.BOLT_4, m_incorrectTimeSubtraction, m_pressThreshold);
		m_bolt5.Create(this, PressInteractableType.BOLT_5, m_incorrectTimeSubtraction, m_pressThreshold);
		m_bolt6.Create(this, PressInteractableType.BOLT_6, m_incorrectTimeSubtraction, m_pressThreshold);
	}

	/// <summary>
	/// Sets a new stage for all interactables
	/// </summary>
	/// <param name="stage"></param>
	public void SetNewStage(int stage)
	{
		m_blueLever.SetNewStage(stage);
		m_redLever.SetNewStage(stage);
		m_greenLever.SetNewStage(stage);
		m_yellowLever.SetNewStage(stage);
		m_resetButton.SetNewStage(stage);
		m_bolt1.SetNewStage(stage);
		m_bolt2.SetNewStage(stage);
		m_bolt3.SetNewStage(stage);
		m_bolt4.SetNewStage(stage);
		m_bolt5.SetNewStage(stage);
		m_bolt6.SetNewStage(stage);
	}

	#endregion //Interactables

	[Header("Misc. Variables")]
	[Tooltip("How much time is lost when you press the wrong button")]
	[SerializeField] private float m_incorrectTimeSubtraction;

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

		//Generates the variables
		CreatePipes();
		CreateWheel();

		//Generates the interactables
		CreateInteractables();

		//set generated state
		m_isGenerated = true;
	}

	/// <summary>
	/// Breaks the machine
	/// </summary>
	public override void BreakMachine()
	{
		//call base
		base.BreakMachine();
	}

	/// <summary>
	/// Fixes the machine
	/// </summary>
	public override void FixMachine()
	{
		//call base
		base.FixMachine();

		//reset variables
		SetNewLeaks();
		StopLeaking();
		SetLeakyPipes();
		SetNewStage(0);
	}
}
