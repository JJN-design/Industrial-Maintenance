using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interactable : MonoBehaviour
{
	[Tooltip("The material for when this interactable is being interacted with")]
	[SerializeField] protected Material m_highlightedMaterial;
	//The previous material before this was interacted with
	private Material m_prevMaterial;

	/// <summary>
	/// Code for interacting with this interactable
	/// </summary>
	virtual public void InteractWith()
	{
		m_prevMaterial = GetComponent<Renderer>().material;
		GetComponent<Renderer>().material = m_highlightedMaterial;
	}

	/// <summary>
	/// Code for stopping interaction with this interactable
	/// </summary>
	virtual public void StopInteractingWith()
	{
		GetComponent<Renderer>().material = m_prevMaterial;
	}
}
