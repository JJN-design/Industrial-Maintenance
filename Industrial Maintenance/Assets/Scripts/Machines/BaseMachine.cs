﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseMachine : MonoBehaviour
{
	//Whether or not the machine is currently working
	protected bool m_isWorking = true;

	//variable for if machine is generated
	protected bool m_isGenerated = false;

	[Header("Particle Generators")]
	[Tooltip("The broken particle system")]
	[SerializeField] private ParticleSystem m_brokenParticles;

	//The machine manager
	protected MachineManager m_machineManager;

	[Header("Specific machine variables")]
	[Tooltip("How long before a fail state is reached while this machine is broken")]
	[SerializeField] private float m_timeBeforeFailure;
	private float m_failTimer = 0.0f;
	[Tooltip("The dangerometer of this machine")]
	[SerializeField] private Dangerometer m_dangerometer;

	/// <summary>
	/// Stops particles
	/// </summary>
	void Awake()
	{
		m_brokenParticles.Stop();
		m_dangerometer.Create(m_timeBeforeFailure);
	}

	/// <summary>
	/// Update function, is called every frame
	/// </summary>
	void Update()
	{
		if(!m_isWorking)
		{
			m_failTimer += Time.deltaTime;
			m_dangerometer.SetCurrentFailTimer(m_failTimer);
			if(m_failTimer >= m_timeBeforeFailure)
			{
				Debug.Log("Level failed due to " + gameObject.name);

				m_machineManager.GetController().DisableMovement();
				m_machineManager.GetController().GetUI().ShowScores();
			}
		}
	}

	/// <summary>
	/// Calls for the machine to be broken
	/// </summary>
	virtual public void BreakMachine()
	{
		if (!m_isWorking) //if the machine is already broken, don't rebreak it
			return;
		m_isWorking = false;
		m_dangerometer.SetBroken(true);
		m_brokenParticles.Play();
	}

	/// <summary>
	/// Calls for the machine to return to a working state
	/// </summary>
	virtual public void FixMachine()
	{
		m_brokenParticles.Stop();
		m_isWorking = true;
		m_dangerometer.SetBroken(false);
		Debug.Log(gameObject.name + " was fixed!");
		m_failTimer = 0.0f;
	}

	/// <summary>
	/// Gets the working state of the machine
	/// </summary>
	/// <returns>The working state</returns>
	public bool GetWorking()
	{
		return m_isWorking;
	}

	/// <summary>
	/// Abstract function for generating machine variables
	/// Called from the machine manager
	/// </summary>
	/// <param name="manager">The machine manager</param>
	abstract public void GenerateVariables(MachineManager manager);

	/// <summary>
	/// Reduces how much time there is left to fix the machine
	/// </summary>
	/// <param name="time">How much time to subtract from your time left</param>
	public void SubtractTime(float time)
	{
		m_failTimer += time;
	}

	/// <summary>
	/// Gets the max time before failure
	/// </summary>
	/// <returns>The time before failure</returns>
	public float GetFailureTime() { return m_timeBeforeFailure; }

	/// <summary>
	/// Gets the current timer
	/// </summary>
	/// <returns></returns>
	public float GetCurrentTimer() { return m_failTimer; }
}
