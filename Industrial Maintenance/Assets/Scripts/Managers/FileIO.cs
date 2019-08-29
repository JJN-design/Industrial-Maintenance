using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public struct HighScore
{
	public int score;
	public string name;
}

public class FileIO : MonoBehaviour
{
	/// <summary>
	/// Saves the high scores
	/// </summary>
	/// <param name="highScore1">The highest score reached</param>
	/// <param name="highScore2">The second highest score reached</param>
	/// <param name="highScore3">The third highest score reached</param>
	static public void Save(HighScore highScore1, HighScore highScore2, HighScore highScore3)
	{
		FileStream file;
		string fileDestination = Application.persistentDataPath + "/save.dat";

		//check if file already exists
		if (!File.Exists(fileDestination))
			file = File.Create(fileDestination); //if file does not exist, create it
		else
			file = File.OpenWrite(fileDestination); //if file exists, open it for writing

		//condense high scores into an array
		HighScore[] highScores = { highScore1, highScore2, highScore3 };

		//write high scores to file in binary format
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(file, highScores);

		//done with the file
		file.Close();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	static public HighScore[] Load()
	{
		FileStream file;
		string fileDestination = Application.persistentDataPath + "/save.dat";

		//check if the file exists
		if (File.Exists(fileDestination))
			file = File.OpenRead(fileDestination); //file exists, open it for reading
		else
		{
			HighScore newScore = new HighScore();
			newScore.name = "Empty";
			newScore.score = 0;
			HighScore[] emptyArray = { newScore, newScore, newScore };
			return emptyArray; //no file exists, return empty array
		}

		//reformat and read the binary file
		BinaryFormatter formatter = new BinaryFormatter();
		HighScore[] highScores = (HighScore[])formatter.Deserialize(file);
		return highScores;
	}
}
