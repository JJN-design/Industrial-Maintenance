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

	[Header("Other text")]
	[Tooltip("The display for which machine failed")]
	[SerializeField] private Text m_machineFailedText;
	
	/// <summary>
	/// Updates the text for the manager
	/// </summary>
	void Update()
    {
		//update score display
		m_scoreText.text = ScoreManager.GetScore().ToString();

		//get high scores
		HighScore[] highScores = ScoreManager.GetHighScores();

		float[] secondsSurvived = { highScores[0].time, highScores[1].time, highScores[2].time };
		float[] minutesSurvived = { Mathf.Floor(secondsSurvived[0] / 60), Mathf.Floor(secondsSurvived[1] / 60), Mathf.Floor(secondsSurvived[2] / 60) };
		secondsSurvived[0] -= (minutesSurvived[0] * 60);
		secondsSurvived[1] -= (minutesSurvived[1] * 60);
		secondsSurvived[2] -= (minutesSurvived[2] * 60);

		//update high scores
		if(m_firstHighScoreText != null)
			m_firstHighScoreText.text = highScores[0].name + " / " + highScores[0].score.ToString() + " / " + minutesSurvived[0] + ":" + secondsSurvived[0].ToString("#.00");
		if(m_secondHighScoreText != null)
			m_secondHighScoreText.text = highScores[1].name + " / " + highScores[1].score.ToString() + " / " + minutesSurvived[1] + ":" + secondsSurvived[1].ToString("#.00");
		if(m_thirdHighScoreText != null)
			m_thirdHighScoreText.text = highScores[2].name + " / " + highScores[2].score.ToString() + " / " + minutesSurvived[2] + ":" + secondsSurvived[2].ToString("#.00");
	}

	/// <summary>
	/// Sets the failure string
	/// </summary>
	/// <param name="failString">The string that should be displayed</param>
	public void UpdateFailed(string failString)
	{
		m_machineFailedText.text = failString;
	}

	private bool m_scoresSaveable = true;

	/// <summary>
	/// Saves the scores and shows the panel
	/// </summary>
	public void ShowScores()
	{
		if(m_scoresSaveable)
		{
			m_highScorePanel.SetActive(true);
			ScoreManager.SaveNewScores();
			m_scoresSaveable = false;
		}
	}
}
