using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
	[Tooltip("How fast the player should move")]
	[SerializeField] private float m_acceleration = 10.0f;

	[Tooltip("How fast the player should move")]
	[SerializeField] private float m_maxSpeed = 10.0f;

	[Tooltip("Drag value")]
	[SerializeField] private float m_drag = 10.0f;

	[Tooltip("The mouse camera script of the player")]
	[SerializeField] private MouseCamLook m_mouseCam;

	[Tooltip("The score UI script")]
	[SerializeField] private ScoreUI m_scoreUI;
	public ScoreUI GetUI() { return m_scoreUI; }

	[Tooltip("The index of the main menu scene")]
	[SerializeField] private int m_mainMenuIndex;

	//Whether or not the player can move
	private bool m_canMove = true;

	//Forward/backward movement
	private float m_translation;

	//Left/right movement
	private float m_strafe;

	//The character controller of the player
	private CharacterController m_controller;
	
	//The velocity of the thing
	private Vector3 m_velocity;

	private bool m_scoreOpen = false;

    // Start is called before the first frame update
    void Start()
    {
		m_controller = GetComponent<CharacterController>();
		m_velocity = Vector3.zero;

		//turn off cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
		Vector3 moveDir = MoveDirection();

		float velCompInMoveDir = Vector3.Dot(m_velocity, moveDir);
		float speedComponenent = Mathf.Clamp(m_maxSpeed - velCompInMoveDir, 0.0f, 1.0f);

		Vector3 finalAcceleration = speedComponenent * m_acceleration * moveDir;

		m_velocity -= m_velocity * m_drag * Time.deltaTime;

		if(m_canMove)
		{
			m_velocity += finalAcceleration * Time.deltaTime;
		}

		m_velocity += Physics.gravity * Time.deltaTime;

		if (m_velocity.sqrMagnitude > m_maxSpeed * m_maxSpeed)
			m_velocity = m_velocity.normalized * m_maxSpeed;

		m_controller.Move(m_velocity * Time.deltaTime);
		m_velocity = m_controller.velocity;

		if(Input.GetButtonDown("Cancel"))
		{
			if(!m_scoreOpen)
			{
				DisableMovement();
				m_scoreUI.ShowScores();
				m_scoreUI.UpdateFailed("Player hit Escape.");
				m_scoreOpen = true;
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_mainMenuIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);
			}
		}
    }

	public Vector3 MoveDirection()
	{
		Vector3 moveDir = Vector3.zero;
		int inputCount = 0;

		if(Input.GetAxisRaw("Vertical") > 0)
		{
			moveDir += transform.forward;
			++inputCount;
		}
		if (Input.GetAxisRaw("Vertical") < 0)
		{
			moveDir -= transform.forward;
			++inputCount;
		}
		if (Input.GetAxisRaw("Horizontal") > 0)
		{
			moveDir += transform.right;
			++inputCount;
		}
		if (Input.GetAxisRaw("Horizontal") < 0)
		{
			moveDir -= transform.right;
			++inputCount;
		}

		// Normalize movement vector if more than one direction vector is added.
		if (inputCount > 1)
			moveDir.Normalize();

		return moveDir;
	}

	public void DisableMovement()
	{
		m_canMove = false;
		m_mouseCam.DisableMovement();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
