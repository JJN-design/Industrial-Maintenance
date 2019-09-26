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
	[Tooltip("How far the raycast from the player will go")]
	[SerializeField] private float m_raycastDist;
	[Tooltip("The interactable layer")]
	[SerializeField] private LayerMask m_interactableLayer;
	[Tooltip("How far you can move from the interactable without cancelling interaction")]
	[SerializeField] private float m_interactDistance;

	[Header("Limits")]
	[Tooltip("How high up the player can look")]
	[SerializeField] private float m_xRotationLimitsUpper;
	[Tooltip("How low down the player can look")]
	[SerializeField] private float m_xRotationLimitsLower;

	//The player
	private GameObject m_player;

	//Variables for mouse movement
	private Vector2 m_mouseLook;
	private Vector2 m_smoothV;

	//Variables for holding buttons
	private Interactable m_currentlyInteractingWith;
	private bool m_isInteracting;
    
	/// <summary>
	/// Called before first frame update
	/// </summary>
    void Awake()
    {
		m_player = this.transform.parent.gameObject;
    }

    /// <summary>
	/// Called each frame
	/// </summary>
    void Update()
    {
		if(Input.GetButtonDown("Fire1")) //call Interact() on click
		{
			Interact();
		}

		if(Input.GetButtonUp("Fire1")) //call StopInteract() when click is released
		{
			StopInteract();
		}

		if(m_currentlyInteractingWith != null) //check distance between player and object
		{
			float distance = Vector3.Distance(m_currentlyInteractingWith.transform.position, transform.position);
			if (distance >= m_interactDistance) //if distance is higher than threshold, stop interacting with object
				StopInteract();
		}
		
		//i don't really know what most of this code following does, i copied it off of some random github
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

		if(m_canLook) //checks if player is able to look around
		{
			transform.localRotation = Quaternion.AngleAxis(-m_mouseLook.y, Vector3.right);
			m_player.transform.localRotation = Quaternion.AngleAxis(m_mouseLook.x, m_player.transform.up);
		}
		//end of copied code
	}

	/// <summary>
	/// Code for interacting with interactables
	/// </summary>
	private void Interact()
	{
		//get middle of screen
		int x = Screen.width / 2;
		int y = Screen.height / 2;
		Camera cam = GetComponent<Camera>();
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(new Vector3(x, y, 0.0f)); 
		if (Physics.Raycast(ray, out hit, m_raycastDist, m_interactableLayer)) //ray from middle of screen
		{
			Debug.Log("Hit " + hit.transform.name);
			if(hit.transform.GetComponent<Interactable>() != null) //check that object has correct component
			{
				m_currentlyInteractingWith = hit.transform.GetComponent<Interactable>();
				m_currentlyInteractingWith.InteractWith();
				m_isInteracting = true;
			}
		}
	}

	/// <summary>
	/// Code for when player stops interaction with interactable
	/// </summary>
	private void StopInteract()
	{
		if(m_isInteracting) //if currently interacting
		{
			m_isInteracting = false; //no longer interacting
			m_currentlyInteractingWith.StopInteractingWith(); //call stop interacting code
			m_currentlyInteractingWith = null; //clear current interaction
		}
	}
	
	/// <summary>
	/// Function for disabling movement
	/// </summary>
	public void DisableMovement()
	{
		m_canLook = false;
	}
}
