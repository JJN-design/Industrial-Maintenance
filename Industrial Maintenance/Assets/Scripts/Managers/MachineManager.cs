using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MachineLists
{
	WOODCHIPPER,
	PRESS,
	PAINTER,
	NONE
}

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

	[Header("Audio")]
	[Tooltip("The source of the alarm audio")]
	[SerializeField] AudioSource m_alarmSource;
	[Tooltip("The clip that plays when the woodchipper breaks")]
	[SerializeField] AudioClip m_woodchipperAlarm;
	[Tooltip("The clip that plays when the press breaks")]
	[SerializeField] AudioClip m_pressAlarm;
	[Tooltip("The clip that plays when the painter breaks")]
	[SerializeField] AudioClip m_painterAlarm;

	//The last machine that broke
	private MachineLists m_lastBreak = MachineLists.NONE;

	[Header("Timers")]
	[Tooltip("How long between machine breakings")]
	[SerializeField] private float m_timeBetweenBreaks;
	[Tooltip("The initial timer count")]
	[SerializeField] private float m_initialTimer;
	//The current timer of the machine breaking
	private float m_breakTimer;

	//How long the player has survived for
	private float m_survivalTime = 0.0f;
	
	[Tooltip("How long it takes to produce a box")]
	[SerializeField] private float m_timeBetweenBoxes;
	//the current timer of box production
	private float m_boxTimer = 0.0f;

	[Header("Other Objects")]
	[Tooltip("The FPS controller of the player")]
	[SerializeField] private FPSController m_playerController;
	public FPSController GetController() { return m_playerController; }

	[Tooltip("The assembly line manager")]
	[SerializeField] private AssemblyLine m_assemblyLine;

	/// <summary>
	/// Called on awaken
	/// </summary>
	private void Awake()
	{
		m_breakTimer = m_initialTimer;
		if(m_woodchipperEnabled)
			m_woodchipper.GenerateVariables(this);
		if(m_pressEnabled)
			m_press.GenerateVariables(this);
		if(m_painterEnabled)
			m_painter.GenerateVariables(this);
	}

	/// <summary>
	/// Called each frame
	/// </summary>
	void Update()
    {
		//Increment timer
		m_boxTimer += Time.deltaTime;
		m_survivalTime += Time.deltaTime;

		ScoreManager.SetTime(m_survivalTime);

		//If timer reaches threshold, produce a log
		if(m_boxTimer >= m_timeBetweenBoxes)
		{
			//Reset timer and add score
			m_boxTimer -= m_timeBetweenBoxes;
			Debug.Log("A log was produced!");
			m_assemblyLine.CreateBox();
		}

		m_breakTimer += Time.deltaTime;
		if(m_breakTimer >= m_timeBetweenBreaks)
		{
			m_breakTimer -= m_timeBetweenBreaks;
			BreakMachine();
		}
#if UNITY_EDITOR
		//Debug key to break woodchipper
		if(Input.GetKeyDown(KeyCode.Alpha1))
			m_woodchipper.BreakMachine();

		if (Input.GetKeyDown(KeyCode.Alpha2))
			m_painter.BreakMachine();

		if (Input.GetKeyDown(KeyCode.Alpha3))
			m_press.BreakMachine();

		if (Input.GetKeyDown(KeyCode.Alpha4))
			BreakMachine();
#endif //UNITY_EDITOR
    }

	/// <summary>
	/// Gets the working state of a particular machine
	/// </summary>
	/// <param name="machine">Which machine to check</param>
	/// <returns>The working state of said machine</returns>
	public bool CheckMachine(MachineLists machine)
	{
		switch(machine)
		{
			case (MachineLists.WOODCHIPPER):
				return m_woodchipper.GetWorking();
			case (MachineLists.PAINTER):
				return m_painter.GetWorking();
			case (MachineLists.PRESS):
				return m_press.GetWorking();
			default:
				return false;
		}
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
					if (!m_woodchipper.GetWorking())
						Debug.Log("Woodchipper tried to break but is already broken");
					else
					{
						if (m_lastBreak != MachineLists.WOODCHIPPER)
						{
							m_woodchipper.BreakMachine();
							m_lastBreak = MachineLists.WOODCHIPPER;
							m_alarmSource.clip = m_woodchipperAlarm;
							m_alarmSource.Play();
							Debug.Log("Woodchipper broke");
						}
						else
						{
							Debug.Log("Woodchipper tried to break but was the last machine to break");
							BreakMachine();
						}

					}
				}
				else
					Debug.Log("Woodchipper tried to break but is disabled");
				break;
			case (1):
				if (m_pressEnabled)
				{
					if (!m_press.GetWorking())
						Debug.Log("Press tried to break but is already broken");
					else
					{
						if(m_lastBreak != MachineLists.PRESS)
						{
							m_press.BreakMachine();
							m_lastBreak = MachineLists.PRESS;
							m_alarmSource.clip = m_pressAlarm;
							m_alarmSource.Play();
							Debug.Log("Press broke");
						}
						else
						{
							Debug.Log("Press tried to break but was the last machine to break");
							BreakMachine();
						}
					}
				}
				else
					Debug.Log("Press tried to break but is disabled");
				break;
			case (2):
				if (m_painterEnabled)
				{
					if (!m_painter.GetWorking())
						Debug.Log("Painter tried to break but is already broken");
					else
					{
						if(m_lastBreak != MachineLists.PAINTER)
						{
							m_painter.BreakMachine();
							m_lastBreak = MachineLists.PAINTER;
							m_alarmSource.clip = m_painterAlarm;
							m_alarmSource.Play();
							Debug.Log("Painter broke");
						}
						else
						{
							Debug.Log("Painter tried to break but was the last machine to break");
							BreakMachine();
						}
					}
				}
				else
					Debug.Log("Painter tried to break but is disabled");
				break;
			case (3):
				if(m_lastBreak != MachineLists.NONE)
				{
					Debug.Log("No machine was broken");
				}
				else
				{
					Debug.Log("No machine was broken but no machine was broken last cycle");
					BreakMachine();
				}
				break;
			default:
				Debug.LogError("Something went horribly wrong with Random.Range");
				break;
		}
	}
}
