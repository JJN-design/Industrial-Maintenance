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
	private Press m_parent;
	public override void InteractWith()
	{
		
	}
}
