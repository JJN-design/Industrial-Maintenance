using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WoodchipperInteractableType
{
	SMALL_MAGENTA,
	SMALL_YELLOW,
	SMALL_BLACK,
	SMALL_CYAN,
	BIG_RED
}

public class WoodchipperInteractable : Interactable
{
	//The woodchipper this interactable is linked to
	private Woodchipper m_parent;

	//The type of button this interactable is
	private WoodchipperInteractableType m_buttonType;

	//Whether or not this button is on the second stage
	private bool m_secondStage = false;

	//How much time is lost when an incorrect button is pressed
	private float m_incorrectTimeSubtraction;

	/// <summary>
	/// Creates the button and sets up variables
	/// Should be called from a Woodchipper
	/// </summary>
	/// <param name="machine">The machine this button is linked to</param>
	/// <param name="buttonType">The type of button this button is</param>
	/// <param name="incorrectTime">How much time is subtracted when an incorrect button is pressed</param>
	public void Create(Woodchipper machine, WoodchipperInteractableType buttonType, float incorrectTime)
	{
		m_parent = machine;
		m_buttonType = buttonType;
		m_incorrectTimeSubtraction = incorrectTime;
	}

	/// <summary>
	/// Sets a new 'correct' state
	/// </summary>
	/// <param name="correct">The new correct state of this button</param>
	/// <param name="secondStage">Whether this button is in second stage</param>
	public void SetCorrect(bool correct, bool secondStage)
	{
		base.SetCorrect(correct);
		m_secondStage = secondStage;
	}

	/// <summary>
	/// Function to be called when this is interacted with
	/// </summary>
	public override void InteractWith()
	{
		if(!m_isCorrect) //If this button isn't correct, subtract time remaining before failure
		{
			Debug.Log("Incorrect button!");
			m_parent.SubtractTime(m_incorrectTimeSubtraction);
			return;
		}
		Debug.Log("Correct button!");
		m_isCorrect = false; //Reset correct state
		if(m_buttonType != WoodchipperInteractableType.BIG_RED && !m_secondStage) //If the button isn't a 'big red' button and we're not on the second stage, check light colour
		{
			m_parent.EnableLight(); //sets light colour to correct one
			switch(m_parent.GetLightColour())
			{
				case (LightColour.BLUE): 
					switch (m_buttonType) //If a blue light, set new button to clockwise small button
					{
						case (WoodchipperInteractableType.SMALL_MAGENTA):
							m_parent.SetNewButton(WoodchipperInteractableType.SMALL_CYAN, true);
							break;
						case (WoodchipperInteractableType.SMALL_YELLOW):
							m_parent.SetNewButton(WoodchipperInteractableType.SMALL_MAGENTA, true);
							break;
						case (WoodchipperInteractableType.SMALL_BLACK):
							m_parent.SetNewButton(WoodchipperInteractableType.SMALL_YELLOW, true);
							break;
						case (WoodchipperInteractableType.SMALL_CYAN):
							m_parent.SetNewButton(WoodchipperInteractableType.SMALL_BLACK, true);
							break;
						default:
							break;
					}
					break;
				case (LightColour.ORANGE): //If an orange light, set new button to big red button
					m_parent.SetNewButton(WoodchipperInteractableType.BIG_RED, true);
					break;
				case (LightColour.PINK): //If a pink light, set new button to diagonal button
					switch (m_buttonType)
					{
						case (WoodchipperInteractableType.SMALL_MAGENTA):
							m_parent.SetNewButton(WoodchipperInteractableType.SMALL_BLACK, true);
							break;
						case (WoodchipperInteractableType.SMALL_YELLOW):
							m_parent.SetNewButton(WoodchipperInteractableType.SMALL_CYAN, true);
							break;
						case (WoodchipperInteractableType.SMALL_BLACK):
							m_parent.SetNewButton(WoodchipperInteractableType.SMALL_MAGENTA, true);
							break;
						case (WoodchipperInteractableType.SMALL_CYAN):
							m_parent.SetNewButton(WoodchipperInteractableType.SMALL_YELLOW, true);
							break;
						default:
							break;
					}
					break;
				default:
					break;
			}
		}
		else //If second stage or big red button, fix the machine and reset 'second stage'
		{
			m_parent.FixMachine();
			m_secondStage = false;
		}
	}

	/// <summary>
	/// Code for stopping interaction with this interactable
	/// Empty on woodchipper interactable
	/// </summary>
	public override void StopInteractingWith()
	{
		return;
	}
}
