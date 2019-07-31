using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	//The highest score reached
	static int m_firstHighScore;

	//The second highest score reached
	static int m_secondHighScore;

	//The third highest score reached
	static int m_thirdHighScore;

	//The current score
	static int m_currentScore;

    // Start is called before the first frame update
    void Awake()
    {
		ResetScore(); //set current score to none on scene load
		Debug.Log("score manager init");

		int[] highScores = FileIO.Load(); //attempt to load the high score from file
		m_firstHighScore = highScores[0];
		m_secondHighScore = highScores[1];
		m_thirdHighScore = highScores[2];
    }

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Keypad5))
		{
			AddScore(1);
			Debug.Log("Added one score");
		}
		if(Input.GetKeyDown(KeyCode.Keypad2))
		{
			Debug.Log("Current Score: " + m_currentScore.ToString());
			Debug.Log("\n1st Highest Score: " + m_firstHighScore.ToString());
			Debug.Log("\n2nd Highest Score: " + m_secondHighScore.ToString()); 
			Debug.Log("\n3rd Highest Score: " + m_thirdHighScore.ToString());
		}
		if(Input.GetKeyDown(KeyCode.Keypad8))
		{
			Debug.Log("Saved new scores");
			SaveNewScores();
		}
		if(Input.GetKeyDown(KeyCode.Keypad4))
		{
			Debug.Log("Reset score");
			ResetScore();
		}
	}

	/// <summary>
	/// Resets current score to zero
	/// </summary>
	static public void ResetScore()
	{
		m_currentScore = 0;
	}

	/// <summary>
	/// Gets the current score
	/// </summary>
	/// <returns>The current score</returns>
	static public int GetScore()
	{
		return m_currentScore;
	}

	/// <summary>
	/// Gets the high scores
	/// </summary>
	/// <returns>an array of the high scores</returns>
	static public int[] GetHighScores()
	{
		int[] highScores = { m_firstHighScore, m_secondHighScore, m_thirdHighScore };
		return highScores;
	}

	/// <summary>
	/// Adds score and shifts around high score values appropriately
	/// </summary>
	/// <param name="value">How much score to add</param>
	static public void AddScore(int value)
	{
		m_currentScore += value;
	}

	static public void SaveNewScores()
	{
		//if current score is higher than the third highest score, but is less than the second high score, set a new third highest score
		if (m_thirdHighScore < m_currentScore && m_currentScore <= m_secondHighScore)
			m_thirdHighScore = m_currentScore;

		//if current score is higher than the third highest score, but is less than the first high score, set a new second highest score and shift current second highest to third highest
		else if (m_secondHighScore <= m_currentScore && m_currentScore <= m_firstHighScore)
		{
			m_thirdHighScore = m_secondHighScore;
			m_secondHighScore = m_currentScore;
		}

		//if current high score is higher than the first highest score, set a new highest score and shift previous scores down
		else if (m_firstHighScore <= m_currentScore)
		{
			m_thirdHighScore = m_secondHighScore;
			m_secondHighScore = m_firstHighScore;
			m_firstHighScore = m_currentScore;
		}

		FileIO.Save(m_firstHighScore, m_secondHighScore, m_thirdHighScore);
	}
}
