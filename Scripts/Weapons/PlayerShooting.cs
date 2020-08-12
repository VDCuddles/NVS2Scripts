using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	List<PlayerWeapon> weapons;
	private InputController inputController;

	// Start is called before the first frame update
	void Start()
	{
		inputController = FindObjectOfType<InputController>();
	}

	// Update is called once per frame
	void Update()
	{
		weapons = new List<PlayerWeapon>(FindObjectsOfType<PlayerWeapon>());
		if (Input.GetMouseButton(inputController.LeftClick) && !gameObject.GetComponent<Unit>().IsDead() && !gameObject.GetComponent<Player>().GetIsSwapping())
			shootWeapons();
	}

	void shootWeapons()
	{
		foreach (PlayerWeapon weapon in weapons)
			weapon.Shoot();
	}
}
