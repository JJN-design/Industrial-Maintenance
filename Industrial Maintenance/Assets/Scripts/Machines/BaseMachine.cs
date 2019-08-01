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

public class BaseMachine : MonoBehaviour
{
	//Whether or not the machine is currently working
	protected bool m_isWorking;

	//If the machine is not working, what the issue with it is
	protected MachineIssue m_issue;

	/// <summary>
	/// Calls for the machine to be broken
	/// </summary>
	/// <param name="issue">What the issue is</param>
	public void BreakMachine(MachineIssue issue)
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
	protected void FixMachine()
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
}
