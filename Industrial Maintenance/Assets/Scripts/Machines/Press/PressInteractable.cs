using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PressInteractableType
{
	THIRD_BOLT,
	SECOND_BOLT,
	TIGHTEN_INK,
	LOOSEN_INK,
	YELLOW_LEVER,
	ORANGE_LEVER,
	RESTART_BUTTON
}
public class PressInteractable : Interactable
{
	//The press this interactable is linked to
	private Press m_parent;

	//Whether or not this interactable is on the second stage or not
	private bool m_secondStage;

	//The type of interactable this is
	private PressInteractableType m_interactableType;

	//How much time is lost when an incorrect button is interacted with
	private float m_incorrectTimeSubtraction;

	/// <summary>
	/// Sets a new correct button and whether or not it's second stage
	/// </summary>
	/// <param name="correct">The correct state of the button</param>
	/// <param name="secondStage">Whether this is the second stage or not</param>
	public void SetCorrect(bool correct, bool secondStage)
	{
		base.SetCorrect(correct);
		m_secondStage = secondStage;
	}

	/// <summary>
	/// Creates the interactable and sets up variables
	/// Should only be called from a press
	/// </summary>
	/// <param name="machine">The machine this interactable is linked to</param>
	/// <param name="interactableType">The type of interactable this is</param>
	/// <param name="incorrectTime">How much time should be subtracted on incorrect press</param>
	public void Create(Press machine, PressInteractableType interactableType, float incorrectTime)
	{
		m_parent = machine;
		m_interactableType = interactableType;
		m_incorrectTimeSubtraction = incorrectTime;
	}

	public override void InteractWith()
	{
		
	}
}
