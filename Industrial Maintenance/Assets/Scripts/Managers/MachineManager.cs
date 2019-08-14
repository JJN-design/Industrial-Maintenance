using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
	[Header("Machines")]
	[Tooltip("The object of the woodchipper")]
	[SerializeField] private BaseMachine m_woodchipper;
	[Tooltip("The object of the press")]
	[SerializeField] private BaseMachine m_press;
	[Tooltip("The object of the generator")]
	[SerializeField] private BaseMachine m_generator;

	//whether or not the factory is currently producing boxes
	private bool m_producingBoxes = true;

	[Header("Timers")]
	[Tooltip("How long between machine breakings")]
	[SerializeField] private float m_timeBetweenBreaks;
	//The current timer of the machine breaking
	private float m_breakTimer = 0.0f;
	
	[Tooltip("How long it takes to produce a box")]
	[SerializeField] private float m_timeBetweenBoxes;
	//the current timer of box production
	private float m_boxTimer = 0.0f;

	[Header("Other Objects")]
	[Tooltip("The FPS controller of the player")]
	[SerializeField] private FPSController m_playerController;
	public FPSController GetController() { return m_playerController; }

	private void Awake()
	{
		m_woodchipper.GenerateVariables(this);
		/*m_press.GenerateVariables(this);
		m_generator.GenerateVariables(this);*/
	}

	// Update is called once per frame
	void Update()
    {
		//Check if the factory can produce boxes
		m_producingBoxes = CheckMachines();
		if(m_producingBoxes)
		{
			//Increment timer
			m_boxTimer += Time.deltaTime;

			//If timer reaches threshold, produce a box
			if(m_boxTimer >= m_timeBetweenBoxes)
			{
				//Reset timer and add score
				m_boxTimer -= m_timeBetweenBoxes;
				Debug.Log("A box was produced!");
				ScoreManager.AddScore(1);
			}
		}

		m_breakTimer += Time.deltaTime;
		if(m_breakTimer >= m_timeBetweenBreaks)
		{
			m_breakTimer -= m_timeBetweenBreaks;
			BreakMachine();
		}

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BreakMachine();
        }
    }

	/// <summary>
	/// Checks if all machines are functional
	/// </summary>
	/// <returns>Whether or not all machines are working</returns>
	private bool CheckMachines()
	{
		if (!m_woodchipper.GetWorking())
			return false;
		/*if (!m_press.GetWorking())
			return false;
		if (!m_generator.GetWorking())
			return false;*/
		return true;
	}

	/// <summary>
	/// Generates a random issue for a machine
	/// </summary>
	/// <returns>A random MachineIssue</returns>
	private MachineIssue GenerateIssue()
	{
		return (MachineIssue)Random.Range(0, 4);
	}

	/// <summary>
	/// Breaks a random machine, has a chance to generate a 'FIXED' issue, or break an already broken machine
	/// in which case, nothing will happen
	/// </summary>
	private void BreakMachine()
	{
		int machineToBreak = Random.Range(0, /*4*/ 1);
		MachineIssue issue = GenerateIssue();
		switch(machineToBreak)
		{
			case (0):
				m_woodchipper.BreakMachine(issue);
				Debug.Log("Woodchipper broke with issue " + issue.ToString());
				break;
			/*case (1):
				m_press.BreakMachine(issue);
				Debug.Log("Press broke with issue " + issue.ToString());
				break;
			case (2):
				m_generator.BreakMachine(issue);
				Debug.Log("Generator broke with issue " + issue.ToString());
				break;
			case (3):
				Debug.Log("No machine was broken");
				break;*/
			default:
				Debug.LogError("Something went horribly wrong with Random.Range");
				break;
		}
	}
}
