using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
	[Header("Text fields")]
	[Tooltip("The text for the first highest score")]
	[SerializeField] private Text m_firstHighScoreText;
	[Tooltip("The text for the second highest score")]
	[SerializeField] private Text m_secondHighScoreText;
	[Tooltip("The text for the third highest score")]
	[SerializeField] private Text m_thirdHighScoreText;

	/// <summary>
	/// Code to be called on awake
	/// </summary>
	private void Awake()
	{
		//get high scores
		HighScore[] highScores = ScoreManager.GetHighScores();

		//calculate time survived
		float[] secondsSurvived = { highScores[0].time, highScores[1].time, highScores[2].time };
		float[] minutesSurvived = { Mathf.Floor(secondsSurvived[0] / 60), Mathf.Floor(secondsSurvived[1] / 60), Mathf.Floor(secondsSurvived[2] / 60) };
		secondsSurvived[0] -= (minutesSurvived[0] * 60);
		secondsSurvived[1] -= (minutesSurvived[1] * 60);
		secondsSurvived[2] -= (minutesSurvived[2] * 60);

		//update text displays
		m_firstHighScoreText.text = highScores[0].name + " / " + highScores[0].score.ToString() + " / " + minutesSurvived[0] + ":" + secondsSurvived[0].ToString("#.00");
		m_secondHighScoreText.text = highScores[1].name + " / " + highScores[1].score.ToString() + " / " + minutesSurvived[1] + ":" + secondsSurvived[1].ToString("#.00");
		m_thirdHighScoreText.text = highScores[2].name + " / " + highScores[2].score.ToString() + " / " + minutesSurvived[2] + ":" + secondsSurvived[2].ToString("#.00");
	}
}
