using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamLook : MonoBehaviour
{
	//Whether or not the player can currently look around
	private bool m_canLook = true;

	[Header("Mouse settings")]
		[Tooltip("The sensitivity of the mouse look")]
		[SerializeField] private float m_sensitivity = 5.0f;
		[Tooltip("The mouse smoothing to be applied")]
		[SerializeField] private float m_smoothing = 2.0f;

	[Header("Limits")]
		[Tooltip("How high up the player can look")]
		[SerializeField] private float m_xRotationLimitsUpper;
		[Tooltip("How low down the player can look")]
		[SerializeField] private float m_xRotationLimitsLower;

	//The player
	private GameObject m_player;

	private Vector2 m_mouseLook;
	private Vector2 m_smoothV;
    
	// Start is called before the first frame update
    void Start()
    {
		m_player = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
			m_canLook = false;
		if(m_canLook)
		{
			Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(m_sensitivity * m_smoothing, m_sensitivity * m_smoothing));

			//interpolated float between the two values
			m_smoothV.x = Mathf.Lerp(m_smoothV.x, mouseDelta.x, 1f / m_smoothing);
			m_smoothV.y = Mathf.Lerp(m_smoothV.y, mouseDelta.y, 1f / m_smoothing);

			m_mouseLook += m_smoothV;
			if (m_mouseLook.y > m_xRotationLimitsUpper)
				m_mouseLook.y = m_xRotationLimitsUpper;
			if (m_mouseLook.y < m_xRotationLimitsLower)
				m_mouseLook.y = m_xRotationLimitsLower;

			transform.localRotation = Quaternion.AngleAxis(-m_mouseLook.y, Vector3.right);
			m_player.transform.localRotation = Quaternion.AngleAxis(m_mouseLook.x, m_player.transform.up);
		}
	}
}
