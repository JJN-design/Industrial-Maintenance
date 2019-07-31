using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
	[SerializeField] private Text m_scoreText;
	[SerializeField] private Text m_firstHighScoreText;
	[SerializeField] private Text m_secondHighScoreText;
	[SerializeField] private Text m_thirdHighScoreText;

	// Update is called once per frame
	void Update()
    {
		//update score display
		m_scoreText.text = ScoreManager.GetScore().ToString();
    }
}
