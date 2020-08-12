using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBench : MonoBehaviour
{
	private Player player;
	private SpawnController sc;
	private SoundLibrary sl;
	private Altar altar;
	private StatLibrary statl;
	[SerializeField] private int resIncreaseFactor = default;
	[SerializeField] private GameObject newBonebag = default;

	private void Start()
	{
		statl = FindObjectOfType<StatLibrary>();
		altar = FindObjectOfType<Altar>();
		player = FindObjectOfType<Player>();
		sc = FindObjectOfType<SpawnController>();
		sl = FindObjectOfType<SoundLibrary>();

	}
	private void OnTriggerStay(Collider other)
	{
		if (player && other.gameObject.GetComponent<Player>() && GetComponentInChildren<Pickup>())
		{
			string pickupname = GetComponentInChildren<Pickup>().GetPickupName();
			ApplyBonus(pickupname);
			if (statl.GetDetrizideBank() < 30000)
				GetComponent<AudioSource>().PlayOneShot(sl.GetPickupGet());
			Destroy(GetComponentInChildren<Pickup>().gameObject);
		}
	}

	private void ApplyBonus(string pickupname)
	{
		switch (pickupname)
		{
			case "SummonPlus2":
				if (statl.GetDetrizideBank() < 30000)
					GetComponent<AudioSource>().PlayOneShot(sl.GetSummonUpgrade());
				altar.SetResCost(altar.GetResCost() + resIncreaseFactor);
				altar.SetResMax(altar.GetResMax() + 2);
				break;
			case "Scythes":
				if (statl.GetDetrizideBank() < 30000)
					GetComponent<AudioSource>().PlayOneShot(sl.GetScythe());
				//replace bonebag with upgraded bonebag
				if (player.GetPlayerOwnedWeapons().Contains(sc.provideGameObject("Snubnose")))
					player.AnimateSwapWeapon(player.GetCurrentWeapon(), "Snubnose");
				else
					player.AnimateSwapWeapon(player.GetCurrentWeapon(), "BoneBag");
				sc.SetBoneBag(newBonebag);
				break;
			case "Health":
				Unit unit = player.GetComponent<Unit>();
				unit.SetMaxHealth(unit.GetMaxHealth() * 2);
				unit.SetHealth(unit.GetMaxHealth());
				break;
		}
	}
}
