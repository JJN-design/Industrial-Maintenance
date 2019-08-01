using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interactable : MonoBehaviour
{
	//Whether or not pressing this button is the correct thing to do
	protected bool m_isCorrect;

	virtual public void SetCorrect(bool correct)
	{
		m_isCorrect = correct;
	}

	abstract public void InteractWith();
}
