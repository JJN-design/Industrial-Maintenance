using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dangerometer : MonoBehaviour
{
	[Header("Pointer rotation")]
	[Tooltip("The rotation for the minimum danger level")]
	[SerializeField] private float m_minRotation;
	[Tooltip("The rotation for the maximum danger level")]
	[SerializeField] private float m_maxRotation;
	[Tooltip("The axis of rotation (Values should be between 0 and 1)")]
	[SerializeField] private Vector3 m_rotationAxis;

	[Header("GameObjects")]
	[Tooltip("The object for the pointer")]
	[SerializeField] private GameObject m_pointer;

	//the current timer and the max time before failure
	private float m_currentFailTimer;
	private float m_timeBeforeFailure;

	//the percentage of danger we currently have
	private float m_dangerPercentage = 0.0f;

	//whether or not the machine is broken
	private bool m_isMachineBroken = false;

	/// <summary>
	/// Sets what the current timer is
	/// </summary>
	/// <param name="timer">The current timer</param>
	public void SetCurrentFailTimer(float timer)
	{
		m_currentFailTimer = timer;
	}

	/// <summary>
	/// hey can i get a uhhhhhhhhhhhhh box?
	/// </summary>
	/// <param name="broke">box machine broke</param>
	public void SetBroken(bool broke)
	{
		m_isMachineBroken = broke;
	}

	/// <summary>
	/// Creates initial variables on dangerometer
	/// </summary>
	/// <param name="maxFailTime">The maximum time allowed for a machine to be broken before level failure</param>
	public void Create(float maxFailTime)
	{
		m_timeBeforeFailure = maxFailTime;
	}

	/// <summary>
	/// Called each frame
	/// </summary>
	private void Update()
	{
		if(m_isMachineBroken)
		{
			m_dangerPercentage = m_currentFailTimer / m_timeBeforeFailure;
			//TODO rotate pointer
		}
	}
}
