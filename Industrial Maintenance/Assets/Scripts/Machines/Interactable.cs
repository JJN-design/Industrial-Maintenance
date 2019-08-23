using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interactable : MonoBehaviour
{
	//Whether or not using this interactable is the correct thing to do
	protected bool m_isCorrect;

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
	abstract public void InteractWith();

	/// <summary>
	/// Code for stopping interaction with this interactable
	/// </summary>
	abstract public void StopInteractingWith();
}
