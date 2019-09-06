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

	/// <summary>
	/// Sets up variables for the spinning blades
	/// </summary>
	/// <param name="spinDirection">What direction these spin</param>
	/// <param name="spinSpeed">How fast these should spin</param>
	/// <param name="brokenMulti">How much slower these should go if machine is broken</param>
	public void Create(BladeSpinDirection spinDirection, float spinSpeed, float brokenMulti)
	{
		m_spinDirection = spinDirection;
		m_spinSpeed = spinSpeed;
		m_brokenSpeedMultiplier = brokenMulti;

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
	/// Update function, runs every frame
	/// </summary>
	private void Update()
	{
		//rotate the blades a little each frame
		transform.Rotate(Vector3.up, m_currentSpinSpeed * Time.deltaTime);
	}
}
