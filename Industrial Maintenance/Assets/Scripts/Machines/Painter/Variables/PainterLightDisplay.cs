using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterLightDisplay : MonoBehaviour
{
	[SerializeField] private Renderer m_renderer;

	private Material m_disabledMaterial;

	private Material m_enabledMaterial;
	/// <summary>
	/// Sets the renderer and parent
	/// </summary>
	/// <param name="disabled">The material to be used when this light is disabled</param>
	/// <param name="enabled">The material to be used when this light is enabled</param>
	public void Create(Material disabled, Material enabled)
	{
		m_enabledMaterial = enabled;
		m_disabledMaterial = disabled;
		m_renderer.material = m_disabledMaterial;
	}

	/// <summary>
	/// Turns the light on and sets the colour
	/// </summary>
	/// <param name="colour">The colour the light is set to</param>
	public void EnableLight(Color colour)
	{
		m_renderer.material = m_enabledMaterial;
		m_renderer.material.SetColor("_EmissionColor", colour);
	}

	/// <summary>
	/// Disables the light
	/// </summary>
	public void DisableLight()
	{
		m_renderer.material = m_disabledMaterial;
	}
}
