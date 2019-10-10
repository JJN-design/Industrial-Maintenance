using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterLightDisplay : MonoBehaviour
{
	[SerializeField] private Renderer m_renderer;

	private Color m_disabledColour;
	/// <summary>
	/// Sets the renderer and parent
	/// </summary>
	public void Create(Color disabled)
	{
		m_disabledColour = disabled;
		m_renderer.material.color = m_disabledColour;
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
		m_renderer.material.color = m_disabledColour;
	}
}
