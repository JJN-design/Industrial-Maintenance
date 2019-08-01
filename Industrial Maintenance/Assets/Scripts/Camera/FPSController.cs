using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
	[Tooltip("How fast the player should move")]
	[SerializeField] private float m_speed = 10.0f;

	//Forward/backward movement
	private float m_translation;

	//Left/right movement
	private float m_strafe;

	private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
		m_rigidbody = GetComponent<Rigidbody>();
		//turn off cursor
		Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
		//Input.GetAxis() is used to get user input
		m_translation = Input.GetAxis("Vertical") * m_speed * Time.deltaTime;
		m_strafe = Input.GetAxis("Horizontal") * m_speed * Time.deltaTime;
		//transform.Translate(m_strafe, 0, m_translation);
		m_rigidbody.AddRelativeForce(new Vector3(m_strafe, 0, m_translation));

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			//enable cursor
			Cursor.lockState = CursorLockMode.None;
		}
    }
}
