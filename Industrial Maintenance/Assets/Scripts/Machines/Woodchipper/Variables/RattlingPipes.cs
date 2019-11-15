﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RattlingPipes : MonoBehaviour
{
	//whether or not this pipe is currently rattling
	private bool m_isRattling;

	//the starting position of this pipe
	private Vector3 m_startPos;

	//variables for shaking
	[Header("Shaking variables")]
	[Tooltip("How fast this pipe shakes in each axis")]
	[SerializeField] private Vector3 m_shakeSpeed; 
	[Tooltip("How much this pipe shakes in each axis")]
	[SerializeField] private Vector3 m_shakeIntensity;

	[Header("Audio")]
	[Tooltip("The audio source for a rattling pipe")]
	[SerializeField] private AudioSource m_rattleNoise;

	/// <summary>
	/// Set starting position
	/// </summary>
	private void Awake()
	{
		m_startPos = transform.position;
	}

	/// <summary>
	/// Called each frame, if shaking is active, move the object around
	/// </summary>
	private void Update()
	{
		if(m_isRattling)
		{
			Vector3 newPosition = transform.position;
			newPosition.x = newPosition.x + Mathf.Sin(Time.time * m_shakeSpeed.x) * m_shakeIntensity.x;
			newPosition.y = newPosition.y + Mathf.Sin(Time.time * m_shakeSpeed.y) * m_shakeIntensity.y;
			newPosition.z = newPosition.z + Mathf.Sin(Time.time * m_shakeSpeed.z) * m_shakeIntensity.z;
			transform.position = newPosition;
		}
	}

	/// <summary>
	/// Gets the rattling state of the pipe
	/// </summary>
	/// <returns></returns>
	public bool GetRattlingState() { return m_isRattling; }

	/// <summary>
	/// Set rattling to true
	/// </summary>
	public void StartRattling()
	{
		m_isRattling = true;
		m_rattleNoise.Play();
	}

	/// <summary>
	/// Reset position and set rattling to false
	/// </summary>
	public void StopRattling()
	{
		transform.position = m_startPos;
		m_isRattling = false;
		m_rattleNoise.Stop();
	}
}
