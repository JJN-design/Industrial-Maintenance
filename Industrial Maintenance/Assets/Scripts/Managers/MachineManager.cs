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
	[Tooltip("The object of the painter")]
	[SerializeField] private BaseMachine m_painter;

	[Header("Machines Enabled State")]
	[Tooltip("Whether or not the woodchipper is enabled")]
	[SerializeField] private bool m_woodchipperEnabled;
	[Tooltip("Whether or not the press is enabled")]
	[SerializeField] private bool m_pressEnabled;
	[Tooltip("Whether or not the painter is enabled")]
	[SerializeField] private bool m_painterEnabled;

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
		if(m_woodchipperEnabled)
			m_woodchipper.GenerateVariables(this);
		if(m_pressEnabled)
			m_press.GenerateVariables(this);
		if(m_painterEnabled)
			m_painter.GenerateVariables(this);
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
		if(m_woodchipperEnabled)
			if (!m_woodchipper.GetWorking())
				return false;
		if(m_pressEnabled)
			if (!m_press.GetWorking())
				return false;
		if(m_painterEnabled)
			if (!m_painter.GetWorking())
				return false;
		return true;
	}

	/// <summary>
	/// Breaks a random machine, has a chance to generate a 'FIXED' issue, or break an already broken machine
	/// in which case, nothing will happen
	/// </summary>
	private void BreakMachine()
	{
		int machineToBreak = Random.Range(0, 4);
		switch(machineToBreak)
		{
			case (0):
				if (m_woodchipperEnabled)
				{
					m_woodchipper.BreakMachine();
					Debug.Log("Woodchipper broke");
				}
				else
					Debug.Log("Woodchipper tried to break but is disabled");
				break;
			case (1):
				if (m_pressEnabled)
				{
					m_press.BreakMachine();
					Debug.Log("Press broke");
				}
				else
					Debug.Log("Press tried to break but is disabled");
				break;
			case (2):
				if (m_painterEnabled)
				{
					m_painter.BreakMachine();
					Debug.Log("Painter");
				}
				else
					Debug.Log("Painter tried to break but is disabled");
				break;
			case (3):
				Debug.Log("No machine was broken");
				break;
			default:
				Debug.LogError("Something went horribly wrong with Random.Range");
				break;
		}
	}
}
