using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Press Variables
//The type of the nozzle on the machine
enum NozzleType
{
	NARROW,
	WIDE
}

//The possible RPM ratings
public enum RPMRating
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
	//Whether or not the machine's variables have been generated yet
	private bool m_isGenerated = false;

	[Header("Interactables")]
	[Tooltip("The second bolt from the left")]
	[SerializeField] private PressInteractable m_secondBolt;
	[Tooltip("The third bolt from the left")]
	[SerializeField] private PressInteractable m_thirdBolt;
	[Tooltip("The interactable to tighten the ink valve")]
	[SerializeField] private PressInteractable m_tightenInk;
	[Tooltip("The interactable to loosen the ink valve")]
	[SerializeField] private PressInteractable m_loosenInk;
	[Tooltip("The yellow coloured lever")]
	[SerializeField] private PressInteractable m_yellowLever;
	[Tooltip("The orange coloured lever")]
	[SerializeField] private PressInteractable m_orangeLever;
	[Tooltip("The button to restart the machine")]
	[SerializeField] private PressInteractable m_restartButton;

	private NozzleType m_nozzleType;
	[Header("Nozzle Types")]
	[Tooltip("The narrow nozzle")]
	[SerializeField] private GameObject m_narrowNozzle;
	[Tooltip("The wide nozzle")]
	[SerializeField] private GameObject m_wideNozzle;

	/// <summary>
	/// Randomly sets a nozzle and makes the correct nozzle visible
	/// </summary>
	private void CreateNozzle()
	{
		m_nozzleType = (NozzleType)Random.Range(0, 2);
		switch (m_nozzleType)
		{
			case (NozzleType.NARROW):
				m_narrowNozzle.SetActive(true);
				break;
			case (NozzleType.WIDE):
				m_wideNozzle.SetActive(true);
				break;
			default:
				Debug.LogError("Nozzle type randomisation went wrong on press");
				break;
		}
	}

	private RPMRating m_RPMRating;
	[Header("RPM Ratings")]
	[Tooltip("The display for an RPM rating of 700")]
	[SerializeField] private GameObject m_RPM700;
	[Tooltip("The display for an RPM rating of 856")]
	[SerializeField] private GameObject m_RPM856;
	[Tooltip("The display for an RPM rating of 900")]
	[SerializeField] private GameObject m_RPM900;
	[Tooltip("The display for an RPM rating of 902")]
	[SerializeField] private GameObject m_RPM902;
	[Tooltip("The display for an RPM rating of 1000")]
	[SerializeField] private GameObject m_RPM1000;

	/// <summary>
	/// Randomly sets an RPM rating and makes the correct display visible
	/// </summary>
	private void CreateRPM()
	{
		m_RPMRating = (RPMRating)Random.Range(0, 5);
		switch(m_RPMRating)
		{
			case (RPMRating.RPM700):
				m_RPM700.SetActive(true);
				break;
			case (RPMRating.RPM856):
				m_RPM856.SetActive(true);
				break;
			case (RPMRating.RPM900):
				m_RPM900.SetActive(true);
				break;
			case (RPMRating.RPM902):
				m_RPM902.SetActive(true);
				break;
			case (RPMRating.RPM1000):
				m_RPM1000.SetActive(true);
				break;
		}
	}

	/// <summary>
	/// Gets the RPM rating
	/// </summary>
	/// <returns>The RPM rating of the machine</returns>
	public RPMRating GetRPM() { return m_RPMRating; }

	private int m_modelNumber;
	//whether the model number is even or not
	private bool m_modelIsEven;
	[Tooltip("The display for model number")]
	[SerializeField] private Text m_modelNumText;

	/// <summary>
	/// Randomly sets a model number and checks if it's even
	/// </summary>
	private void CreateModelNumber()
	{
		m_modelNumber = Random.Range(0, 100000);

		m_modelNumText.text = m_modelNumber.ToString();

		if (m_modelNumber % 2 == 0)
			m_modelIsEven = true;
		else
			m_modelIsEven = false;
	}

	[Header("Other variables")]
	[Tooltip("How much time to be subtracted on an incorrect button press in seconds")]
	[SerializeField] private float m_incorrectTime;
	[Tooltip("How long the restart button needs to be held to restart on a non-multiple of 100 RPM rating")]
	[SerializeField] private float m_buttonHoldTime;
	[Tooltip("How long the restart button can be held for before failure on a multiple of 100 RPM rating")]
	[SerializeField] private float m_maxButtonHoldTime;

	/// <summary>
	/// Breaks the machine
	/// </summary>
	/// <param name="issue">The issue that was generated for the machine</param>
	public override void BreakMachine(MachineIssue issue)
	{
		//call base machine break code
		base.BreakMachine(issue);

		switch(issue)
		{
			case (MachineIssue.DUST_PLUMES):
				if (m_modelIsEven)
					m_secondBolt.SetCorrect(true);
				else
					m_thirdBolt.SetCorrect(true);
				break;
			case (MachineIssue.ON_FIRE):
				if (m_nozzleType == NozzleType.NARROW)
					m_tightenInk.SetCorrect(true);
				else if (m_nozzleType == NozzleType.WIDE)
					m_loosenInk.SetCorrect(true);
				else
					Debug.LogError("Invalid nozzle type on press!");
				break;
			case (MachineIssue.SPARKING):
				if (m_RPMRating == RPMRating.RPM700 || m_RPMRating == RPMRating.RPM856)
					m_yellowLever.SetCorrect(true);
				else if (m_RPMRating == RPMRating.RPM900 || m_RPMRating == RPMRating.RPM902 || m_RPMRating == RPMRating.RPM1000)
					m_orangeLever.SetCorrect(true);
				else
					Debug.LogError("Invalid RPM rating on press!");
				break;
			case (MachineIssue.FIXED):
				//no issue, no action needed
				break;
			default:
				Debug.LogError("Invalid issue generated for press!");
				break;
		}
	}

	/// <summary>
	/// Generates the variables for press
	/// </summary>
	override public void GenerateVariables(MachineManager manager)
	{
		if (m_isGenerated) //if the machine's already had variables generated, don't do it again
			return;

		//set manager
		m_machineManager = manager;

		//Create interactables
		m_secondBolt.Create(this, PressInteractableType.SECOND_BOLT, m_incorrectTime);
		m_thirdBolt.Create(this, PressInteractableType.THIRD_BOLT, m_incorrectTime);
		m_tightenInk.Create(this, PressInteractableType.TIGHTEN_INK, m_incorrectTime);
		m_loosenInk.Create(this, PressInteractableType.LOOSEN_INK, m_incorrectTime);
		m_yellowLever.Create(this, PressInteractableType.YELLOW_LEVER, m_incorrectTime);
		m_orangeLever.Create(this, PressInteractableType.ORANGE_LEVER, m_incorrectTime);
		m_restartButton.Create(this, PressInteractableType.RESTART_BUTTON, m_incorrectTime, m_buttonHoldTime, m_maxButtonHoldTime);

		//Randomly generates variables for the machine
		CreateNozzle();
		CreateRPM();
		CreateModelNumber();

		//confirm that this machine has been generated
		m_isGenerated = true;
	}

	/// <summary>
	/// Sets a new correct interactable
	/// </summary>
	/// <param name="interactableType">The new interactable to be set as correct</param>
	/// <param name="secondStage">Whether or not this is on the second stage</param>
	public void SetNewInteractable(PressInteractableType interactableType)
	{
		switch(interactableType)
		{
			case (PressInteractableType.SECOND_BOLT):
				m_secondBolt.SetCorrect(true);
				break;
			case (PressInteractableType.THIRD_BOLT):
				m_thirdBolt.SetCorrect(true);
				break;
			case (PressInteractableType.LOOSEN_INK):
				m_loosenInk.SetCorrect(true);
				break;
			case (PressInteractableType.TIGHTEN_INK):
				m_tightenInk.SetCorrect(true);
				break;
			case (PressInteractableType.ORANGE_LEVER):
				m_orangeLever.SetCorrect(true);
				break;
			case (PressInteractableType.YELLOW_LEVER):
				m_yellowLever.SetCorrect(true);
				break;
			case (PressInteractableType.RESTART_BUTTON):
				m_restartButton.SetCorrect(true);
				break;
			default:
				Debug.LogError("Invalid interactable type on Press!");
				break;
		}
	}
}
