using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WoodchipperInteractableType
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
    private WoodchipperInteractableType m_interactableType;

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
    public void Create(WoodchipperReworked machine, WoodchipperInteractableType type, float incorrectTime)
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
		
		if(!m_parent.GetWorking()) //only if machine isn't working
		{
			switch (m_currentStage)
			{
				case (0):
					bool stageOneCorrect = StageOne();
					if (stageOneCorrect)
						InteractCorrect();
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
	}

	/// <summary>
	/// Function to be called when the machine is fixed
	/// </summary>
	public void MachineFixed()
	{
		m_parent.SetNewStage(0);
	}

	/// <summary>
	/// Sets a new stage for all machines
	/// </summary>
	/// <param name="stage">The new stage value</param>
	public void SetNewStage(int stage)
	{
		m_currentStage = stage;
	}

	/// <summary>
	/// Code to be called when this isn't the correct interactable
	/// </summary>
	private void InteractFail()
	{
		m_parent.SubtractTime(m_incorrectTimeSubtraction);
		Debug.Log("Incorrect interactable press for Woodchipper rework at stage " + m_currentStage.ToString());
	}

	/// <summary>
	/// Code to be called when this is the correct interactable and is not the last stage
	/// </summary>
	private void InteractCorrect()
	{
		m_parent.SetNewStage(m_currentStage + 1);
		Debug.Log("Correct interactable press for Woodchipper rework at stage " + m_currentStage.ToString());
	}

	/// <summary>
	/// Code for if this machine is in stage 1 of being fixed
	/// </summary>
	/// <returns>Whether this interactable is the correct one to interact with</returns>
	private bool StageOne()
	{
		switch(m_interactableType)
		{
			case (WoodchipperInteractableType.RED_LEVER): //if spin direction is correct and axle orientation is horizontal, or spin direction is incorrect and axle orientation is vertical, this is correct
				if (m_parent.GetSpinDirection() == BladeSpinDirection.CORRECT && m_parent.GetAxleOrientation() == AxleOrientation.HORIZONTAL
					|| m_parent.GetSpinDirection() == BladeSpinDirection.INCORRECT && m_parent.GetAxleOrientation() == AxleOrientation.VERTICAL)
				{
					m_parent.StartRattlingPipe(); //start rattling pipe for stage 2
					return true;
				}
				else
					return false;
				
			case (WoodchipperInteractableType.BLUE_LEVER): //if spin direction is incorrect and axle orientation is horizontal, or spin direction is correct and axle orientation is vertical, this is correct
				if (m_parent.GetSpinDirection() == BladeSpinDirection.INCORRECT && m_parent.GetAxleOrientation() == AxleOrientation.HORIZONTAL
					|| m_parent.GetSpinDirection() == BladeSpinDirection.CORRECT && m_parent.GetAxleOrientation() == AxleOrientation.VERTICAL)
				{
					m_parent.StartRattlingPipe(); //start rattling pipe for stage 2
					return true;
				}
				else
					return false;

			default: //any other interactable is incorrect
				return false;
		}
	}

	/// <summary>
	/// Code for if this machine is in stage 2 of being fixed
	/// </summary>
	/// <returns>Whether this interactable is the correct one to interact with</returns>
	private bool StageTwo()
	{
		switch(m_parent.GetRattlingPipe())
		{
			case (RattlingPipe.BLUE): //if the pipe is blue...
				if (m_parent.GetAxleOrientation() == AxleOrientation.HORIZONTAL && m_interactableType == WoodchipperInteractableType.BLUE_LEVER) //...and axle is horizontal, pull blue lever
					return true;
				else if (m_parent.GetAxleOrientation() == AxleOrientation.VERTICAL && m_interactableType == WoodchipperInteractableType.RED_LEVER)//...and axle is vertical, pull red lever
					return true;
				else
					return false;

			case (RattlingPipe.GREEN): //if the pipe is green...
				if (m_parent.GetAxleOrientation() == AxleOrientation.VERTICAL && m_interactableType == WoodchipperInteractableType.BLUE_LEVER) //...and axle is vertical, pull blue lever
					return true;
				else if (m_parent.GetAxleOrientation() == AxleOrientation.HORIZONTAL && m_interactableType == WoodchipperInteractableType.RED_LEVER) //...and axle is horizontal, pull red lever
					return true;
				else
					return false;

			case (RattlingPipe.RED): //if the pipe is red...
				if (m_parent.GetAxleOrientation() == AxleOrientation.HORIZONTAL && m_interactableType == WoodchipperInteractableType.BLUE_LEVER) //...and axle is horizontal, pull blue lever
					return true;
				else if (m_parent.GetAxleOrientation() == AxleOrientation.VERTICAL && m_interactableType == WoodchipperInteractableType.RED_LEVER) //...and axle is vertical, pull red lever
					return true;
				else
					return false;

			case (RattlingPipe.YELLOW): //if the pipe is yellow...
				if (m_parent.GetAxleOrientation() == AxleOrientation.VERTICAL && m_interactableType == WoodchipperInteractableType.BLUE_LEVER) //...and axle is vertical, pull blue lever
					return true;
				else if (m_parent.GetAxleOrientation() == AxleOrientation.HORIZONTAL && m_interactableType == WoodchipperInteractableType.RED_LEVER) //...and axle is horizontal, pull red lever
					return true;
				else
					return false;

			default:
				Debug.LogError("Invalid rattling pipe on Woodchipper");
				return false;
		}
	}

	/// <summary>
	/// Code for if this machine is in stage 3 of being fixed
	/// </summary>
	/// <returns>Whether this interactable is the correct one to interact with</returns>
	private bool StageThree()
	{
		switch (m_parent.GetRotationRate())
		{
			case (RotationRate.RPM300):
				return Rotation300();
			case (RotationRate.RPM400):
				return Rotation400();
			case (RotationRate.RPM500):
				return Rotation500();
			case (RotationRate.RPM600):
				return Rotation600();
			case (RotationRate.RPM700):
				return Rotation700();
			default:
				Debug.LogError("Invalid RPM rating on Woodchipper");
				return false;
		}
	}

	#region Stage Three functions

	/// <summary>
	/// Code to be called if the rotation rate is 300
	/// </summary>
	/// <returns>Whether or not this is the correct interactable</returns>
	private bool Rotation300()
	{
		switch (m_parent.GetPressure())
		{
			case (PressureGauge.PSI30):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_A)
					return true;
				else
					return false;
			case (PressureGauge.PSI45):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_C)
					return true;
				else
					return false;
			case (PressureGauge.PSI60):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_D)
					return true;
				else
					return false;
			case (PressureGauge.PSI75):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_B)
					return true;
				else
					return false;
			case (PressureGauge.PSI90):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_E)
					return true;
				else
					return false;
			default:
				Debug.LogError("Invalid pressure rating on Woodchipper");
				return false;
		}
	}

	/// <summary>
	/// Code to be called if the rotation rate is 400
	/// </summary>
	/// <returns>Whether or not this is the correct interactable</returns>
	private bool Rotation400()
	{
		switch (m_parent.GetPressure())
		{
			case (PressureGauge.PSI30):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_B)
					return true;
				else
					return false;
			case (PressureGauge.PSI45):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_D)
					return true;
				else
					return false;
			case (PressureGauge.PSI60):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_E)
					return true;
				else
					return false;
			case (PressureGauge.PSI75):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_A)
					return true;
				else
					return false;
			case (PressureGauge.PSI90):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_C)
					return true;
				else
					return false;
			default:
				Debug.LogError("Invalid pressure rating on Woodchipper");
				return false;
		}
	}

	/// <summary>
	/// Code to be called if the rotation rate is 500
	/// </summary>
	/// <returns>Whether or not this is the correct interactable</returns>
	private bool Rotation500()
	{
		switch (m_parent.GetPressure())
		{
			case (PressureGauge.PSI30):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_E)
					return true;
				else
					return false;
			case (PressureGauge.PSI45):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_A)
					return true;
				else
					return false;
			case (PressureGauge.PSI60):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_C)
					return true;
				else
					return false;
			case (PressureGauge.PSI75):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_D)
					return true;
				else
					return false;
			case (PressureGauge.PSI90):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_B)
					return true;
				else
					return false;
			default:
				Debug.LogError("Invalid pressure rating on Woodchipper");
				return false;
		}
	}

	/// <summary>
	/// Code to be called if the rotation rate is 600
	/// </summary>
	/// <returns>Whether or not this is the correct interactable</returns>
	private bool Rotation600()
	{
		switch (m_parent.GetPressure())
		{
			case (PressureGauge.PSI30):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_D)
					return true;
				else
					return false;
			case (PressureGauge.PSI45):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_E)
					return true;
				else
					return false;
			case (PressureGauge.PSI60):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_B)
					return true;
				else
					return false;
			case (PressureGauge.PSI75):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_C)
					return true;
				else
					return false;
			case (PressureGauge.PSI90):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_A)
					return true;
				else
					return false;
			default:
				Debug.LogError("Invalid pressure rating on Woodchipper");
				return false;
		}
	}

	/// <summary>
	/// Code to be called if the rotation rate is 700
	/// </summary>
	/// <returns>Whether or not this is the correct interactable</returns>
	private bool Rotation700()
	{
		switch (m_parent.GetPressure())
		{
			case (PressureGauge.PSI30):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_C)
					return true;
				else
					return false;
			case (PressureGauge.PSI45):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_B)
					return true;
				else
					return false;
			case (PressureGauge.PSI60):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_A)
					return true;
				else
					return false;
			case (PressureGauge.PSI75):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_E)
					return true;
				else
					return false;
			case (PressureGauge.PSI90):
				if (m_interactableType == WoodchipperInteractableType.BUTTON_D)
					return true;
				else
					return false;
			default:
				Debug.LogError("Invalid pressure rating on Woodchipper");
				return false;
		}
	}

	#endregion //Stage Three functions
}