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
	private int m_currentStage = 0;

	//how long this button has been held for
	private float m_holdTimer = 0.0f;

	//How long before a press is considered a long press
	private float m_pressThreshold;

	//Whether or not this button is currently being held
	private bool m_currentlyHeld = false;

	/// <summary>
	/// Sets initial variables for interactable
	/// </summary>
	/// <param name="machine">The machine this interactable is a child of</param>
	/// <param name="type">The type of interactable this is</param>
	/// <param name="incorrectTime">How much time is lost on an incorrect press</param>
	/// <param name="threshold">How long before a press is considered a long press</param>
	public void Create(PressReworked machine, PressInteractableType type, float incorrectTime, float threshold)
	{
		m_parent = machine;
		m_interactableType = type;
		m_incorrectTimeSubtraction = incorrectTime;
		m_pressThreshold = threshold;
	}

	/// <summary>
	/// Called every frame, updates currently held time
	/// </summary>
	private void Update()
	{
		if (m_currentlyHeld)
			m_holdTimer += Time.deltaTime;
	}

	/// <summary>
	/// Sets a new stage on this interactable
	/// </summary>
	/// <param name="stage">The stage</param>
	public void SetNewStage(int stage)
	{
		m_currentStage = stage;
	}

	/// <summary>
	/// Code to be called when this interactable is interacted with
	/// </summary>
	public override void InteractWith()
	{
		//call base
		base.InteractWith();

		//set currently held to true
		m_currentlyHeld = true;

		//if parent is not working
		if(!m_parent.GetWorking())
		{
			switch(m_currentStage)
			{
				case (0):
					bool stageOneCorrect = StageOne();
					if (stageOneCorrect)
					{
						m_parent.StartLeaking();
						InteractCorrect();
					}
					else
						InteractFail();
					break;
				case (1):
					bool stageTwoCorrect = StageTwo();
					if (stageTwoCorrect)
						InteractCorrect();
					else
						InteractFail();
					break;
			}
		}
	}

	/// <summary>
	/// Code to be called when this interactable is no longer interacted with
	/// </summary>
	public override void StopInteractingWith()
	{
		//call base
		base.StopInteractingWith();

		//set currently held to false
		m_currentlyHeld = false;



		//if parent is not working
		if(!m_parent.GetWorking())
		{
			switch (m_currentStage)
			{
				case (0):
					bool stageOneCorrect = StageOne();
					if (stageOneCorrect)
					{
						m_parent.StartLeaking();
						InteractCorrect();
					}
					else
						InteractFail();
					break;
				case (1):
					bool stageTwoCorrect = StageTwo();
					if (stageTwoCorrect)
						InteractCorrect();
					else
						InteractFail();
					break;
				case (2):
					bool stageThreeCorrect = StageThree();
					if (stageThreeCorrect)
						m_parent.FixMachine();
					else
						InteractFail();
					break;
			}
		}

		m_holdTimer = 0.0f;
	}

	/// <summary>
	/// Function to be called when the machine is fixed
	/// </summary>
	public void MachineFixed()
	{
		m_parent.SetNewStage(0);
	}

	/// <summary>
	/// Code to be called when this isn't the correct interactable
	/// </summary>
	private void InteractFail()
	{
		m_parent.SubtractTime(m_incorrectTimeSubtraction);
		Debug.Log("Incorrect interactable press for Press rework at stage " + m_currentStage.ToString());
	}

	/// <summary>
	/// Code to be called when this is the correct interactable and is not the last stage
	/// </summary>
	private void InteractCorrect()
	{
		m_parent.SetNewStage(m_currentStage + 1);
		Debug.Log("Correct interactable press for Press rework at stage " + m_currentStage.ToString());
	}

	/// <summary>
	/// Code to be called on stage 1 of fixing the machine
	/// </summary>
	/// <returns>Whether or not this was the correct interactable</returns>
	private bool StageOne()
	{
		SpokeState spokeState = m_parent.GetSpokeState();
		SpokeNumber spokeNumber = m_parent.GetSpokeNumber();

		if(spokeState == SpokeState.CURVED)
		{
			switch(spokeNumber)
			{
				case (SpokeNumber.FOUR_SPOKES):
					return (m_interactableType == PressInteractableType.BOLT_6);
				case (SpokeNumber.EIGHT_SPOKES):
					return (m_interactableType == PressInteractableType.BOLT_5);
				case (SpokeNumber.SIXTEEN_SPOKES):
					return (m_interactableType == PressInteractableType.BOLT_4);
			}
		}
		else if(spokeState == SpokeState.STRAIGHT)
		{
			switch (spokeNumber)
			{
				case (SpokeNumber.FOUR_SPOKES):
					return (m_interactableType == PressInteractableType.BOLT_2);
				case (SpokeNumber.EIGHT_SPOKES):
					return (m_interactableType == PressInteractableType.BOLT_1);
				case (SpokeNumber.SIXTEEN_SPOKES):
					return (m_interactableType == PressInteractableType.BOLT_3);
			}
		}
		return false;
	}

	/// <summary>
	/// Code to be called on stage 2 of fixing the machine
	/// </summary>
	/// <returns>Whether or not this was the correct interactable</returns>
	private bool StageTwo()
	{
		int ySplitsLeaking = 0;
		int singlesLeaking = 0;

		if (m_parent.GetSingle1Leaking())
			singlesLeaking++;
		if (m_parent.GetSingle2Leaking())
			singlesLeaking++;
		if (m_parent.GetYSplit1Leaking())
			ySplitsLeaking++;
		if (m_parent.GetYSplit2Leaking())
			ySplitsLeaking++;

		switch(ySplitsLeaking)
		{
			case (0):
				switch(singlesLeaking)
				{
					case (1): //S
						if (m_interactableType == PressInteractableType.YELLOW_LEVER)
							return true;
						else
							return false;
					case (2): //SS
						if (m_interactableType == PressInteractableType.GREEN_LEVER)
							return true;
						else
							return false;
				}
				break;
			case (1):
				switch(singlesLeaking)
				{
					case (0): //Y
						if (m_interactableType == PressInteractableType.RED_LEVER)
							return true;
						else
							return false;
					case (1): //YS
						if (m_interactableType == PressInteractableType.BLUE_LEVER)
							return true;
						else
							return false;
					case (2): //YSS
						if (m_interactableType == PressInteractableType.RED_LEVER)
							return true;
						else
							return false;
				}
				break;
			case (2):
				switch(singlesLeaking)
				{
					case (0): //YY
						if (m_interactableType == PressInteractableType.GREEN_LEVER)
							return true;
						else
							return false;
					case (1): //YYS
						if (m_interactableType == PressInteractableType.YELLOW_LEVER)
							return true;
						else
							return false;
					case (2): //YYSS
						if (m_interactableType == PressInteractableType.BLUE_LEVER)
							return true;
						else
							return false;
				}
				break;
		}

		Debug.LogError("This error should never be run, if it is run, pray");
		return false;
	}

	/// <summary>
	/// Code to be called on stage 3 of fixing the machine
	/// </summary>
	/// <returns>Whether or not this was the correct interactable</returns>
	private bool StageThree()
	{
		PressLeakLiquid liquid = m_parent.GetLeakLiquid();
		PressLeakVelocity velocity = m_parent.GetLeakVelocity();

		if (m_interactableType == PressInteractableType.RESET_BUTTON && liquid == PressLeakLiquid.INK && velocity == PressLeakVelocity.HIGH && m_holdTimer < m_pressThreshold)
			return true;
		if (m_interactableType == PressInteractableType.RESET_BUTTON && liquid == PressLeakLiquid.INK && velocity == PressLeakVelocity.LOW && m_holdTimer >= m_pressThreshold)
			return true;
		if (m_interactableType == PressInteractableType.RESET_BUTTON && liquid == PressLeakLiquid.WATER && velocity == PressLeakVelocity.HIGH && m_holdTimer >= m_pressThreshold)
			return true;
		if (m_interactableType == PressInteractableType.RESET_BUTTON && liquid == PressLeakLiquid.WATER && velocity == PressLeakVelocity.LOW && m_holdTimer < m_pressThreshold)
			return true;
		else
			return false;
	}
}
