using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Painter Variables

enum PainterLight
{
	RED,		//RRR	#FF0000
	ROSE,		//RRB	#FF007F
	VIOLET,		//RBB	#7F00FF 
	BLUE,		//BBB	#0000FF
	AZURE,		//BBG	#007FFF
	SPRING,		//BGG	#00FF7F
	GREEN,		//GGG	#00FF00
	CHARTREUSE, //GGR	#7FFF00
	ORANGE,		//GRR	#FF7F00
	GREY		//RGB	#7F7F7F
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
	[Tooltip("The red colour")]
	[SerializeField] private Color m_red = new Color(1.0f, 0.0f, 0.0f);
	[Tooltip("The rose colour")]
	[SerializeField] private Color m_rose = new Color(1.0f, 0.0f, 0.5f);
	[Tooltip("The violet colour")]
	[SerializeField] private Color m_violet = new Color(0.5f, 0.0f, 1.0f); //violet is a nice name
	[Tooltip("The blue colour")]
	[SerializeField] private Color m_blue = new Color(0.0f, 0.0f, 1.0f);
	[Tooltip("The azure colour")]
	[SerializeField] private Color m_azure = new Color(0.0f, 0.5f, 1.0f);
	[Tooltip("The spring colour")]
	[SerializeField] private Color m_spring = new Color(0.0f, 1.0f, 0.5f);
	[Tooltip("The green colour")]
	[SerializeField] private Color m_green = new Color(0.0f, 1.0f, 0.0f);
	[Tooltip("The chartreuse colour")]
	[SerializeField] private Color m_chartreuse = new Color(0.5f, 1.0f, 0.0f); //wish there was a more commonly known alternative name for this colour
	[Tooltip("The orange colour")]
	[SerializeField] private Color m_orange = new Color(1.0f, 0.5f, 0.0f);
	[Tooltip("The grey colour")]
	[SerializeField] private Color m_grey = new Color(0.5f, 0.5f, 0.5f);

	#endregion

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

		//TODO create variables

		//TODO create interactables

		//sets manager
		m_machineManager = manager;

		//sets generated state
		m_isGenerated = true;
	}
}
