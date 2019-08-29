using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
	[Tooltip("How fast the player should move")]
	[SerializeField] private float m_speed = 10.0f;

	[Tooltip("The mouse camera script of the player")]
	[SerializeField] private MouseCamLook m_mouseCam;

	[Tooltip("The score UI script")]
	[SerializeField] private ScoreUI m_scoreUI;
	public ScoreUI GetUI() { return m_scoreUI; }

	//Whether or not the player can move
	private bool m_canMove = true;

	//Forward/backward movement
	private float m_translation;

	//Left/right movement
	private float m_strafe;

	//The rigidbody of the player
	private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
		m_rigidbody = GetComponent<Rigidbody>();
		//turn off cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
		//Input.GetAxis() is used to get user input
		m_translation = Input.GetAxis("Vertical") * m_speed * Time.deltaTime;
		m_strafe = Input.GetAxis("Horizontal") * m_speed * Time.deltaTime;
		//transform.Translate(m_strafe, 0, m_translation);
		if(m_canMove)
			m_rigidbody.AddRelativeForce(new Vector3(m_strafe, 0, m_translation));

		if(Input.GetButtonDown("Cancel"))
		{
			DisableMovement();
			m_scoreUI.ShowScores();
		}
    }

	public void DisableMovement()
	{
		m_canMove = false;
		m_mouseCam.DisableMovement();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
