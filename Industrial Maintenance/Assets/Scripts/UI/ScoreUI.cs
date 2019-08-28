using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
	[Header("Score Displays")]
	[Tooltip("The display for the current score")]
	[SerializeField] private Text m_scoreText;

	[Header("High Score Displays")]
	[Tooltip("The panel the high scores are displayed on")]
	[SerializeField] private GameObject m_highScorePanel;

	[Tooltip("The display for the highest score")]
	[SerializeField] private Text m_firstHighScoreText;

	[Tooltip("The display for the second highest score")]
	[SerializeField] private Text m_secondHighScoreText;

	[Tooltip("The display for the third highest score")]
	[SerializeField] private Text m_thirdHighScoreText;
	
	/// <summary>
	/// Updates the text for the manager
	/// </summary>
	void Update()
    {
		//update score display
		m_scoreText.text = ScoreManager.GetScore().ToString();

		//get high scores
		HighScore[] highScores = ScoreManager.GetHighScores();

		//update high scores
		m_firstHighScoreText.text = highScores[0].name + " - " + highScores[0].score.ToString();
		m_secondHighScoreText.text = highScores[1].name + " - " + highScores[1].score.ToString();
		m_thirdHighScoreText.text = highScores[2].name + " - " + highScores[2].score.ToString();
	}

	/// <summary>
	/// Saves the scores and shows the panel
	/// </summary>
	public void ShowScores()
	{
		m_highScorePanel.SetActive(true);
		ScoreManager.SaveNewScores();
	}
}
