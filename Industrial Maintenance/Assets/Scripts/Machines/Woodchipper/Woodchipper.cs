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
enum LightColour
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

	/// <summary>
	/// Generates the variables the woodchipper may have
	/// </summary>
	override public void GenerateVariables()
	{
		if (m_isGenerated) //if the machine's already had variables generated, don't do it again
			return;

		m_bladeOrientation = (BladeOrientation)Random.Range(0, 2);
		m_inputWords = (InputWords)Random.Range(0, 3);
		m_warrantyLabel = (WarrantyLabel)Random.Range(0, 2);
		m_conveyorColour = (ConveyorColour)Random.Range(0, 4);
		m_lightColour = (LightColour)Random.Range(0, 3);

		//confirm that this machine has been generated
		m_isGenerated = true;
	}
}
