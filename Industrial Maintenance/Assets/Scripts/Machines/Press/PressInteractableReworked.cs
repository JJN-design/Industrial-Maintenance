using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PressInteractableType
{
	RED_LEVER,
	GREEN_LEVER,
	BLUE_LEVER,
	YELLOW_LEVER,
	RESET_BUTTON,
	BOLT_1,
	BOLT_2,
	BOLT_3,
	BOLT_4,
	BOLT_5,
	BOLT_6
}

public class PressInteractableReworked : Interactable
{
	//The reworked press this interactable is linked to
	private PressReworked m_parent;

	//The type of interactable this is
	private PressInteractableType m_interactableType;

	//How much time is lost when an incorrect interactable is pressed
	private float m_incorrectTimeSubtraction;

	//What stage of fixing the machine are we at?
	private int m_currentStage;

	/// <summary>
	/// Sets initial variables for interactable
	/// </summary>
	/// <param name="machine"></param>
	/// <param name="type"></param>
	/// <param name="incorrectTime"></param>
	public void Create(PressReworked machine, PressInteractableType type, float incorrectTime)
	{
		m_parent = machine;
		m_interactableType = type;
		m_incorrectTimeSubtraction = incorrectTime;
	}
}
