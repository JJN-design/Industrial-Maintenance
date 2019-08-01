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
	private bool m_isGenerated = false;

	//Variables for the machine
	private BladeOrientation m_bladeOrientation;
	private InputWords m_inputWords;
	private WarrantyLabel m_warrantyLabel;
	private ConveyorColour m_conveyorColour;
	private LightColour m_lightColour;

	public LightColour GetLightColour()
	{
		return m_lightColour;
	}

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

	/// <summary>
	/// Generates the variables the woodchipper may have
	/// </summary>
	override public void GenerateVariables()
	{
		if (m_isGenerated) //if the machine's already had variables generated, don't do it again
			return;

		m_smallMagentaButton.Create(this, ButtonType.SMALL_MAGENTA);
		m_smallYellowButton.Create(this, ButtonType.SMALL_YELLOW);
		m_smallBlackButton.Create(this, ButtonType.SMALL_BLACK);
		m_smallCyanButton.Create(this, ButtonType.SMALL_CYAN);
		m_bigRedButton.Create(this, ButtonType.BIG_RED);

		m_bladeOrientation = (BladeOrientation)Random.Range(0, 2);
		m_inputWords = (InputWords)Random.Range(0, 3);
		m_warrantyLabel = (WarrantyLabel)Random.Range(0, 2);
		m_conveyorColour = (ConveyorColour)Random.Range(0, 4);
		m_lightColour = (LightColour)Random.Range(0, 3);

		//confirm that this machine has been generated
		m_isGenerated = true;
	}

	public override void BreakMachine(MachineIssue issue)
	{
		base.BreakMachine(issue);

		if (m_inputWords == InputWords.TIMBER && m_conveyorColour == ConveyorColour.BLUE)
			m_smallYellowButton.SetCorrect(true, false);
		else if (m_bladeOrientation == BladeOrientation.HORIZONTAL && m_inputWords == InputWords.WOOD)
			m_bigRedButton.SetCorrect(true, false);
		else if (m_conveyorColour == ConveyorColour.WHITE && m_warrantyLabel == WarrantyLabel.MISSING)
			m_smallCyanButton.SetCorrect(true, false);
		else if (m_warrantyLabel == WarrantyLabel.EXPIRED && m_bladeOrientation == BladeOrientation.VERTICAL)
			m_bigRedButton.SetCorrect(true, false);
		else if (m_conveyorColour == ConveyorColour.YELLOW)
			m_smallMagentaButton.SetCorrect(true, false);
		else if (m_conveyorColour == ConveyorColour.RED && m_inputWords == InputWords.LUMBER)
			m_bigRedButton.SetCorrect(true, false);
		else
			m_smallBlackButton.SetCorrect(true, false);
	}

	public void SetNewButton(ButtonType button, bool secondStage)
	{
		switch(button)
		{
			case (ButtonType.BIG_RED):
				m_bigRedButton.SetCorrect(true, secondStage);
				break;
			case (ButtonType.SMALL_BLACK):
				m_smallBlackButton.SetCorrect(true, secondStage);
				break;
			case (ButtonType.SMALL_CYAN):
				m_smallCyanButton.SetCorrect(true, secondStage);
				break;
			case (ButtonType.SMALL_MAGENTA):
				m_smallMagentaButton.SetCorrect(true, secondStage);
				break;
			case (ButtonType.SMALL_YELLOW):
				m_smallYellowButton.SetCorrect(true, secondStage);
				break;
			default:
				Debug.LogError("Invalid button type on woodchipper");
				break;
		}
	}
}
