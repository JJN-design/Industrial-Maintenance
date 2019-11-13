using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StompState
{
	STATIC,
	STOMPING,
	STOMPED,
	RETRACTING
}

public class Boot : MonoBehaviour
{
	[Header("GameObjects")]
	[Tooltip("The object of the boot")]
	[SerializeField] private GameObject m_boot;
	[Tooltip("The boot explosion particle system")]
	[SerializeField] private ParticleSystem m_bootParticles;

	[Header("Positional Variables")]
	//the default vector3 position of the boot
	private Vector3 m_defaultPosition;
	//the stomped vector3 position of the boot
	private Vector3 m_stompedPosition;
	[Tooltip("How much in the Y axis the boot should move when stomped down")]
	[SerializeField] private float m_yMovement;

	[Header("Timing Variables")]
	[Tooltip("How quickly this boot should stomp down")]
	[SerializeField] private float m_stompTime;
	[Tooltip("How long the boot should stay stomped")]
	[SerializeField] private float m_stayTime;
	[Tooltip("How long the boot should take to retract")]
	[SerializeField] private float m_retractTime;

	//the current state of the boot
	private StompState m_stompState = StompState.STATIC;
	
	//timer for stomping
	private float m_timer = 0.0f;

	//Whether to play the particles or not
	private bool m_correctBox;

	/// <summary>
	/// code to be called on scene load
	/// </summary>
	private void Awake()
	{
		m_defaultPosition = m_boot.transform.position;

		Vector3 stompPos = m_defaultPosition;
		stompPos.y += m_yMovement;

		m_stompedPosition = stompPos;
	}

	/// <summary>
	/// code to be called each frame
	/// </summary>
	private void Update()
	{
		switch(m_stompState)
		{
			case (StompState.STATIC):
				return;
			case (StompState.STOMPING):
				//increment timer
				m_timer += Time.deltaTime;

				//if timer is done, change state to next state
				if (m_timer >= m_stompTime)
				{
					m_stompState = StompState.STOMPED;
					m_timer -= m_stompTime;
					if(!m_correctBox)
						m_bootParticles.Play();
					return;
				}
				//calculate percentage
				float stompPercentage = m_timer / m_stompTime;

				//calculate the new position of the boot
				Vector3 newPos = Vector3.Lerp(m_defaultPosition, m_stompedPosition, stompPercentage);

				//set position
				m_boot.transform.position = newPos;
				break;
			case (StompState.STOMPED):
				//increment timer
				m_timer += Time.deltaTime;

				//if timer is done, change state to next state
				if (m_timer >= m_stayTime)
				{
					m_stompState = StompState.RETRACTING;
					m_timer -= m_stayTime;
					return;
				}
				break;
			case (StompState.RETRACTING):
				//increment timer
				m_timer += Time.deltaTime;

				//if timer is done, change state to next state
				if (m_timer >= m_retractTime)
				{
					m_stompState = StompState.STATIC;
					m_timer -= m_retractTime;
					return;
				}
				//calculate percentage
				float retractPercentage = m_timer / m_stompTime;

				//calculate the new position of the boot
				Vector3 newRetractPos = Vector3.Lerp(m_stompedPosition, m_defaultPosition, retractPercentage);

				//set position
				m_boot.transform.position = newRetractPos;
				break;
		}
	}

	/// <summary>
	/// Starts the stomp cycle
	/// </summary>
	/// <param name="correctBox">Whether or not the box was a correct box</param>
	public void Stomp(bool correctBox)
	{
		m_correctBox = correctBox;
		m_stompState = StompState.STOMPING;
		m_timer = 0.0f;
	}
}
