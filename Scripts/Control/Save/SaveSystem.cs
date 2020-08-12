using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem

{
	public static void SaveGame(int score, GameOptions go, InputController input, float mvol, int rwidth, int rheight, int gqual)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/save.necro";
		FileStream stream = new FileStream(path, FileMode.Create);

		SaveData data = new SaveData(score, go, input, mvol, rwidth, rheight, gqual);

		formatter.Serialize(stream, data);
		Debug.Log("Saved data to " + path + " successfully.");
		stream.Close();
	}

	public static SaveData LoadGame()
	{
		string path = Application.persistentDataPath + "/save.necro";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			SaveData data = formatter.Deserialize(stream) as SaveData;
			Debug.Log("Loaded data from " + path + " successfully.");
			//Debug.Log("highscore = " + data.highScore);

			stream.Close();
			return data;
		}
		else
		{
			Debug.Log("Save file not found at " + path);
			return null;
		}
	}

	public static void ClearSave()
	{
		string path = Application.persistentDataPath + "/save.necro";
		if (File.Exists(path))
			File.Delete(path);
		Debug.Log("File deleted from " + path);
	}
}
