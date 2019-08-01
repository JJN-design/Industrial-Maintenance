using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MachineIssue
{
	FIXED, 
	ON_FIRE,
	DUST_PLUMES,
	SPARKING
}

abstract public class BaseMachine : MonoBehaviour
{
	//Whether or not the machine is currently working
	protected bool m_isWorking = true;

	//If the machine is not working, what the issue with it is
	protected MachineIssue m_issue;

	[Header("Specific machine variables")]
	[Tooltip("How long before a fail state is reached while this machine is broken")]
	[SerializeField] private float m_timeBeforeFailure;
	private float m_failTimer;

	/// <summary>
	/// Update function, is called every frame
	/// </summary>
	void Update()
	{
		if(!m_isWorking)
		{
			m_failTimer += Time.deltaTime;
			if(m_failTimer >= m_timeBeforeFailure)
			{
				Debug.Log("Level failed");
				//TODO: Failure state
			}
		}
	}

	/// <summary>
	/// Calls for the machine to be broken
	/// </summary>
	/// <param name="issue">What the issue is</param>
	virtual public void BreakMachine(MachineIssue issue)
	{
		if (issue == MachineIssue.FIXED) //if the issue given is FIXED, don't break the machine
			return;
		if (!m_isWorking) //if the machine is already broken, don't rebreak it
			return;
		m_isWorking = false;
		m_issue = issue;
	}

	/// <summary>
	/// Calls for the machine to return to a working state
	/// </summary>
	public void FixMachine()
	{
		m_isWorking = true;
		m_issue = MachineIssue.FIXED;
	}

	/// <summary>
	/// Gets the working state of the machine
	/// </summary>
	/// <returns>The working state</returns>
	public bool GetWorking()
	{
		return m_isWorking;
	}

	abstract public void GenerateVariables();

	/// <summary>
	/// Reduces how much time there is left to fix the machine
	/// </summary>
	/// <param name="time">How much time to subtract from your time left</param>
	public void SubtractTime(float time)
	{
		m_failTimer += time;
	}
}
