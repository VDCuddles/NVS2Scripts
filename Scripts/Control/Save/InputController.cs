using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

	[SerializeField] public KeyCode MoveUp = KeyCode.W;
	[SerializeField] public KeyCode MoveDown = KeyCode.S;
	[SerializeField] public KeyCode MoveLeft = KeyCode.A;
	[SerializeField] public KeyCode MoveRight = KeyCode.D;
	[SerializeField] public KeyCode Detrizide = KeyCode.E;
	[SerializeField] public KeyCode Jump = KeyCode.Space;
	[SerializeField] public KeyCode ToggleMusic = KeyCode.M;
	[SerializeField] public KeyCode ToggleUI = KeyCode.Tab;
	[SerializeField] public KeyCode Reset = KeyCode.F12;
	[SerializeField] public KeyCode Exit = KeyCode.Escape;
	[SerializeField] public KeyCode CycleWeapons = KeyCode.F;
	[SerializeField] public KeyCode NextWeapon = KeyCode.Q;
	//guns
	[SerializeField] public KeyCode Gauntlets = KeyCode.Alpha1;
	[SerializeField] public KeyCode BoneBag = KeyCode.Alpha2;
	[SerializeField] public KeyCode Shotgun = KeyCode.Alpha3;
	[SerializeField] public KeyCode Crucifier = KeyCode.Alpha4;
	[SerializeField] public KeyCode Reaver = KeyCode.Alpha5;
	[SerializeField] public KeyCode Devourer = KeyCode.Alpha6;
	[SerializeField] public KeyCode VoidCannon = KeyCode.Alpha7;

	[SerializeField] public float mouseSensitivity = default;
	[SerializeField] public int LeftClick = 0;
	[SerializeField] public int RightClick = 1;

	private List<KeyCode> keyList;
	public List<KeyCode> GetKeyList() { return keyList; }

	private GameOptions gops;
	private GameObject crosshair;
	public void SetCrosshair(GameObject value) { crosshair = value; }
	public GameObject GetCrosshair() { return crosshair; }

	void Start()
	{
		crosshair = GameObject.Find("Crosshair");
		keyList = new List<KeyCode>();
		keyList.AddRange(new KeyCode[] { MoveUp, MoveDown, MoveLeft, MoveRight, Detrizide, Jump, ToggleMusic, ToggleUI, Reset, Exit, CycleWeapons, NextWeapon, Gauntlets, BoneBag, Shotgun, Devourer, Reaver, Crucifier, VoidCannon});

		gops = FindObjectOfType<GameOptions>();
	}

	void Update()
	{
		if (!crosshair)
			crosshair = GameObject.Find("Crosshair");
		if (crosshair)
		{
			if (crosshair.activeInHierarchy != gops.GetUIEnabled())
			{
				crosshair.SetActive(gops.GetUIEnabled());
			}
			if (Input.GetKeyDown(ToggleUI))
			{

				gops.SetUIEnabled(!gops.GetUIEnabled());
				crosshair.SetActive(gops.GetUIEnabled());
			}
		}
	}
}
