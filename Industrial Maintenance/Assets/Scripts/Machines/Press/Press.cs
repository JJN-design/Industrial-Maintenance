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
	//Whether or not the machine's variables have been generated yet
	private bool m_isGenerated = false;

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
	
	/// <summary>
	/// Generates the variables for press
	/// </summary>
	override public void GenerateVariables(MachineManager manager)
	{
		if (m_isGenerated) //if the machine's already had variables generated, don't do it again
			return;

		m_machineManager = manager;

		//TODO interactable generation

		//Randomly generates variables for the machine
		CreateNozzle();
		CreateRPM();
		CreateModelNumber();

		//confirm that this machine has been generated
		m_isGenerated = true;
	}
}
