using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System;

public class GameOptions : MonoBehaviour
{

	[SerializeField] private bool tutorialsEnabled = default;
	[SerializeField] private bool UIenabled = default;
	[SerializeField] private int defaultMaxHealth = default;
	[SerializeField] private MusicPlayer musicplayer = default;

	public void SetTutorialsEnabled(bool value) { tutorialsEnabled = value; }
	public bool GetTutorialsEnabled() { return tutorialsEnabled; }
	public void SetMaxHealth(int value) { defaultMaxHealth = value; }
	public int GetMaxHealth() { return defaultMaxHealth; }
	public bool GetUIEnabled() { return UIenabled; }
	public void SetUIEnabled(bool value) { UIenabled = value; }

	private SaveData savedata;
	public SaveData GetSave() { return savedata; }

	private Type ictype = typeof(InputController);
	private Type savetype = typeof(SaveData);
	private InputController ic;

	void Start()
	{
		ic = gameObject.GetComponent<InputController>();
		LoadSaveData();
		DontDestroyOnLoad(gameObject);
	}

	public void LoadSaveData()
	{
		if (SaveSystem.LoadGame() != null)
		{
			savedata = SaveSystem.LoadGame();
			LoadVolResAndGQual();
		}

		else
		{
			SaveSystem.SaveGame(0, this, ic, musicplayer.GetChosenVolume(), Screen.currentResolution.width, Screen.currentResolution.height, QualitySettings.GetQualityLevel());
			Debug.Log("Created default save.");
			savedata = SaveSystem.LoadGame();
			LoadVolResAndGQual();
		}

		tutorialsEnabled = savedata.tutorialsEnabled;
		UIenabled = savedata.UIenabled;
		defaultMaxHealth = savedata.defaultMaxHealth;

		foreach (System.Reflection.FieldInfo field in ictype.GetFields())
		{
			foreach (System.Reflection.FieldInfo sfield in savetype.GetFields())
			{
				if (field.Name == sfield.Name)
					field.SetValue(ic, sfield.GetValue(savedata));
			}
		}
	}

	public void LoadVolResAndGQual()
	{
		SetMaxHealth(savedata.defaultMaxHealth);
		musicplayer.SetChosenVolume(savedata.musicVolume);
		musicplayer.gameObject.GetComponent<AudioSource>().volume = musicplayer.GetChosenVolume();
		Screen.SetResolution(savedata.reswidth, savedata.resheight, true);
		QualitySettings.SetQualityLevel(savedata.gqualindex);
	}
}
