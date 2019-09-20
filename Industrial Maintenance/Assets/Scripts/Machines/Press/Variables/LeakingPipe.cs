﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeakingPipe : MonoBehaviour
{
	[Header("Particle Systems")]
	[Tooltip("The ink particles")]
	[SerializeField] private ParticleSystem m_inkParticles;
	[Tooltip("The water particles")]
	[SerializeField] private ParticleSystem m_waterParticles;

	//the velocities of the leaking
	private float m_lowLeakVelocity;
	private float m_highLeakVelocity;

	//the emission rate of the leaking
	private float m_lowLeakEmission;
	private float m_highLeakEmission;

	//the current leak liquid and velocity
	private PressLeakLiquid m_liquid;
	private PressLeakVelocity m_velocity;

	/// <summary>
	/// Creates the object and sets variables
	/// </summary>
	/// <param name="parent">The parent of the machine</param>
	/// <param name="lowVelocity">The velocity of the 'low' leaking velocity</param>
	/// <param name="highVelocity">The velocity of the 'high' leaking velocity</param>
	/// <param name="liquid">The liquid that the machine will initially leak</param>
	/// <param name="velocity">The velocity that the machine will initially leak at</param>
	/// <param name="lowEmission">The emission rate of the 'low' leaking velocity</param>
	/// <param name="highEmission">The emission rate of the 'high' leaking velocity</param>
	public void Create(float lowVelocity, float highVelocity, PressLeakLiquid liquid, PressLeakVelocity velocity, float lowEmission, float highEmission)
	{
		m_lowLeakVelocity = lowVelocity;
		m_highLeakVelocity = highVelocity;
		m_liquid = liquid;
		m_velocity = velocity;
		m_lowLeakEmission = lowEmission;
		m_highLeakEmission = highEmission;
	}

	/// <summary>
	/// Updates the current liquid and velocity
	/// </summary>
	/// <param name="liquid">The new liquid that the machine will leak</param>
	/// <param name="velocity">The new velocity that the machine will leak at</param>
	public void UpdateLeak(PressLeakLiquid liquid, PressLeakVelocity velocity)
	{
		m_liquid = liquid;
		m_velocity = velocity;

		ParticleSystem.MainModule inkMain = m_inkParticles.main;
		ParticleSystem.MainModule waterMain = m_waterParticles.main;
		ParticleSystem.EmissionModule inkEmission = m_inkParticles.emission;
		ParticleSystem.EmissionModule waterEmission = m_waterParticles.emission;

		//sets the velocities
		switch(m_velocity)
		{
			case (PressLeakVelocity.HIGH):
				inkMain.startSpeed = m_highLeakVelocity;
				waterMain.startSpeed = m_highLeakVelocity;
				inkEmission.rateOverTime = new ParticleSystem.MinMaxCurve(m_highLeakEmission);
				waterEmission.rateOverTime = new ParticleSystem.MinMaxCurve(m_highLeakEmission);
				break;
			case (PressLeakVelocity.LOW):
				inkMain.startSpeed = m_lowLeakVelocity;
				waterMain.startSpeed = m_lowLeakVelocity;
				inkEmission.rateOverTime = new ParticleSystem.MinMaxCurve(m_lowLeakEmission);
				waterEmission.rateOverTime = new ParticleSystem.MinMaxCurve(m_lowLeakEmission);
				break;
			default:
				Debug.LogError("Invalid PressLeakVelocity passed on LeakingPipe!");
				break;
		}
	}

	/// <summary>
	/// Starts the pipe leak
	/// </summary>
	public void StartLeaking()
	{
		switch(m_liquid)
		{
			case (PressLeakLiquid.INK):
				m_inkParticles.Play();
				break;
			case (PressLeakLiquid.WATER):
				m_waterParticles.Play();
				break;
			default:
				Debug.LogError("Invalid PressLeakLiquid on LeakingPipe!");
				break;
		}
	}

	/// <summary>
	/// Stops the pipe leak
	/// </summary>
	public void StopLeaking()
	{
		m_inkParticles.Stop();
		m_waterParticles.Stop();
	}
}
