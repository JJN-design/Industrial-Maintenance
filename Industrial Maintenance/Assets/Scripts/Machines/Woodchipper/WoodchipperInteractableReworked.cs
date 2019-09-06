using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WoodchipperInteractableTypeV2
{
    BUTTON_A,
    BUTTON_B,
    BUTTON_C,
    BUTTON_D,
    BUTTON_E,
    RED_LEVER,
    BLUE_LEVER
}

public class WoodchipperInteractableReworked : Interactable
{
    //The reworked woodchipper this interactable is linked to
    private WoodchipperReworked m_parent;

    //The type of interactable this is
    private WoodchipperInteractableTypeV2 m_interactableType;

    //How much time is lost when an incorrect interactable is pressed
    private float m_incorrectTimeSubtraction;

	/// <summary>
	/// Sets initial variables for interactable
	/// </summary>
	/// <param name="machine"></param>
	/// <param name="type"></param>
	/// <param name="incorrectTime"></param>
    public void Create(WoodchipperReworked machine, WoodchipperInteractableTypeV2 type, float incorrectTime)
    {
        m_parent = machine;
        m_interactableType = type;
        m_incorrectTimeSubtraction = incorrectTime;
    }

	/// <summary>
	/// Function to be called when this is interacted with
	/// </summary>
	public override void InteractWith()
	{
		base.InteractWith();
		//TODO
	}

	/// <summary>
	/// Function to be called when this is no longer interacted with
	/// </summary>
	public override void StopInteractingWith()
	{
		base.StopInteractingWith();
		//TODO
	}
}
