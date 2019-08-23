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

	//The type of interactable this is
	private PressInteractableType m_interactableType;

	//How much time is lost when an incorrect button is interacted with
	private float m_incorrectTimeSubtraction;

	//Whether or not this interactable is currently pressed
	private bool m_isPressed;

	//whether or not this button needs holding (unneccesary for non-restart buttons)
	private bool m_needsHolding;

	//how long the restart button needs to be held if it is a held button
	private float m_heldTime;

	//how long the restart button can be held for if it's not a held button
	private float m_maxHeldTime;

	//how long the timer has been held for
	private float m_heldTimer;

	/// <summary>
	/// 
	/// </summary>
	private void Update()
	{
		if(m_interactableType == PressInteractableType.RESTART_BUTTON && m_isPressed) //if restart button and is currently being pressed
		{
			m_heldTimer += Time.deltaTime; //increment timer
			if(!m_needsHolding && m_heldTimer >= m_maxHeldTime) //if restart button doesn't need holding and was held for too long, subtract remaining time
			{
				Debug.Log("Held too long!");
				m_parent.SubtractTime(m_incorrectTimeSubtraction);
				return;
			}
			else if (m_needsHolding && m_heldTimer >= m_heldTime) //if restart button needed holding and has been held for long enough, fix machine
			{
				Debug.Log("Restart button was held long enough!");
				m_parent.FixMachine();
				return;
			}
		}
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

	/// <summary>
	/// Creates the interactable and sets up variables
	/// Should only be called from a press
	/// Should only be called for a restart button specifically
	/// </summary>
	/// <param name="machine">The machine this interactable is linked to</param>
	/// <param name="interactableType">The type of interactable this is</param>
	/// <param name="incorrectTime">How much time should be subtracted on incorrect press</param>
	/// <param name="heldTime">How long this button should need to be held for</param>
	public void Create(Press machine, PressInteractableType interactableType, float incorrectTime, float heldTime, float maxHeldTime)
	{
		if (interactableType != PressInteractableType.RESTART_BUTTON)
		{
			Debug.LogError("This PressInteractable.Create() overload should only be used with RESTART_BUTTON");
			return;
		}
		m_parent = machine;
		m_interactableType = interactableType;
		m_incorrectTimeSubtraction = incorrectTime;
		m_heldTime = heldTime;
		m_maxHeldTime = maxHeldTime;
		RPMRating RPM = machine.GetRPM();
		if (RPM == RPMRating.RPM856 || RPM == RPMRating.RPM902)
			m_needsHolding = true;
		else
			m_needsHolding = false;
	}

	/// <summary>
	/// Function to be called when this interactable is interacted with
	/// </summary>
	public override void InteractWith()
	{
		if(!m_isCorrect) //if button is incorrect, subtract time remaining and stop going through code
		{
			Debug.Log("Incorrect button!");
			m_parent.SubtractTime(m_incorrectTimeSubtraction);
			return;
		}

		if(m_interactableType == PressInteractableType.RESTART_BUTTON) //if this is the restart button, start held sequence
			m_isPressed = true;

		m_isCorrect = false; //reset correct state

		Debug.Log("Correct button!");
		m_parent.SetNewInteractable(PressInteractableType.RESTART_BUTTON); //set restart button to new correct button
	}

	/// <summary>
	/// Function to be called when this interactable is stopped being interacted with
	/// </summary>
	public override void StopInteractingWith()
	{
		if(m_isPressed) //if already pressed, continue with code
		{
			m_isPressed = false; //reset pressed state
			m_heldTimer = 0.0f; //reset timer
			if (m_needsHolding && m_heldTimer < m_heldTime) //if this button needed holding and was not held long enough, subtract time
			{
				Debug.Log("Not held long enough!");
				m_parent.SubtractTime(m_incorrectTimeSubtraction);
				return;
			}
			else if (!m_needsHolding && m_heldTimer < m_maxHeldTime) //if this button did not need holding and was held for small enough time, fix machine
			{
				Debug.Log("Restart button wasn't held too long!");
				m_parent.FixMachine();
				return;
			}
		}
	}
}
