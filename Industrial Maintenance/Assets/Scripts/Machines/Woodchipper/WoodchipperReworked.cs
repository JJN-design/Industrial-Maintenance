using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Woodchipper Variables
public enum AxleOrientation
{
    VERTICAL,
    HORIZONTAL
}

public enum BladeSpinDirection
{
    INCORRECT,
    CORRECT
}

public enum RattlingPipe
{
    GREEN,
    YELLOW,
    BLUE,
    RED
}

public enum RotationRate
{
    RPM300,
    RPM400,
    RPM500,
    RPM600,
    RPM700
}

public enum PressureGauge
{
    PSI30,
    PSI45,
    PSI60,
    PSI75,
    PSI90
}
#endregion //Woodchipper Variables

public class WoodchipperReworked : BaseMachine
{
	//Whether or not this machine's variables have been generated
	private bool m_isGenerated = false;

	#region Axle Orientation

	private AxleOrientation m_axleOrientation; //the current axle orientation
	[Header("Axle Orientations")]
	[Tooltip("The assembly of the blades")]
	[SerializeField] private GameObject m_bladeAssembly;
	[Tooltip("The rotation of the blade assembly in vertical configuration")]
	[SerializeField] private Vector3 m_verticalRotation;
	[Tooltip("The rotation of the blade assembly in horizontal configuration")]
	[SerializeField] private Vector3 m_horizontalRotation;
	
	/// <summary>
	/// Gets the axle orientation of the woodchipper
	/// </summary>
	/// <returns>The current axle orientation</returns>
	public AxleOrientation GetAxleOrientation() { return m_axleOrientation; }
	
	/// <summary>
	/// Randomly sets a blade orientation and sets the correct rotation
	/// </summary>
	private void CreateAxle()
	{
		//Randomly sets a configuration for the axle
		m_axleOrientation = (AxleOrientation)Random.Range(0, 2);
		switch(m_axleOrientation)
		{
			case (AxleOrientation.VERTICAL):
				Quaternion verticalRotation = Quaternion.Euler(m_verticalRotation);
				m_bladeAssembly.transform.localRotation = verticalRotation;
				break;
			case (AxleOrientation.HORIZONTAL):
				Quaternion horizontalRotation = Quaternion.Euler(m_horizontalRotation);
				m_bladeAssembly.transform.localRotation = horizontalRotation;
				break;
			default:
				Debug.LogError("Invalid AxleOrientation randomised on CreateAxle()" +
					"");
				break;
		}
	}

	/// <summary>
	/// Sets a new axle orientation
	/// </summary>
	/// <param name="orientation">The new orientation</param>
	private void UpdateAxle(AxleOrientation orientation)
	{
		m_axleOrientation = orientation;
		switch (m_axleOrientation)
		{
			case (AxleOrientation.VERTICAL):
				Quaternion verticalRotation = Quaternion.Euler(m_verticalRotation);
				m_bladeAssembly.transform.localRotation = verticalRotation;
				break;
			case (AxleOrientation.HORIZONTAL):
				Quaternion horizontalRotation = Quaternion.Euler(m_horizontalRotation);
				m_bladeAssembly.transform.localRotation = horizontalRotation;
				break;
			default:
				Debug.LogError("Invalid AxleOrientation passed through on UpdateAxle()");
				break;
		}
	}
	#endregion //Axle Orientation

	#region Blade Spin Direction

	private BladeSpinDirection m_bladeSpinDirection;
	[Header("Blade Spin variables")]
	[Tooltip("The left (in horizontal orientation) blade")]
	[SerializeField] private SpinningBlades m_leftBlades;
	[Tooltip("The right (in horizontal orientation) blade")]
	[SerializeField] private SpinningBlades m_rightBlades;
	[Tooltip("How fast the blades should spin")]
	[SerializeField] private float m_bladeSpinSpeed;
	[Tooltip("The multiplier for blade spin speed when")]
	[SerializeField] private float m_brokenBladeMultiplier;

	/// <summary>
	/// Gets the current spin direction of the blades
	/// </summary>
	/// <returns></returns>
	public BladeSpinDirection GetSpinDirection() { return m_bladeSpinDirection; }

	/// <summary>
	/// Sets initial variables on the blades
	/// </summary>
	private void CreateBlades()
	{
		m_bladeSpinDirection = (BladeSpinDirection)Random.Range(0, 2);

		m_leftBlades.Create(m_bladeSpinDirection, m_bladeSpinSpeed, m_brokenBladeMultiplier);
		m_rightBlades.Create(m_bladeSpinDirection, -m_bladeSpinSpeed, m_brokenBladeMultiplier);
	}

	/// <summary>
	/// Updates the blade's spin direction
	/// </summary>
	/// <param name="newSpin">The new spin direction</param>
	private void UpdateBlades(BladeSpinDirection newSpin)
	{
		m_bladeSpinDirection = newSpin;

		m_leftBlades.UpdateSpinDirection(newSpin);
		m_rightBlades.UpdateSpinDirection(newSpin);
	}

	#endregion //Blade Spin Direction

	#region Pipe Rattling

	private RattlingPipe m_rattlingPipe;
	private bool m_isPipeRattling = false;
	[Header("Pipe Rattling variables")]
	[Tooltip("The green pipe")]
	[SerializeField] private RattlingPipes m_greenPipe;
	[Tooltip("The yellow pipe")]
	[SerializeField] private RattlingPipes m_yellowPipe;
	[Tooltip("The blue pipe")]
	[SerializeField] private RattlingPipes m_bluePipe;
	[Tooltip("The red pipe")]
	[SerializeField] private RattlingPipes m_redPipe;
	[Tooltip("How fast pipes should shake in the 3 axis")]
	[SerializeField] private Vector3 m_pipeShakeSpeed;
	[Tooltip("How much pipes should shake")]
	[SerializeField] private float m_pipeShakeIntensity;

	/// <summary>
	/// Gets which pipe is rattling
	/// </summary>
	/// <returns>The rattling pipe</returns>
	public RattlingPipe GetRattlingPipe() { return m_rattlingPipe; }

	/// <summary>
	/// Sets initial variables for the rattling pipe
	/// </summary>
	private void CreateRattlingPipe()
	{
		m_rattlingPipe = (RattlingPipe)Random.Range(0, 4);
	}

	/// <summary>
	/// Change which pipe is rattling
	/// </summary>
	/// <param name="rattle">The new pipe to rattle</param>
	private void UpdateRattlingPipe(RattlingPipe rattle)
	{
		if (rattle == m_rattlingPipe) //if the pipe isn't being changed, don't bother with rest of code
			return;
		if (m_isPipeRattling) //if a pipe is currently rattling, stop rattling the old pipe and start rattling the new one
		{
			switch (m_rattlingPipe) //stop rattling old pipe
			{
				case (RattlingPipe.GREEN):
					m_greenPipe.StopRattling();
					break;
				case (RattlingPipe.YELLOW):
					m_yellowPipe.StopRattling();
					break;
				case (RattlingPipe.BLUE):
					m_bluePipe.StopRattling();
					break;
				case (RattlingPipe.RED):
					m_redPipe.StopRattling();
					break;
				default:
					Debug.LogError("Invalid rattling pipe type on WoodchipperReworked");
					break;
			}
			m_rattlingPipe = rattle; //update rattling pipe
			switch (m_rattlingPipe) //start rattling new pipe
			{
				case (RattlingPipe.GREEN):
					m_greenPipe.StartRattling();
					break;
				case (RattlingPipe.YELLOW):
					m_yellowPipe.StartRattling();
					break;
				case (RattlingPipe.BLUE):
					m_bluePipe.StartRattling();
					break;
				case (RattlingPipe.RED):
					m_redPipe.StartRattling();
					break;
				default:
					Debug.LogError("Invalid rattling pipe type on WoodchipperReworked");
					break;
			}
		}
		else //if no pipe is currently rattling, just update the pipe
			m_rattlingPipe = rattle;
	}

	/// <summary>
	/// Start rattling the currently active pipe
	/// </summary>
	private void StartRattlingPipe()
	{
		m_isPipeRattling = true;
		switch(m_rattlingPipe)
		{
			case (RattlingPipe.GREEN):
				m_greenPipe.StartRattling();
				break;
			case (RattlingPipe.YELLOW):
				m_yellowPipe.StartRattling();
				break;
			case (RattlingPipe.BLUE):
				m_bluePipe.StartRattling();
				break;
			case (RattlingPipe.RED):
				m_redPipe.StartRattling();
				break;
			default:
				Debug.LogError("Invalid rattling pipe type on WoodchipperReworked");
				break;
		}
	}

	/// <summary>
	/// Stop rattling all pipes (just in case)
	/// </summary>
	private void StopRattlingPipe()
	{
		m_isPipeRattling = false;
		m_greenPipe.StopRattling();
		m_yellowPipe.StopRattling();
		m_bluePipe.StopRattling();
		m_redPipe.StopRattling();
	}

	#endregion //Pipe Rattling

	#region Machine RPM

	private RotationRate m_RPM;
	[Header("RPM variables")]
	[Tooltip("The field for RPM values")]
	[SerializeField] Text m_RPMField;
	[Tooltip("The text displayed if RPM is 300")]
	[SerializeField] string m_RPM300Text;
	[Tooltip("The text displayed if RPM is 400")]
	[SerializeField] string m_RPM400Text;
	[Tooltip("The text displayed if RPM is 500")]
	[SerializeField] string m_RPM500Text;
	[Tooltip("The text displayed if RPM is 600")]
	[SerializeField] string m_RPM600Text;
	[Tooltip("The text displayed if RPM is 700")]
	[SerializeField] string m_RPM700Text;

	/// <summary>
	/// Gets the rotation rate of the woodchipper
	/// </summary>
	/// <returns>The current rotation rate</returns>
	public RotationRate GetRotationRate() { return m_RPM; }

	/// <summary>
	/// Sets up variables for rotation rate
	/// </summary>
	private void CreateRotationRate()
	{
		m_RPM = (RotationRate)Random.Range(0, 5);
	}

	#endregion //Machine RPM

	/// <summary>
	/// Initially generates the variables the woodchipper might have
	/// </summary>
	/// <param name="manager">The machine manager</param>
	public override void GenerateVariables(MachineManager manager)
	{
		if (m_isGenerated) //if machine is already generated, don't redo it
			return;

		//Create the variables
		CreateAxle();
		CreateBlades();
		CreateRattlingPipe();

		//sets manager
		m_machineManager = manager;
	}
}
