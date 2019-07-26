using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamLook : MonoBehaviour
{
	[SerializeField] private float m_sensitivity = 5.0f;
	[SerializeField] private float m_smoothing = 2.0f;

	[SerializeField] private GameObject m_player;

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
		Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		//interpolated float between the two values
		m_smoothV.x = Mathf.Lerp(m_smoothV.x, mouseDelta.x, 1f / m_smoothing);
		m_smoothV.y = Mathf.Lerp(m_smoothV.y, mouseDelta.y, 1f / m_smoothing);

		m_mouseLook += m_smoothV;

		transform.localRotation = Quaternion.AngleAxis(-m_mouseLook.y, Vector3.right);
		m_player.transform.localRotation = Quaternion.AngleAxis(m_mouseLook.x, m_player.transform.up);
	}
}
