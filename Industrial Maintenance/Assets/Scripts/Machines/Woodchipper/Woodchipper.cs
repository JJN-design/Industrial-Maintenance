using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Woodchipper Variables
//The orientation of the blades of the woodchipper
enum BladeOrientation
{
	VERTICAL,
	HORIZONTAL
}

//The words on the input of the woodchipper
enum InputWords
{
	LUMBER,
	WOOD,
	TIMBER
}

//The state of the warranty label
enum WarrantyLabel
{
	MISSING,
	EXPIRED
}

//What colour the conveyor is
enum ConveyorColour
{
	WHITE,
	YELLOW,
	BLUE,
	RED
}

//What colour the light is
public enum LightColour
{
	PINK,
	ORANGE,
	BLUE
}
#endregion
public class Woodchipper : BaseMachine
{
	//Whether or not the machine's variables have been generated yet
	private bool m_isGenerated = false;

	private BladeOrientation m_bladeOrientation;
	[Header("Blade Orientations")]
	[Tooltip("The vertical blade assembly")]
	[SerializeField] private GameObject m_verticalBlades;
	[Tooltip("The horizontal blade assembly")]
	[SerializeField] private GameObject m_horizontalBlades;

	/// <summary>
	/// Randomly sets a blade orientation and makes the correct assembly visible
	/// </summary>
	private void CreateBlades()
	{
		m_bladeOrientation = (BladeOrientation)Random.Range(0, 2);
		switch (m_bladeOrientation)
		{
			case (BladeOrientation.VERTICAL):
				m_verticalBlades.SetActive(true);
				break;
			case (BladeOrientation.HORIZONTAL):
				m_horizontalBlades.SetActive(true);
				break;
			default:
				Debug.LogError("Blade orientation randomisation went wrong on woodchipper");
				break;
		}
	}

	private InputWords m_inputWords;
	[Header("Input Words")]
	[Tooltip("The 'lumber' input label")]
	[SerializeField] private GameObject m_lumberInput;
	[Tooltip("The 'wood' input label")]
	[SerializeField] private GameObject m_woodInput;
	[Tooltip("The 'timber' input label")]
	[SerializeField] private GameObject m_timberInput;

	/// <summary>
	/// Randomly sets an input word and makes the correct object visible
	/// </summary>
	private void CreateInputWords()
	{
		m_inputWords = (InputWords)Random.Range(0, 3);
		switch (m_inputWords)
		{
			case (InputWords.LUMBER):
				m_lumberInput.SetActive(true);
				break;
			case (InputWords.WOOD):
				m_woodInput.SetActive(true);
				break;
			case (InputWords.TIMBER):
				m_timberInput.SetActive(true);
				break;
			default:
				Debug.LogError("Input word randomisation went wrong on woodchipper");
				break;
		}
	}

	private WarrantyLabel m_warrantyLabel;
	[Header("Warranty Labels")]
	[Tooltip("The missing warranty label")]
	[SerializeField] private GameObject m_missingWarranty;
	[Tooltip("The expired warranty label")]
	[SerializeField] private GameObject m_expiredWarranty;

	/// <summary>
	/// Randomly sets a warranty label and makes the correct label visible
	/// </summary>
	private void CreateWarrantyLabel()
	{
		m_warrantyLabel = (WarrantyLabel)Random.Range(0, 2);
		switch(m_warrantyLabel)
		{
			case (WarrantyLabel.MISSING):
				m_missingWarranty.SetActive(true);
				break;
			case (WarrantyLabel.EXPIRED):
				m_expiredWarranty.SetActive(true);
				break;
			default:
				Debug.LogError("Warranty label randomisation went wrong on woodchipper");
				break;
		}
	}

	private ConveyorColour m_conveyorColour;
	[Header("Conveyor Colours")]
	[Tooltip("The white conveyor")]
	[SerializeField] private GameObject m_whiteConveyor;
	[Tooltip("The yellow conveyor")]
	[SerializeField] private GameObject m_yellowConveyor;
	[Tooltip("The blue conveyor")]
	[SerializeField] private GameObject m_blueConveyor;
	[Tooltip("The red conveyor")]
	[SerializeField] private GameObject m_redConveyor;

	/// <summary>
	/// Randomly sets a conveyor colour and makes the correct conveyor visible
	/// </summary>
	private void CreateConveyor()
	{
		m_conveyorColour = (ConveyorColour)Random.Range(0, 4);
		switch (m_conveyorColour)
		{
			case (ConveyorColour.WHITE):
				m_whiteConveyor.SetActive(true);
				break;
			case (ConveyorColour.YELLOW):
				m_yellowConveyor.SetActive(true);
				break;
			case (ConveyorColour.BLUE):
				m_blueConveyor.SetActive(true);
				break;
			case (ConveyorColour.RED):
				m_redConveyor.SetActive(true);
				break;
			default:
				Debug.LogError("Conveyor randomisation went wrong on woodchipper");
				break;
		}
	}

	private LightColour m_lightColour;
	[Header("Light Colours")]
	[Tooltip("The pink light")]
	[SerializeField] private GameObject m_pinkLight;
	[Tooltip("The orange light")]
	[SerializeField] private GameObject m_orangeLight;
	[Tooltip("The blue light")]
	[SerializeField] private GameObject m_blueLight;

	/// <summary>
	/// Randomly sets a light colour and makes the correct light visible
	/// </summary>
	private void CreateLight()
	{
		m_lightColour = (LightColour)Random.Range(0, 3);
		switch(m_lightColour)
		{
			case (LightColour.PINK):
				m_pinkLight.SetActive(true);
				break;
			case (LightColour.ORANGE):
				m_orangeLight.SetActive(true);
				break;
			case (LightColour.BLUE):
				m_blueLight.SetActive(true);
				break;
			default:
				Debug.LogError("Light colour randomisation went wrong on woodchipper");
				break;
		}
	}

	/// <summary>
	/// Gets the light colour of the machine
	/// </summary>
	/// <returns>The light colour</returns>
	public LightColour GetLightColour() { return m_lightColour; }

	[Header("Interactable objects")]
	[Tooltip("The small magenta button")]
	[SerializeField] private WoodchipperInteractable m_smallMagentaButton;
	[Tooltip("The small yellow button")]
	[SerializeField] private WoodchipperInteractable m_smallYellowButton;
	[Tooltip("The small black button")]
	[SerializeField] private WoodchipperInteractable m_smallBlackButton;
	[Tooltip("The small cyan button")]
	[SerializeField] private WoodchipperInteractable m_smallCyanButton;
	[Tooltip("The big red button")]
	[SerializeField] private WoodchipperInteractable m_bigRedButton;

	[Header("Other variables")]
	[Tooltip("How much time to be subtracted on an incorrect button press in seconds")]
	[SerializeField] private float m_incorrectTime;

	/// <summary>
	/// Generates the variables the woodchipper may have
	/// Also creates the buttons
	/// </summary>
	override public void GenerateVariables(MachineManager manager)
	{
		if (m_isGenerated) //if the machine's already had variables generated, don't do it again
			return;

		//set manager
		m_machineManager = manager;

		//Sets up the buttons for the machine
		m_smallMagentaButton.Create(this, WoodchipperInteractableType.SMALL_MAGENTA, m_incorrectTime);
		m_smallYellowButton.Create(this, WoodchipperInteractableType.SMALL_YELLOW, m_incorrectTime);
		m_smallBlackButton.Create(this, WoodchipperInteractableType.SMALL_BLACK, m_incorrectTime);
		m_smallCyanButton.Create(this, WoodchipperInteractableType.SMALL_CYAN, m_incorrectTime);
		m_bigRedButton.Create(this, WoodchipperInteractableType.BIG_RED, m_incorrectTime);

		//Randomly generates variables for the machine
		CreateBlades();
		CreateInputWords();
		CreateWarrantyLabel();
		CreateConveyor();
		CreateLight();

		//confirm that this machine has been generated
		m_isGenerated = true;
	}

	/// <summary>
	/// Breaks the machine and sets up the correct button to hit
	/// </summary>
	/// <param name="issue">The issue that the machine has</param>
	public override void BreakMachine(MachineIssue issue)
	{
		//Base machine break
		base.BreakMachine(issue);

		//Sets the correct button to press based on the variables the machine may have
		if (m_inputWords == InputWords.TIMBER && m_conveyorColour == ConveyorColour.BLUE) //A blue conveyor with an input labelled 'Timber' means small yellow is correct
			m_smallYellowButton.SetCorrect(true, false);
		else if (m_bladeOrientation == BladeOrientation.HORIZONTAL && m_inputWords == InputWords.WOOD) //A horizontal blade assembly with an input labelled 'Wood' means big red is correct
			m_bigRedButton.SetCorrect(true, false);
		else if (m_conveyorColour == ConveyorColour.WHITE && m_warrantyLabel == WarrantyLabel.MISSING) //A missing warranty with a white conveyor means small cyan is correct
			m_smallCyanButton.SetCorrect(true, false);
		else if (m_warrantyLabel == WarrantyLabel.EXPIRED && m_bladeOrientation == BladeOrientation.VERTICAL) //An expired warranty with a vertical blade assembly means big red is correct
			m_bigRedButton.SetCorrect(true, false);
		else if (m_conveyorColour == ConveyorColour.YELLOW) //A yellow conveyor means small magenta is correct
			m_smallMagentaButton.SetCorrect(true, false);
		else if (m_conveyorColour == ConveyorColour.RED && m_inputWords == InputWords.LUMBER) //A red conveyor with an input labelled 'Lumber' means big red is correct
			m_bigRedButton.SetCorrect(true, false);
		else //If no other conditions are met, small black is correct
			m_smallBlackButton.SetCorrect(true, false);
	}

	/// <summary>
	/// Sets a new button to be correct
	/// </summary>
	/// <param name="button">The type of button to be set correct next</param>
	/// <param name="secondStage">Whether or not this is the second stage of button press</param>
	public void SetNewButton(WoodchipperInteractableType button, bool secondStage)
	{
		switch(button) //Switch for the new button
		{
			case (WoodchipperInteractableType.BIG_RED):
				m_bigRedButton.SetCorrect(true, secondStage);
				break;
			case (WoodchipperInteractableType.SMALL_BLACK):
				m_smallBlackButton.SetCorrect(true, secondStage);
				break;
			case (WoodchipperInteractableType.SMALL_CYAN):
				m_smallCyanButton.SetCorrect(true, secondStage);
				break;
			case (WoodchipperInteractableType.SMALL_MAGENTA):
				m_smallMagentaButton.SetCorrect(true, secondStage);
				break;
			case (WoodchipperInteractableType.SMALL_YELLOW):
				m_smallYellowButton.SetCorrect(true, secondStage);
				break;
			default:
				Debug.LogError("Invalid button type on woodchipper");
				break;
		}
	}
}