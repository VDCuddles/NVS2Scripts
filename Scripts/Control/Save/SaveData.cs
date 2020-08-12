using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SaveData
{
    public int highScore;
	//Options info
	public float musicVolume;
	public int reswidth;
	public int resheight;
	public int gqualindex;

	//GameOptions info;
	public bool tutorialsEnabled;
	public bool UIenabled;
	public int defaultMaxHealth;
	//InputController info
	public KeyCode MoveUp;
	public KeyCode MoveDown;
	public KeyCode MoveLeft;
	public KeyCode MoveRight;
	public KeyCode Detrizide;
	public KeyCode Jump;
	public KeyCode ToggleMusic;
	public KeyCode ToggleUI;
	public KeyCode Reset;
	public KeyCode Exit;
	public KeyCode CycleWeapons;
	public KeyCode NextWeapon;
	//guns
	public KeyCode Gauntlets;
	public KeyCode BoneBag;
	public KeyCode Shotgun;
	public KeyCode Devourer;
	public KeyCode Reaver;
	public KeyCode Crucifier;
	public KeyCode VoidCannon;

	public float mouseSensitivity;

	public SaveData(int score, GameOptions go, InputController input, float mvol, int rwidth, int rheight, int gqual)
    {
        highScore = score;
		//Options info
		musicVolume = mvol;
		reswidth = rwidth;
		resheight = rheight;
		gqualindex = gqual;

		//GameOptions info;
		tutorialsEnabled = go.GetTutorialsEnabled();
		UIenabled = go.GetUIEnabled();
		defaultMaxHealth = go.GetMaxHealth();
		//InputController info
		MoveUp = input.MoveUp;
		MoveDown = input.MoveDown;
		MoveLeft = input.MoveLeft;
		MoveRight = input.MoveRight;
		Detrizide = input.Detrizide;
		Jump = input.Jump;
		ToggleMusic = input.ToggleMusic;
		ToggleUI = input.ToggleUI;
		Reset = input.Reset;
		Exit = input.Exit;
		CycleWeapons = input.CycleWeapons;
		NextWeapon = input.NextWeapon;
		//guns
		Gauntlets = input.Gauntlets;
		BoneBag = input.BoneBag;
		Shotgun = input.Shotgun;
		Devourer = input.Devourer;
		Reaver = input.Reaver;
		Crucifier = input.Crucifier;
		VoidCannon = input.VoidCannon;

		mouseSensitivity = input.mouseSensitivity;
	}
}
