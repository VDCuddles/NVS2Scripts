using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
	Resolution[] resolutions;
	TMPro.TMP_Dropdown.OptionData[] healths;
	[SerializeField] private AudioMixer audioMixer = default;
	[SerializeField] private GameObject gamePersistent = default;
	[SerializeField] private TMPro.TMP_Dropdown resolutionDropDown = default;
	[SerializeField] private TMPro.TMP_Dropdown graphicsDropDown = default;
	[SerializeField] private TMPro.TMP_Dropdown healthDropdown = default;
	[SerializeField] private Toggle uitoggle = default;
	[SerializeField] private Toggle tutorialToggle = default;
	[SerializeField] private Slider msslider = default;
	[SerializeField] private Slider mpslider = default;
	[SerializeField] private TMPro.TextMeshProUGUI msnumber = default;
	[SerializeField] private TMPro.TextMeshProUGUI musicnumber = default;
	[SerializeField] private TMPro.TextMeshProUGUI sfxnumber = default;

	private InputController ic;
	private AudioSource mpaso;
	private MusicPlayer mp;
	private GameOptions gops;

	private void Start()
	{
		GameObject gp;
		if (!FindObjectOfType<GameOptions>())
		{
			gp = Instantiate(gamePersistent);
			gp.name = gamePersistent.name;
		}

		mpaso = FindObjectOfType<MusicPlayer>().gameObject.GetComponent<AudioSource>();
		mp = FindObjectOfType<MusicPlayer>();
		ic = FindObjectOfType<InputController>();
		gops = FindObjectOfType<GameOptions>();

		StartCoroutine("BuildOptions");
	}

	IEnumerator BuildOptions()
	{
		yield return new WaitForFixedUpdate();
		//resolution
		resolutions = Screen.resolutions;
		resolutionDropDown.ClearOptions();
		List<string> options = new List<string>();
		// set dropdown value index and so; value for resolution
		int currentResoIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + "x" + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width
				&& resolutions[i].height == Screen.currentResolution.height)
			{
				currentResoIndex = i;
			}
		}
		resolutionDropDown.AddOptions(options);
		resolutionDropDown.value = currentResoIndex;
		resolutionDropDown.RefreshShownValue();

		//graphics quality
		graphicsDropDown.value = QualitySettings.GetQualityLevel();
		graphicsDropDown.captionText.SetText("Graphics Quality");


		//max health
		healths = healthDropdown.options.ToArray();
		//set health dropdown value index/ current health value
		int currentHealthIndex = 0;
		for (int i = 0; i < healths.Length; i++)
		{
			if (healths[i].text == gops.GetMaxHealth().ToString())
			{
				currentHealthIndex = i;
			}
		}
		healthDropdown.value = currentHealthIndex;
		healthDropdown.RefreshShownValue();


		//mouse sensitivity
		msslider.value = FindObjectOfType<InputController>().mouseSensitivity;
		msnumber.text = msslider.value.ToString();

		//music volume
		mpslider.value = mpaso.gameObject.GetComponent<MusicPlayer>().GetChosenVolume();
		musicnumber.text = TruncateString(mpaso.volume.ToString(), 4);

		//toggles
		tutorialToggle.isOn = gops.GetTutorialsEnabled();
		uitoggle.isOn = gops.GetUIEnabled();
	}

	public void DeleteSettings()
	{
		SaveSystem.ClearSave();
		gops.LoadSaveData();
	}

	public void SaveSettings()
	{
		SaveSystem.SaveGame(gops.GetSave().highScore, gops, ic, mp.GetChosenVolume(), Screen.currentResolution.width, Screen.currentResolution.height, QualitySettings.GetQualityLevel());
		gops.LoadSaveData();

	}

	public static string TruncateString(string input, int limit)
	{
		string substring = input;

		if (substring.Length >= limit)
		{
			substring = "";

			for (int i = 0; i < limit; i++)
			{
				substring += input[i];
			}
		}
		return substring;

	}
	public void SetMusicVolume(float value)
	{
		mpaso.volume = value;
		mpaso.gameObject.GetComponent<MusicPlayer>().SetChosenVolume(value);
		musicnumber.text = TruncateString(mpaso.volume.ToString(), 4);
	}
	public void SetSFXVolume(float value)
	{
		audioMixer.SetFloat("SFXVolume", value);
		float vol;
		audioMixer.GetFloat("SFXVolume", out vol);
		sfxnumber.text = TruncateString(vol.ToString(), 4);
	}
	public void SetResolution(int resIndex)
	{
		Resolution resolution = resolutions[resIndex];
		Screen.SetResolution(resolution.width, resolution.height, true);
	}
	public void SetGraphicsQuality(int qualityindex)
	{
		QualitySettings.SetQualityLevel(qualityindex);
	}
	public void SetUIEnabled()
	{
		gops.SetUIEnabled(uitoggle.isOn);
	}
	public void SetTutorialsEnabled()
	{
		gops.SetTutorialsEnabled(tutorialToggle.isOn);
	}

	public void SetMaxHealth(int healthindex)
	{
		gops.SetMaxHealth(ExtractIntFromStr(healths[healthindex].text));
		if (gops.GetMaxHealth() > 250)
			GameObject.Find("HealthTip").GetComponent<TextMeshProUGUI>().text = "(Note: High Score will only track for 250 or less Max Health.)";
		else
			GameObject.Find("HealthTip").GetComponent<TextMeshProUGUI>().text = "";
	}

	public void SetMouseSensitivity(float sindex)
	{
		ic.mouseSensitivity = sindex;
		msnumber.text = msslider.value.ToString();
	}

	public static int ExtractIntFromStr(string str)
	{
		string a = str;
		string b = string.Empty;
		int val = 0;

		for (int i = 0; i < a.Length; i++)
		{
			if (char.IsDigit(a[i]))
				b += a[i];
		}

		if (b.Length > 0)
			val = int.Parse(b);
		return val;
	}
}
