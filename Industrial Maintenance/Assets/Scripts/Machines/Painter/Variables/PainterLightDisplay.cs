using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterLightDisplay : MonoBehaviour
{
	private Painter m_parent;

	private Renderer m_renderer;

	/// <summary>
	/// Sets the renderer and parent
	/// </summary>
	public void Create(Painter parent)
	{
		m_renderer = GetComponent<Renderer>();
		m_parent = parent;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="colour"></param>
	public void EnableLight(Color colour)
	{
		m_renderer.material.color = colour;
	}

	/// <summary>
	/// Disables the light
	/// </summary>
	public void DisableLight()
	{
		//m_renderer.material.color = m_parent
	}
}
