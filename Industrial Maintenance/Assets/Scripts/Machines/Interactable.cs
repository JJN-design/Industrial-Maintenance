using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interactable : MonoBehaviour
{
	//Whether or not using this interactable is the correct thing to do
	protected bool m_isCorrect;

	[Tooltip("The material for when this interactable is being interacted with")]
	[SerializeField] protected Material m_highlightedMaterial;
	//The previous material before this was interacted with
	private Material m_prevMaterial;

	/// <summary>
	/// Set the correct state of this interactable
	/// </summary>
	/// <param name="correct">Whether or not this interactable is the correct one to use</param>
	virtual public void SetCorrect(bool correct)
	{
		m_isCorrect = correct;
	}

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
