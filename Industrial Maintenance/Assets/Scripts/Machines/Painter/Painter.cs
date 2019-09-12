using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Painter Variables

public enum PainterLight
{
	RED,		//RRR	#FF0000
	ROSE,		//RRB	#FF007F
	VIOLET,		//RBB	#7F00FF 
	BLUE,		//BBB	#0000FF
	AZURE,		//BBG	#007FFF
	SPRING,		//BGG	#00FF7F
	GREEN,		//GGG	#00FF00
	CHARTREUSE,	//GGR	#7FFF00
	ORANGE,		//GRR	#FF7F00
	GREY,		//RGB	#7F7F7F
	NULL		//Should not be called unless via error
}

public enum PainterPresses
{
	RED,
	GREEN,
	BLUE,
	EMPTY
}

#endregion //Painter Variables

public class Painter : BaseMachine
{
	//variable for if machine is generated
	private bool m_isGenerated;

	#region Light Variables

	//the current colour of the light
	private PainterLight m_lightColour;
	[Header("Light Colour variables")]
	[Tooltip("The three different lights")]
	[SerializeField] private PainterLightDisplay[] m_lights = new PainterLightDisplay[4];
	[Tooltip("The red colour")]
	[SerializeField] private Color m_red = new Color(1.0f, 0.0f, 0.0f);
	[Tooltip("The rose colour")]
	[SerializeField] private Color m_rose = new Color(1.0f, 0.0f, 0.5f);
	[Tooltip("The violet colour")]
	[SerializeField] private Color m_violet = new Color(0.5f, 0.0f, 1.0f);
	[Tooltip("The blue colour")]
	[SerializeField] private Color m_blue = new Color(0.0f, 0.0f, 1.0f);
	[Tooltip("The azure colour")]
	[SerializeField] private Color m_azure = new Color(0.0f, 0.5f, 1.0f);
	[Tooltip("The spring colour")]
	[SerializeField] private Color m_spring = new Color(0.0f, 1.0f, 0.5f);
	[Tooltip("The green colour")]
	[SerializeField] private Color m_green = new Color(0.0f, 1.0f, 0.0f);
	[Tooltip("The chartreuse colour")]
	[SerializeField] private Color m_chartreuse = new Color(0.5f, 1.0f, 0.0f);
	[Tooltip("The orange colour")]
	[SerializeField] private Color m_orange = new Color(1.0f, 0.5f, 0.0f);
	[Tooltip("The grey colour")]
	[SerializeField] private Color m_grey = new Color(0.5f, 0.5f, 0.5f);
	[Tooltip("The light colour when the machine is fixed")]
	[SerializeField] private Color m_disabled = new Color(0.0f, 0.0f, 0.0f);
	/// <summary>
	/// Gets the disabled colour
	/// </summary>
	/// <returns>The disabled colour</returns>
	public Color GetDisabledColour() { return m_disabled; }

	/// <summary>
	/// Gets the current light colour
	/// </summary>
	/// <returns>The light colour</returns>
	public PainterLight GetLight() { return m_lightColour; }

	/// <summary>
	/// Sets the light colour to the current colour
	/// </summary>
	/// <param name="puzzleStage">What puzzle stage light should be enabled, should be 0, 1, 2, or 3</param>
	private void EnableLight(int puzzleStage)
	{
		if(puzzleStage >= 4 || puzzleStage < 0) //if puzzle stage isn't 0, 1, 2, or 3, throw an error
		{
			Debug.LogError("Invalid puzzleStage on Painter.EnableLight()!");
			return;
		}
		switch(m_lightColour) //set light colour of chosen light
		{
			case(PainterLight.RED):
				m_lights[puzzleStage].EnableLight(m_red);
				break;
			case(PainterLight.ROSE):
				m_lights[puzzleStage].EnableLight(m_rose);
				break;
			case(PainterLight.VIOLET):
				m_lights[puzzleStage].EnableLight(m_violet);
				break;
			case(PainterLight.BLUE):
				m_lights[puzzleStage].EnableLight(m_blue);
				break;
			case(PainterLight.AZURE):
				m_lights[puzzleStage].EnableLight(m_azure);
				break;
			case(PainterLight.SPRING):
				m_lights[puzzleStage].EnableLight(m_spring);
				break;
			case(PainterLight.GREEN):
				m_lights[puzzleStage].EnableLight(m_green);
				break;
			case(PainterLight.CHARTREUSE):
				m_lights[puzzleStage].EnableLight(m_chartreuse);
				break;
			case(PainterLight.ORANGE):
				m_lights[puzzleStage].EnableLight(m_orange);
				break;
			case(PainterLight.GREY):
				m_lights[puzzleStage].EnableLight(m_grey);
				break;
			default:
				Debug.LogError("Invalid light colour on Painter!");
				break;
		}
	}

	/// <summary>
	/// Sets the light colour to disabled
	/// </summary>
	/// <param name="puzzleStage">What stage light colour should be disabled, should be 0, 1, 2, or 3</param>
	private void DisableLight(int puzzleStage)
	{
		if (puzzleStage >= 4 || puzzleStage < 0)
		{
			Debug.LogError("Invalid puzzleStage on Painter.DisableLight()!");
			return;
		}
		m_lights[puzzleStage].DisableLight();
	}

	/// <summary>
	/// Generates a new light colour with randomness weighted against a repeat colour
	/// </summary>
	private void GenerateLight()
	{
		//check the previous light colour
		PainterLight previousLight = m_lightColour;

		//generate a new light colour
		PainterLight newLight = (PainterLight)Random.Range(0, 10);
		if (newLight == previousLight) //if new colour is the same as the previous one, generate again
			newLight = (PainterLight)Random.Range(0, 10);

		//set light colour
		m_lightColour = newLight;
	}

	/// <summary>
	/// Randomly sets a light colour
	/// </summary>
	private void InitLight()
	{
		m_lightColour = (PainterLight)Random.Range(0, 10);
		foreach(PainterLightDisplay light in m_lights)
		{
			light.Create(this);
		}
	}

	#endregion //Light Variables

	#region Painter Interactables

	[Header("Interactables")]
	[Tooltip("The red button")]
	[SerializeField] private PainterInteractable m_redButton;
	[Tooltip("The green button")]
	[SerializeField] private PainterInteractable m_greenButton;
	[Tooltip("The blue button")]
	[SerializeField] private PainterInteractable m_blueButton;

	/// <summary>
	/// Creates the interactables
	/// </summary>
	private void CreateInteractables()
	{
		m_redButton.Create(this, PainterInteractableType.RED_BUTTON);
		m_greenButton.Create(this, PainterInteractableType.GREEN_BUTTON);
		m_blueButton.Create(this, PainterInteractableType.BLUE_BUTTON);
	}

	#endregion //Painter Interactables

	#region Puzzle Variables

	//how many different puzzles are left to do
	private int m_puzzlesLeft;

	//The presses currently put into the machine
	private PainterPresses[] m_presses = new PainterPresses[3] { PainterPresses.EMPTY, PainterPresses.EMPTY, PainterPresses.EMPTY};

	//How many presses have been put into the machine?
	private int m_pressCount;

	/// <summary>
	/// Inputs a button press into the painter
	/// </summary>
	/// <param name="press">The button that was pressed</param>
	public void InputPress(PainterInteractableType press)
	{
		//check what button was pressed
		switch(press)
		{
			case (PainterInteractableType.RED_BUTTON):
				m_presses[m_pressCount] = PainterPresses.RED;
				break;
			case (PainterInteractableType.GREEN_BUTTON):
				m_presses[m_pressCount] = PainterPresses.GREEN;
				break;
			case (PainterInteractableType.BLUE_BUTTON):
				m_presses[m_pressCount] = PainterPresses.BLUE;
				break;
			default:
				Debug.LogError("Invalid interactable type on painter!");
				break;
		}

		//increment press count
		m_pressCount++;

		//if three presses, calculate the colour
		if(m_pressCount == 3)
		{
			//calculate the colour
			PainterLight calculatedColour = CalculateColour();

			//TODO compare colour to light colour
			if (calculatedColour == m_lightColour)
				CompletePuzzle();
			else
				FailPuzzle();

			//Reset the presses
			ResetPresses();
		}
	}

	/// <summary>
	/// Called if a puzzle stage was completed
	/// </summary>
	private void CompletePuzzle()
	{
		Debug.Log("Puzzle stage was completed on painter!\n"
			+ "Light was: " + m_lightColour.ToString());

		//decrement puzzles left
		m_puzzlesLeft--;

		//Disables the previous light
		DisableLight(m_puzzlesLeft);

		//if no more puzzles left, fix machine
		if(m_puzzlesLeft <= 0)
			FixMachine();
		else //otherwise, generate a new light colour and enable the next light
		{
			GenerateLight();
			EnableLight(m_puzzlesLeft - 1);
		}
	}

	/// <summary>
	/// Called if a puzzle stage was failed
	/// </summary>
	private void FailPuzzle()
	{
		//subtract time remaining
		SubtractTime(m_incorrectTimeSubtraction);

		Debug.Log("Puzzle stage was failed on painter!\n"
			+ "Light is: " + m_lightColour.ToString());
	}

	/// <summary>
	/// Resets press counts
	/// </summary>
	private void ResetPresses()
	{
		//set press count to zero
		m_pressCount = 0; 

		//set presses to empty
		for (int i = 0; i < m_presses.Length; i++)
		{
			m_presses[i] = PainterPresses.EMPTY;
		}
	}

	/// <summary>
	/// WARNING BIG FUNCTION, 
	/// Calculates the light colour based on the three presses, 
	/// should only be called if press count adds up to 3
	/// </summary>
	/// <returns>The resulting light colour</returns>
	private PainterLight CalculateColour()
	{
		//the count of each press
		int redPresses = 0, greenPresses = 0, bluePresses = 0;

		//add up presses
		for(int i = 0; i < m_presses.Length; i++)
		{
			if(m_presses[i] == PainterPresses.EMPTY) //check if array isn't full yet
			{
				Debug.LogWarning("CalculateColour() should only be used when m_presses is full!");
				return PainterLight.NULL;
			}

			switch (m_presses[i]) //add relevant press to the count
			{
				case (PainterPresses.RED):
					redPresses++;
					continue;
				case (PainterPresses.GREEN):
					greenPresses++;
					continue;
				case (PainterPresses.BLUE):
					bluePresses++;
					continue;
				default:
					Debug.LogWarning("Invalid colour on Painter.CalculateColour()!");
					return PainterLight.NULL;
			}
		}

		//if press count does not reach 3, throw error
		if(redPresses + greenPresses + bluePresses != 3)
		{
			Debug.LogWarning("Invalid press count on Painter.CalculateColour()!");
			return PainterLight.NULL;
		}

		#region Calculation of colour

		if (redPresses == 3) //if three red presses, colour is red
			return PainterLight.RED;
		else if (redPresses == 2) //if two red presses, colour is either orange or rose
		{
			if (greenPresses == 1) //if one green press, colour is orange
				return PainterLight.ORANGE;
			else if (bluePresses == 1) //if one blue press, colour is rose
				return PainterLight.ROSE;
		}
		else if (redPresses == 1) //if one red press, colour is either violet, chartreuse, or grey
		{
			if (greenPresses == 2) //if also two green presses, colour is chartreuse
				return PainterLight.CHARTREUSE;
			else if (bluePresses == 2) //if also two blue presses, colour is violet
				return PainterLight.VIOLET;
			else if (greenPresses == 1 && bluePresses == 1) //if one of each, colour is grey
				return PainterLight.GREY;
		}
		else if (greenPresses == 3) //if three green presses, colour is green
			return PainterLight.GREEN;
		else if (greenPresses == 2 && bluePresses == 1) //if two green presses and one blue press, colour is spring
			return PainterLight.SPRING;
		else if (greenPresses == 1 && bluePresses == 2) //if one green press and two blue presses, colour is azure
			return PainterLight.AZURE;
		else if (bluePresses == 3) //if three blue presses, colour is blue
			return PainterLight.BLUE;

		//if no other possibilities are chosen, something wrong has happened
		Debug.LogError("Presses do not add up to 3 total on Painter.CalculateColour()!\n" +
			"Counts: \n"
			+ "Red: " + redPresses.ToString()
			+ "\nGreen: " + greenPresses.ToString()
			+ "\nBlue: " + bluePresses.ToString());
		return PainterLight.NULL;

		#endregion //Calculation of colour
	}

	#endregion //Puzzle Variables

	[Header("Misc. Variables")]
	[Tooltip("How much time is lost when you press the wrong button")]
	[SerializeField] private float m_incorrectTimeSubtraction;

	/// <summary>
	/// Initially generates the variables the painter might have
	/// </summary>
	/// <param name="manager">The machine manager</param>
	public override void GenerateVariables(MachineManager manager)
	{
		if (m_isGenerated) //if already generated, don't regenerate
			return;

		//Generates a light colour
		InitLight();

		//creates interactables and sets variables on them
		CreateInteractables();

		//sets manager
		m_machineManager = manager;

		//sets generated state
		m_isGenerated = true;
	}

	/// <summary>
	/// Calls for the machine to be broken
	/// </summary>
	/// <param name="issue">The issue the machine broke with</param>
	public override void BreakMachine(MachineIssue issue)
	{
		//call base
		base.BreakMachine(issue);

		//set puzzles left to 4
		m_puzzlesLeft = 4;

		//generates a light colour
		GenerateLight();

		//enable light
		EnableLight(3);
	}

	/// <summary>
	/// Calls for the machine to return to a working state
	/// </summary>
	public override void FixMachine()
	{
		//call base
		base.FixMachine();
	}
}
