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
	[Tooltip("How fast the blades should spin at 500 RPM")]
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

		m_leftBlades.Create(m_bladeSpinDirection, m_bladeSpinSpeed, m_brokenBladeMultiplier, m_RPM);
		m_rightBlades.Create(m_bladeSpinDirection, -m_bladeSpinSpeed, m_brokenBladeMultiplier, m_RPM);
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
		switch (m_rattlingPipe)
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
	/// Sets up variables for rotation rate and sets the text field to reflect the rotation rate
	/// </summary>
	private void CreateRotationRate()
	{
		//randomly select an rpm rating
		m_RPM = (RotationRate)Random.Range(0, 5);

		//set the text field to the proper text
		switch (m_RPM)
		{
			case (RotationRate.RPM300):
				m_RPMField.text = m_RPM300Text;
				break;
			case (RotationRate.RPM400):
				m_RPMField.text = m_RPM400Text;
				break;
			case (RotationRate.RPM500):
				m_RPMField.text = m_RPM500Text;
				break;
			case (RotationRate.RPM600):
				m_RPMField.text = m_RPM600Text;
				break;
			case (RotationRate.RPM700):
				m_RPMField.text = m_RPM700Text;
				break;
			default:
				m_RPMField.text = "INVALID RPM";
				Debug.LogError("Randomisation went wrong for RPM rating on WoodchipperReworked");
				break;
		}
	}

	/// <summary>
	/// Updates the rotation rate and sets the text field to reflect the new rotation rate
	/// </summary>
	/// <param name="newRate"></param>
	private void UpdateRotationRate(RotationRate newRate)
	{
		//set the new rpm rate, and update the spinning blades
		m_RPM = newRate;
		m_leftBlades.UpdateMachineRPM(m_RPM, m_bladeSpinSpeed);
		m_rightBlades.UpdateMachineRPM(m_RPM, -m_bladeSpinSpeed);
		switch (m_RPM) //modify the text field
		{
			case (RotationRate.RPM300):
				m_RPMField.text = m_RPM300Text;
				break;
			case (RotationRate.RPM400):
				m_RPMField.text = m_RPM400Text;
				break;
			case (RotationRate.RPM500):
				m_RPMField.text = m_RPM500Text;
				break;
			case (RotationRate.RPM600):
				m_RPMField.text = m_RPM600Text;
				break;
			case (RotationRate.RPM700):
				m_RPMField.text = m_RPM700Text;
				break;
			default:
				m_RPMField.text = "INVALID RPM";
				Debug.LogError("Invalid RPM rating passed on UpdateRotationRate for WoodchipperReworked");
				break;
		}
	}

	#endregion //Machine RPM

	#region Machine Pressure

	private PressureGauge m_pressure;
	[Header("Pressure Gauge variables")]
	[Tooltip("The GameObject for the 30 PSI reading gauge")]
	[SerializeField] private GameObject m_30PSIGauge;
	[Tooltip("The GameObject for the 45 PSI reading gauge")]
	[SerializeField] private GameObject m_45PSIGauge;
	[Tooltip("The GameObject for the 60 PSI reading gauge")]
	[SerializeField] private GameObject m_60PSIGauge;
	[Tooltip("The GameObject for the 75 PSI reading gauge")]
	[SerializeField] private GameObject m_75PSIGauge;
	[Tooltip("The GameObject for the 90 PSI reading gauge")]
	[SerializeField] private GameObject m_90PSIGauge;

	/// <summary>
	/// Gets the current pressure rating of the machine
	/// </summary>
	/// <returns>The current pressure rating</returns>
	public PressureGauge GetPressure() { return m_pressure; }

	/// <summary>
	/// Randomly sets a pressure rating for the machine, and makes the correct gauge active, while also making the other gauges inactive
	/// </summary>
	private void CreatePressure()
	{
		//Randomly select a pressure rating
		PressureGauge newPressure = (PressureGauge)Random.Range(0, 5);

		//set the pressure rating
		UpdatePressure(newPressure);
	}

	/// <summary>
	/// Sets a new pressure rating, and updates the visible gauge to match
	/// </summary>
	/// <param name="newPressure">The new pressure rating for the machine</param>
	private void UpdatePressure(PressureGauge newPressure)
	{
		//sets the new rating
		m_pressure = newPressure;

		//deactivate all gauges
		m_30PSIGauge.SetActive(false);
		m_45PSIGauge.SetActive(false);
		m_60PSIGauge.SetActive(false);
		m_75PSIGauge.SetActive(false);
		m_90PSIGauge.SetActive(false);

		//activate the correct one
		switch (m_pressure)
		{
			case (PressureGauge.PSI30):
				m_30PSIGauge.SetActive(true);
				break;
			case (PressureGauge.PSI45):
				m_45PSIGauge.SetActive(true);
				break;
			case (PressureGauge.PSI60):
				m_60PSIGauge.SetActive(true);
				break;
			case (PressureGauge.PSI75):
				m_75PSIGauge.SetActive(true);
				break;
			case (PressureGauge.PSI90):
				m_90PSIGauge.SetActive(true);
				break;
		}
	}

	#endregion //Machine Pressure

	#region Interactables

	[Header("Interactables")]
	[Tooltip("The blue lever")]
	[SerializeField] private WoodchipperInteractableReworked m_blueLever;
	[Tooltip("The red lever")]
	[SerializeField] private WoodchipperInteractableReworked m_redLever;
	[Tooltip("The A button")]
	[SerializeField] private WoodchipperInteractableReworked m_buttonA;
	[Tooltip("The B button")]
	[SerializeField] private WoodchipperInteractableReworked m_buttonB;
	[Tooltip("The C button")]
	[SerializeField] private WoodchipperInteractableReworked m_buttonC;
	[Tooltip("The D button")]
	[SerializeField] private WoodchipperInteractableReworked m_buttonD;
	[Tooltip("The E button")]
	[SerializeField] private WoodchipperInteractableReworked m_buttonE;

	/// <summary>
	/// Creates all the interactables
	/// </summary>
	private void CreateInteractables()
	{
		m_blueLever.Create(this, WoodchipperInteractableTypeV2.BLUE_LEVER, m_incorrectTimeSubtraction);
		m_redLever.Create(this, WoodchipperInteractableTypeV2.RED_LEVER, m_incorrectTimeSubtraction);
		m_buttonA.Create(this, WoodchipperInteractableTypeV2.BUTTON_A, m_incorrectTimeSubtraction);
		m_buttonB.Create(this, WoodchipperInteractableTypeV2.BUTTON_B, m_incorrectTimeSubtraction);
		m_buttonC.Create(this, WoodchipperInteractableTypeV2.BUTTON_C, m_incorrectTimeSubtraction);
		m_buttonD.Create(this, WoodchipperInteractableTypeV2.BUTTON_D, m_incorrectTimeSubtraction);
		m_buttonE.Create(this, WoodchipperInteractableTypeV2.BUTTON_E, m_incorrectTimeSubtraction);
	}

	#endregion //Interactables

	[Header("Misc. Variables")]
	[Tooltip("How much time is lost when you press the wrong button")]
	[SerializeField] private float m_incorrectTimeSubtraction;

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
		CreateRattlingPipe();
		CreateRotationRate();
		CreateBlades();
		CreatePressure();

		//sets manager
		m_machineManager = manager;
	}

	/// <summary>
	/// Calls base fix machine, also randomly sets a new value for all the variables
	/// </summary>
	public override void FixMachine()
	{
		//call base function
		base.FixMachine();

		//randomly generate new variables
		AxleOrientation newAxles = (AxleOrientation)Random.Range(0, 2);
		BladeSpinDirection newSpin = (BladeSpinDirection)Random.Range(0, 2);
		RattlingPipe newRattle = (RattlingPipe)Random.Range(0, 4);
		RotationRate newRotateSpeed = (RotationRate)Random.Range(0, 5);
		PressureGauge newPressure = (PressureGauge)Random.Range(0, 5);

		//update all the variables
		UpdateAxle(newAxles);
		UpdateBlades(newSpin);
		UpdateRattlingPipe(newRattle);
		UpdateRotationRate(newRotateSpeed);
		UpdatePressure(newPressure);
	}
}
