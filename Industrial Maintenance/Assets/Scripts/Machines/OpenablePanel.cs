using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenablePanel : MonoBehaviour
{
	[Header("Rotation")]
	[Tooltip("The axis of rotation this panel should rotate in")]
	[SerializeField] private Vector3 m_rotationAxis;
	[Tooltip("How much object should rotate in")]
	[SerializeField] [Range(-180.0f, 180.0f)] private float m_rotationAmount;

	[Header("Audio")]
	[Tooltip("The audio source of the panel")]
	[SerializeField] private AudioSource m_panelOpenSound;

	//whether or not this object has been rotated
	private bool m_rotated = false;

	/// <summary>
	/// If the panel is not already opened, open it
	/// </summary>
	public void Open()
	{
		if (!m_rotated)
		{
			m_panelOpenSound.Play();
			transform.Rotate(m_rotationAxis, m_rotationAmount);
			m_rotated = true;
		}
	}

	/// <summary>
	/// If the panel is already opened, close it
	/// </summary>
	public void Close()
	{
		if(m_rotated)
		{
			transform.Rotate(m_rotationAxis, -m_rotationAmount);
			m_rotated = false;
		}
	}
}
