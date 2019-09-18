using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Painter Interactable Variables

public enum PainterInteractableType
{
	RED_BUTTON,
	GREEN_BUTTON,
	BLUE_BUTTON
}

#endregion //Painter Interactable Variables

public class PainterInteractable : Interactable
{
	//The machine this interactable is linked to
	private Painter m_parent;

	//The kind of interactable this is
	private PainterInteractableType m_type;

	/// <summary>
	/// Creates the interactable and sets variables
	/// </summary>
	/// <param name="machine">The parent of this interactable</param>
	/// <param name="buttonType">The type of button this interactable is</param>
	public void Create(Painter machine, PainterInteractableType buttonType)
	{
		m_parent = machine;
		m_type = buttonType;
	}

	/// <summary>
	/// Code for interacting with this interactable
	/// </summary>
	public override void InteractWith()
	{
		//call base
		base.InteractWith();

		//inputs press into parent
		m_parent.InputPress(m_type);
	}
}