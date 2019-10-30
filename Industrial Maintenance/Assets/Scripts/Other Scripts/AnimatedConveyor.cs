using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedConveyor : MonoBehaviour
{
	[Header("Animation Settings")]
	[Tooltip("The axis the conveyor will move in")]
	[SerializeField] private Vector2 m_axis;
	[Tooltip("The speed this conveyor will animate at")]
	[SerializeField] private float m_animateSpeed;
	[Tooltip("The renderer to animate")]
	[SerializeField] private Renderer m_renderer;

	/// <summary>
	/// Update function, called every frame
	/// </summary>
	private void Update()
	{
		float offset = Time.time * m_animateSpeed;
		m_renderer.material.SetTextureOffset("_MainTex", m_axis * offset);
	}
}
