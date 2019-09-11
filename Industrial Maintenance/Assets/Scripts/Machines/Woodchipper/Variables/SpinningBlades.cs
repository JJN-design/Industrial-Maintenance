using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBlades : MonoBehaviour
{
	//What direction these blades spin
	private BladeSpinDirection m_spinDirection;

	//How fast these blades should spin normally if 'correct' (multiplied by -1 if spin direction is 'INCORRECT')
	private float m_spinSpeed;

	//How much slower these blades should be if the machine is broken
	private float m_brokenSpeedMultiplier;

	//The current speed at which these blades rotate
	private float m_currentSpinSpeed;

	//The current RPM of the machine
	private RotationRate m_machineRPM;

	/// <summary>
	/// Sets up variables for the spinning blades
	/// </summary>
	/// <param name="spinDirection">What direction these spin</param>
	/// <param name="spinSpeed">How fast these should spin</param>
	/// <param name="brokenMulti">How much slower these should go if machine is broken</param>
	public void Create(BladeSpinDirection spinDirection, float spinSpeed, float brokenMulti, RotationRate rotationRate)
	{
		m_spinDirection = spinDirection;
		m_spinSpeed = spinSpeed;
		m_brokenSpeedMultiplier = brokenMulti;
		m_machineRPM = rotationRate;

		switch (m_machineRPM) //modify the spin speed based on machine RPM
		{
			case (RotationRate.RPM300):
				m_spinSpeed *= 0.6f;
				break;
			case (RotationRate.RPM400):
				m_spinSpeed *= 0.8f;
				break;
			case (RotationRate.RPM500):
				//do nothing, as this is the base speed rating
				break;
			case (RotationRate.RPM600):
				m_spinSpeed *= 1.2f;
				break;
			case (RotationRate.RPM700):
				m_spinSpeed *= 1.4f;
				break;
			default:
				break;
		}

		if (m_spinDirection == BladeSpinDirection.CORRECT)
			m_currentSpinSpeed = m_spinSpeed;
		else if (m_spinDirection == BladeSpinDirection.INCORRECT)
			m_currentSpinSpeed = m_spinSpeed * -1.0f;
	}

	/// <summary>
	/// Function to be called when the machine breaks
	/// </summary>
	public void MachineBreak()
	{
		if (m_spinDirection == BladeSpinDirection.CORRECT)
			m_currentSpinSpeed = m_spinSpeed * m_brokenSpeedMultiplier;
		else if (m_spinDirection == BladeSpinDirection.INCORRECT)
			m_currentSpinSpeed = m_spinSpeed * -1.0f * m_brokenSpeedMultiplier;
	}

	/// <summary>
	/// Function to be called when the machine is fixed
	/// </summary>
	public void MachineFix()
	{
		if (m_spinDirection == BladeSpinDirection.CORRECT)
			m_currentSpinSpeed = m_spinSpeed;
		else if (m_spinDirection == BladeSpinDirection.INCORRECT)
			m_currentSpinSpeed = m_spinSpeed * -1.0f;
	}

	/// <summary>
	/// Function to be called when the spin direction updates
	/// </summary>
	/// <param name="newSpin">The new spin direction</param>
	public void UpdateSpinDirection(BladeSpinDirection newSpin)
	{
		if (m_spinDirection == BladeSpinDirection.CORRECT)
			m_currentSpinSpeed = m_spinSpeed;
		else if (m_spinDirection == BladeSpinDirection.INCORRECT)
			m_currentSpinSpeed = m_spinSpeed * -1.0f;
	}

	/// <summary>
	/// Function to be called when the RPM rate of the Woodchipper updates
	/// </summary>
	/// <param name="newRPM">The new RPM rating of the machine</param>
	/// <param name="baseSpinSpeed">The spin speed at 500 RPM</param>
	public void UpdateMachineRPM(RotationRate newRPM, float baseSpinSpeed)
	{
		m_machineRPM = newRPM;
		switch (m_machineRPM)
		{
			case (RotationRate.RPM300):
				m_spinSpeed = baseSpinSpeed * 0.6f;
				break;
			case (RotationRate.RPM400):
				m_spinSpeed = baseSpinSpeed * 0.8f;
				break;
			case (RotationRate.RPM500):
				m_spinSpeed = baseSpinSpeed;
				break;
			case (RotationRate.RPM600):
				m_spinSpeed = baseSpinSpeed * 1.2f;
				break;
			case (RotationRate.RPM700):
				m_spinSpeed = baseSpinSpeed * 1.4f;
				break;
			default:
				Debug.LogError("Invalid RotationRate passed to SpinningBlade through UpdateMachineRPM");
				break;
		}
	}

	/// <summary>
	/// Update function, runs every frame
	/// </summary>
	private void Update()
	{
		//rotate the blades a little each frame
		transform.Rotate(Vector3.right, m_currentSpinSpeed * Time.deltaTime);
	}
}
