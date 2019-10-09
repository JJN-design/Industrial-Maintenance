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

	[Header("Misc Light Colours")]
	[Tooltip("The disabled light colour")]
	[SerializeField] private Color m_disabled = new Color(0.0f, 0.0f, 0.0f);

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

		//TODO activate the first light

	}
}
