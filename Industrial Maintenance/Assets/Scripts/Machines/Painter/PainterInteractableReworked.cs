using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Painter Interactable Variables

public enum PainterInteractableReworkedType
{
	RED_BUTTON,
	GREEN_BUTTON,
	BLUE_BUTTON
}

#endregion //Painter Interactable Variables

public class PainterInteractableReworked : Interactable
{
	private PainterReworked m_parent;

	private PainterInteractableReworkedType m_type;

	/// <summary>
	/// Creates the interactable
	/// </summary>
	/// <param name="parent">The painter this interactable is attached to</param>
	/// <param name="type">The type of interactable this is</param>
	public void Create(PainterReworked parent, PainterInteractableReworkedType type)
	{
		m_parent = parent;
		m_type = type;
	}

	/// <summary>
	/// 
	/// </summary>
	public override void InteractWith()
	{
		base.InteractWith();
	}

	/// <summary>
	/// 
	/// </summary>
	public override void StopInteractingWith()
	{
		base.StopInteractingWith();
	}
}
