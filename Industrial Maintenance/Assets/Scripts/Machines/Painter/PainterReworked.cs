using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Painter Rework Variables

public enum StageOneLight
{
	RED,		//AF0000
	GREEN,		//007F00
	BLUE		//00007F
}

public enum StageTwoLight
{
	YELLOW,		//FFFF00
	PURPLE,		//7F007F
	CYAN		//00FFFF
}

public enum StageThreeLight
{
	GREY,		//808080
	ORANGE,		//FF8000
	CHARTREUSE,	//80FF00
	SPRING,		//00FF80
	AZURE,		//0080FF
	ROSE,		//FF0080
	VIOLET		//8000FF
}

#endregion //Painter Rework Variables

public class PainterReworked : BaseMachine
{
	#region Light Colours

	[Header("Materials")]
	[Tooltip("The disabled light material")]
	[SerializeField] private Material m_disabled;
	[Tooltip("The enabled light material")]
	[SerializeField] private Material m_enabled;

	[Header("Stage One Light Colours")]
	private StageOneLight m_stageOneLight;
	[Tooltip("The colour for a red light")]
	[SerializeField] private Color m_red = new Color(0.6862f, 0.0f, 0.0f);
	[Tooltip("The colour for a green light")]
	[SerializeField] private Color m_green = new Color(0.0f, 0.5f, 0.0f);
	[Tooltip("The colour for a blue light")]
	[SerializeField] private Color m_blue = new Color(0.0f, 0.0f, 0.5f);

	[Header("Stage Two Light Colours")]
	private StageTwoLight m_stageTwoLight;
	[Tooltip("The colour for a yellow light")]
	[SerializeField] private Color m_yellow = new Color(1.0f, 1.0f, 0.0f);
	[Tooltip("The colour for a purple light")]
	[SerializeField] private Color m_purple = new Color(0.5f, 0.0f, 0.5f);
	[Tooltip("The colour for a cyan light")]
	[SerializeField] private Color m_cyan = new Color(0.0f, 1.0f, 1.0f);

	[Header("Stage Three Light Colours")]
	private StageThreeLight m_stageThreeLight;
	[Tooltip("The colour for a grey light")]
	[SerializeField] private Color m_grey = new Color(0.5f, 0.5f, 0.5f);
	[Tooltip("The colour for an orange light")]
	[SerializeField] private Color m_orange = new Color(1.0f, 0.5f, 0.0f);
	[Tooltip("The colour for a chartreuse light")]
	[SerializeField] private Color m_chartreuse = new Color(0.5f, 1.0f, 0.0f);
	[Tooltip("The colour for a spring light")]
	[SerializeField] private Color m_spring = new Color(0.0f, 1.0f, 0.5f);
	[Tooltip("The colour for a azure light")]
	[SerializeField] private Color m_azure = new Color(0.0f, 0.5f, 1.0f);
	[Tooltip("The colour for a rose light")]
	[SerializeField] private Color m_rose = new Color(1.0f, 0.0f, 0.5f);
	[Tooltip("The colour for a violet light")]
	[SerializeField] private Color m_violet = new Color(0.5f, 0.0f, 1.0f);

	[Header("Lights")]
	[Tooltip("The first stage light")]
	[SerializeField] private PainterLightDisplay m_firstStageLight;
	[Tooltip("The second stage light")]
	[SerializeField] private PainterLightDisplay m_secondStageLight;
	[Tooltip("The third stage light")]
	[SerializeField] private PainterLightDisplay m_thirdStageLight;

	/// <summary>
	/// Creates the light displays
	/// </summary>
	private void CreateLights()
	{
		m_firstStageLight.Create(m_disabled, m_enabled);
		m_secondStageLight.Create(m_disabled, m_enabled);
		m_thirdStageLight.Create(m_disabled, m_enabled);
	}

	/// <summary>
	/// Randomly sets colours for each stage
	/// </summary>
	private void GenerateColours()
	{
		m_stageOneLight = (StageOneLight)Random.Range(0, 3); //randomly set a stage one light

		switch(m_stageOneLight) //set a stage two light based on the first stage
		{
			case (StageOneLight.RED): //if red light, second stage can be either yellow or purple
				m_stageTwoLight = (StageTwoLight)Random.Range(0, 2);
				break;
			case (StageOneLight.GREEN): //if green light, second stage can be either yellow or cyan
				int lightColour = Random.Range(0, 2);
				if (lightColour == 0)
					m_stageTwoLight = StageTwoLight.YELLOW;
				else
					m_stageTwoLight = StageTwoLight.CYAN;
				break;
			case (StageOneLight.BLUE): //if blue light, second stage can be either purple or cyan
				m_stageTwoLight = (StageTwoLight)Random.Range(1, 3);
				break;
		}

		switch(m_stageTwoLight)
		{
			case (StageTwoLight.CYAN): //if cyan light, third stage can be either spring, azure, or grey
				int cyanLight = Random.Range(0, 3);
				switch(cyanLight)
				{
					case (0):
						m_stageThreeLight = StageThreeLight.SPRING;
						break;
					case (1):
						m_stageThreeLight = StageThreeLight.AZURE;
						break;
					case (2):
						m_stageThreeLight = StageThreeLight.GREY;
						break;
				}
				break;
			case (StageTwoLight.YELLOW):
				int yellowLight = Random.Range(0, 3); //if yellow light, third stage can be either orange, chartreuse, or grey
				switch(yellowLight)
				{
					case (0):
						m_stageThreeLight = StageThreeLight.ORANGE;
						break;
					case (1):
						m_stageThreeLight = StageThreeLight.CHARTREUSE;
						break;
					case (2):
						m_stageThreeLight = StageThreeLight.GREY;
						break;
				}
				break;
			case (StageTwoLight.PURPLE):
				int purpleLight = Random.Range(0, 3); //if purple light, third stage can be either rose, violet, or grey
				switch(purpleLight)
				{
					case (0):
						m_stageThreeLight = StageThreeLight.ROSE;
						break;
					case (1):
						m_stageThreeLight = StageThreeLight.VIOLET;
						break;
					case (2):
						m_stageThreeLight = StageThreeLight.GREY;
						break;
				}
				break;
		}
	}

	#endregion //Light Colours

	#region Interactables

	[Header("Interactables")]
	[Tooltip("The red button")]
	[SerializeField] private PainterInteractableReworked m_redButton;
	[Tooltip("The green button")]
	[SerializeField] private PainterInteractableReworked m_greenButton;
	[Tooltip("The blue button")]
	[SerializeField] private PainterInteractableReworked m_blueButton;

	/// <summary>
	/// Creates all the interactables
	/// </summary>
	private void CreateInteractables()
	{
		m_redButton.Create(this, PainterInteractableReworkedType.RED_BUTTON);
		m_greenButton.Create(this, PainterInteractableReworkedType.GREEN_BUTTON);
		m_blueButton.Create(this, PainterInteractableReworkedType.BLUE_BUTTON);
	}

	#endregion //Interactables

	#region Puzzle Variables

	private int m_currentStage = 0;

	/// <summary>
	/// Inputs the chosen button press
	/// </summary>
	/// <param name="press">The button that was pressed</param>
	public void InputPress(PainterInteractableReworkedType press)
	{
		//Only input press if machine is broken
		if(!m_isWorking)
		{
			//Change required button based on current stage
			switch(m_currentStage)
			{
				case (0):
					bool stageOne = StageOne(press);
					if (stageOne)
						CorrectPress();
					else
						IncorrectPress();
					break;
				case (1):
					bool stageTwo = StageTwo(press);
					if (stageTwo)
						CorrectPress();
					else
						IncorrectPress();
					break;
				case (2):
					bool stageThree = StageThree(press);
					if (stageThree)
						CorrectPress();
					else
						IncorrectPress();
					break;
			}
		}
	}

	/// <summary>
	/// Code to be run for Stage One
	/// </summary>
	/// <param name="press">The button that was pressed</param>
	/// <returns>Whether the button pressed was correct</returns>
	private bool StageOne(PainterInteractableReworkedType press)
	{
		switch(m_stageOneLight)
		{
			case (StageOneLight.RED): //if light is red...
				if (press == PainterInteractableReworkedType.RED_BUTTON) //...press red button
					return true;
				else
					return false;
			case (StageOneLight.GREEN): //if light is green...
				if (press == PainterInteractableReworkedType.GREEN_BUTTON) //...press green button
					return true;
				else
					return false;
			case (StageOneLight.BLUE): //if light is blue...
				if (press == PainterInteractableReworkedType.BLUE_BUTTON) //...press blue button
					return true;
				else
					return false;
		}
		return false;
	}

	/// <summary>
	/// Code to be run for Stage Two
	/// </summary>
	/// <param name="press">The button that was pressed</param>
	/// <returns>Whether the button pressed was correct</returns>
	private bool StageTwo(PainterInteractableReworkedType press)
	{
		switch(m_stageTwoLight)
		{
			case (StageTwoLight.CYAN): //if light is cyan...
				if (press == PainterInteractableReworkedType.BLUE_BUTTON && m_stageOneLight == StageOneLight.GREEN) //...and stage one was green, press blue
					return true;
				else if (press == PainterInteractableReworkedType.GREEN_BUTTON && m_stageOneLight == StageOneLight.BLUE) //...and stage one was blue, press green
					return true;
				else
					return false;
			case (StageTwoLight.YELLOW): //if light is yellow...
				if (press == PainterInteractableReworkedType.RED_BUTTON && m_stageOneLight == StageOneLight.GREEN) //...and stage one was green, press red
					return true;
				else if (press == PainterInteractableReworkedType.GREEN_BUTTON && m_stageOneLight == StageOneLight.RED) //...and stage one was red, press green
					return true;
				else
					return false;
			case (StageTwoLight.PURPLE): //if light is purple...
				if (press == PainterInteractableReworkedType.RED_BUTTON && m_stageOneLight == StageOneLight.BLUE) //...and stage one was blue, press red
					return true;
				else if (press == PainterInteractableReworkedType.BLUE_BUTTON && m_stageOneLight == StageOneLight.RED) //...and stage one was red, press blue
					return true;
				else
					return false;
		}
		return false;
	}

	/// <summary>
	/// Code to be run for Stage Three
	/// </summary>
	/// <param name="press">The button that was pressed</param>
	/// <returns>Whether the button pressed was correct</returns>
	private bool StageThree(PainterInteractableReworkedType press)
	{
		switch(m_stageTwoLight)
		{
			case (StageTwoLight.CYAN): //if stage two was cyan...
				if (press == PainterInteractableReworkedType.RED_BUTTON && m_stageThreeLight == StageThreeLight.GREY) //...and light is grey, press red
					return true;
				else if (press == PainterInteractableReworkedType.GREEN_BUTTON && m_stageThreeLight == StageThreeLight.SPRING) //...and light is spring, press green
					return true;
				else if (press == PainterInteractableReworkedType.BLUE_BUTTON && m_stageThreeLight == StageThreeLight.AZURE) //...and light is azure, press blue
					return true;
				else
					return false;
			case (StageTwoLight.YELLOW): //if stage two was yellow...
				if (press == PainterInteractableReworkedType.RED_BUTTON && m_stageThreeLight == StageThreeLight.ORANGE) //...and light is orange, press red
					return true;
				else if (press == PainterInteractableReworkedType.GREEN_BUTTON && m_stageThreeLight == StageThreeLight.CHARTREUSE) //...and light is chartreuse, press green
					return true;
				else if (press == PainterInteractableReworkedType.BLUE_BUTTON && m_stageThreeLight == StageThreeLight.GREY) //...and light is grey, press blue
					return true;
				else
					return false;
			case (StageTwoLight.PURPLE): //if stage two was purple...
				if (press == PainterInteractableReworkedType.RED_BUTTON && m_stageThreeLight == StageThreeLight.ROSE) //...and light is rose, press red
					return true;
				else if (press == PainterInteractableReworkedType.GREEN_BUTTON && m_stageThreeLight == StageThreeLight.GREY) //...and light is grey, press green
					return true;
				else if (press == PainterInteractableReworkedType.BLUE_BUTTON && m_stageThreeLight == StageThreeLight.VIOLET) //...and light is violet, press blue
					return true;
				else
					return false;
		}
		return false;
	}

	/// <summary>
	/// Code to be called if the correct button is pressed
	/// </summary>
	private void CorrectPress()
	{
		//play the correct sound
		m_audioSource.clip = m_stageCompleteAudio;
		m_audioSource.Play();

		//change what is done based on stage
		switch(m_currentStage)
		{
			case (0): //on stage one...
				m_currentStage++; //increment stage
				switch(m_stageTwoLight) //enable stage two light
				{
					case (StageTwoLight.CYAN):
						m_secondStageLight.EnableLight(m_cyan);
						break;
					case (StageTwoLight.YELLOW):
						m_secondStageLight.EnableLight(m_yellow);
						break;
					case (StageTwoLight.PURPLE):
						m_secondStageLight.EnableLight(m_purple);
						break;
				}
				break;
			case (1): //on stage two...
				m_currentStage++; //increment stage
				switch(m_stageThreeLight) //enable stage three light
				{
					case (StageThreeLight.GREY):
						m_thirdStageLight.EnableLight(m_grey);
						break;
					case (StageThreeLight.ORANGE):
						m_thirdStageLight.EnableLight(m_orange);
						break;
					case (StageThreeLight.CHARTREUSE):
						m_thirdStageLight.EnableLight(m_chartreuse);
						break;
					case (StageThreeLight.SPRING):
						m_thirdStageLight.EnableLight(m_spring);
						break;
					case (StageThreeLight.AZURE):
						m_thirdStageLight.EnableLight(m_azure);
						break;
					case (StageThreeLight.ROSE):
						m_thirdStageLight.EnableLight(m_rose);
						break;
					case (StageThreeLight.VIOLET):
						m_thirdStageLight.EnableLight(m_violet);
						break;
				}
				break;
			case (2): //on stage three...
				FixMachine(); //fix machine
				break;
		}
	}

	/// <summary>
	/// Code to be called if an incorrect button is pressed
	/// </summary>
	private void IncorrectPress()
	{
		//subtract time
		SubtractTime(m_incorrectTimeSubtraction);

		//play audio for failed stage
		m_audioSource.clip = m_stageFailedAudio;
		m_audioSource.Play();
	}

	#endregion //Puzzle Variables

	/// <summary>
	/// Generates the variables for this machine
	/// </summary>
	/// <param name="manager"></param>
	public override void GenerateVariables(MachineManager manager)
	{
		//set manager
		m_machineManager = manager;

		//generate variables
		GenerateColours();
		CreateLights();

		//create interactables
		CreateInteractables();
	}

	/// <summary>
	/// Calls for the machine to be broken
	/// </summary>
	public override void BreakMachine()
	{
		//call base
		base.BreakMachine();

		//Activates the first light
		switch(m_stageOneLight)
		{
			case (StageOneLight.RED):
				m_firstStageLight.EnableLight(m_red);
				break;
			case (StageOneLight.GREEN):
				m_firstStageLight.EnableLight(m_green);
				break;
			case (StageOneLight.BLUE):
				m_firstStageLight.EnableLight(m_blue);
				break;
		}
	}

	/// <summary>
	/// Code to be called when the machine is fixed
	/// </summary>
	public override void FixMachine()
	{
		//call base
		base.FixMachine();

		//reset stages left
		m_currentStage = 0;

		//generate a new set of lights
		GenerateColours();

		//disable all lights
		m_firstStageLight.DisableLight();
		m_secondStageLight.DisableLight();
		m_thirdStageLight.DisableLight();
	}
}
