using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
	SMALL_MAGENTA,
	SMALL_YELLOW,
	SMALL_BLACK,
	SMALL_CYAN,
	BIG_RED
}

public class WoodchipperInteractable : Interactable
{
	private Woodchipper m_parent;

	private ButtonType m_buttonType;

	private bool m_secondStage;

	public void Create(Woodchipper machine, ButtonType buttonType)
	{
		m_parent = machine;
		m_buttonType = buttonType;
	}

	public void SetCorrect(bool correct, bool secondStage)
	{
		base.SetCorrect(correct);
		m_secondStage = secondStage;
	}

	public override void InteractWith()
	{
		if(!m_isCorrect)
		{
			m_parent.SubtractTime(60.0f);
			return;
		}
		m_isCorrect = false;
		if(m_buttonType != ButtonType.BIG_RED && !m_secondStage)
		{
			switch(m_parent.GetLightColour())
			{
				case (LightColour.BLUE):
					switch (m_buttonType)
					{
						case (ButtonType.SMALL_MAGENTA):
							m_parent.SetNewButton(ButtonType.SMALL_CYAN, true);
							break;
						case (ButtonType.SMALL_YELLOW):
							m_parent.SetNewButton(ButtonType.SMALL_MAGENTA, true);
							break;
						case (ButtonType.SMALL_BLACK):
							m_parent.SetNewButton(ButtonType.SMALL_YELLOW, true);
							break;
						case (ButtonType.SMALL_CYAN):
							m_parent.SetNewButton(ButtonType.SMALL_BLACK, true);
							break;
						default:
							break;
					}
					break;
				case (LightColour.ORANGE):
					m_parent.SetNewButton(ButtonType.BIG_RED, true);
					break;
				case (LightColour.PINK):
					switch (m_buttonType)
					{
						case (ButtonType.SMALL_MAGENTA):
							m_parent.SetNewButton(ButtonType.SMALL_BLACK, true);
							break;
						case (ButtonType.SMALL_YELLOW):
							m_parent.SetNewButton(ButtonType.SMALL_CYAN, true);
							break;
						case (ButtonType.SMALL_BLACK):
							m_parent.SetNewButton(ButtonType.SMALL_MAGENTA, true);
							break;
						case (ButtonType.SMALL_CYAN):
							m_parent.SetNewButton(ButtonType.SMALL_YELLOW, true);
							break;
						default:
							break;
					}
					break;
				default:
					break;
			}
		}
		else
		{
			m_parent.FixMachine();
			m_secondStage = false;
		}
	}
}
