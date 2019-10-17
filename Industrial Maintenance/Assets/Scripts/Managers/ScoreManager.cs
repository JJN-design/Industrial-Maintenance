using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	//The highest score reached
	static HighScore m_firstHighScore;

	//The second highest score reached
	static HighScore m_secondHighScore;

	//The third highest score reached
	static HighScore m_thirdHighScore;

	//The current score
	static int m_currentScore;

	//the name of the current player
	static string m_currentName = "NoName";

	//The current time survived
	static float m_currentTime;

	//whether or not score and timer can increment
	static bool m_scoreIncrementable = true;

    // Start is called before the first frame update
    void Awake()
    {
		ResetScore(); //set current score to none on scene load

		HighScore[] highScores = FileIO.Load(); //attempt to load the high score from file
		m_firstHighScore = highScores[0];
		m_secondHighScore = highScores[1];
		m_thirdHighScore = highScores[2];
    }

	static public void SetTime(float time)
	{
		if(m_scoreIncrementable)
			m_currentTime = time;
	}

	/// <summary>
	/// Sets the name of the player
	/// </summary>
	/// <param name="name">The name to input</param>
	static public void SetName(string name)
	{
		m_currentName = name;
	}

	/// <summary>
	/// Resets current score to zero
	/// </summary>
	static public void ResetScore()
	{
		m_currentScore = 0;
	}

	static public void FailLevel()
	{
		m_scoreIncrementable = false;
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
	static public HighScore[] GetHighScores()
	{
		HighScore[] highScores = { m_firstHighScore, m_secondHighScore, m_thirdHighScore };
		return highScores;
	}

	/// <summary>
	/// Adds score and shifts around high score values appropriately
	/// </summary>
	/// <param name="value">How much score to add</param>
	static public void AddScore(int value)
	{
		if(m_scoreIncrementable)
			m_currentScore += value;
	}

	/// <summary>
	/// Saves the current score to high scores if applicable
	/// </summary>
	static public void SaveNewScores()
	{
		//if current score is higher than the third highest score, but is less than the second high score, set a new third highest score
		if (m_thirdHighScore.score < m_currentScore && m_currentScore <= m_secondHighScore.score)
		{
			m_thirdHighScore.score = m_currentScore;
			m_thirdHighScore.name = m_currentName;
			m_thirdHighScore.time = m_currentTime;
		}

		//if current score is higher than the third highest score, but is less than the first high score, set a new second highest score and shift current second highest to third highest
		else if (m_secondHighScore.score <= m_currentScore && m_currentScore <= m_firstHighScore.score)
		{
			m_thirdHighScore = m_secondHighScore;
			m_secondHighScore.score = m_currentScore;
			m_secondHighScore.name = m_currentName;
			m_secondHighScore.time = m_currentTime;
		}

		//if current high score is higher than the first highest score, set a new highest score and shift previous scores down
		else if (m_firstHighScore.score <= m_currentScore)
		{
			m_thirdHighScore = m_secondHighScore;
			m_secondHighScore = m_firstHighScore;
			m_firstHighScore.score = m_currentScore;
			m_firstHighScore.name = m_currentName;
			m_firstHighScore.time = m_currentTime;
		}

		FileIO.Save(m_firstHighScore, m_secondHighScore, m_thirdHighScore);
	}
}
