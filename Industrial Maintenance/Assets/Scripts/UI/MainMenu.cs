using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[Header("Menu panels")]
	[Tooltip("The panel for the primary menu")]
	[SerializeField] private GameObject m_primaryMenu;
	[Tooltip("The panel for the high score menu")]
	[SerializeField] private GameObject m_highScoreMenu;
	[Tooltip("The panel for the credits menu")]
	[SerializeField] private GameObject m_creditsMenu;

	[Header("Inputs")]
	[Tooltip("The name input field")]
	[SerializeField] private InputField m_nameInput;

	[Header("Misc Variables")]
	[Tooltip("The index of the play scene")]
	[SerializeField] private int m_playSceneIndex;

	/// <summary>
	/// Activates the proper screens
	/// </summary>
	private void Awake()
	{
		m_primaryMenu.SetActive(true);
		m_highScoreMenu.SetActive(false);
		m_creditsMenu.SetActive(false);
	}

	/// <summary>
	/// Code to be run when the 'high scores' button is pressed
	/// </summary>
	public void OnHighScorePressed()
	{
		m_highScoreMenu.SetActive(true);
		m_primaryMenu.SetActive(false);
	}

	/// <summary>
	/// Code to be run when the back button is pressed when on the high scores menu
	/// </summary>
	public void OnHighScoreBackPressed()
	{
		m_highScoreMenu.SetActive(false);
		m_primaryMenu.SetActive(true);
	}

	/// <summary>
	/// Code to be run when the credits button is pressed
	/// </summary>
	public void OnCreditsPressed()
	{
		m_creditsMenu.SetActive(true);
		m_primaryMenu.SetActive(false);
	}

	/// <summary>
	/// Code to be run when the back button is pressed when on the credits menu
	/// </summary>
	public void OnCreditsBackPressed()
	{
		m_creditsMenu.SetActive(false);
		m_primaryMenu.SetActive(true);
	}

	/// <summary>
	/// Code to be run when the start button is pressed
	/// </summary>
	public void OnStartPressed()
	{
		ScoreManager.SetName(m_nameInput.text);
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_playSceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);
	}

	/// <summary>
	/// Code to be run when the quit button is pressed
	/// </summary>
	public void OnQuitPressed()
	{
		Application.Quit();
	}
}
