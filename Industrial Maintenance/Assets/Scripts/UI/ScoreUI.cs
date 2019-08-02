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

	// Update is called once per frame
	void Update()
    {
		//update score display
		m_scoreText.text = ScoreManager.GetScore().ToString();

		//update high scores
		m_firstHighScoreText.text = ScoreManager.GetHighScores()[0].ToString();
		m_secondHighScoreText.text = ScoreManager.GetHighScores()[1].ToString();
		m_thirdHighScoreText.text = ScoreManager.GetHighScores()[2].ToString();
	}

	public void ShowScores()
	{
		m_highScorePanel.SetActive(true);
		ScoreManager.SaveNewScores();
	}
}
