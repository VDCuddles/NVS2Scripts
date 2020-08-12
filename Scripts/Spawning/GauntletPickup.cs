using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletPickup : Pickup
{
	private Player player;
	private SpawnController sc;
	private SoundLibrary sl;

    private void OnTriggerEnter(Collider other)
    {
		player = FindObjectOfType<Player>();
		sc = FindObjectOfType<SpawnController>();
		sl = FindObjectOfType<SoundLibrary>();

		if (player && other.gameObject.GetComponent<Player>() && GetComponentInChildren<Pickup>())
		{
			player.GetPlayerOwnedWeapons().Add(sc.provideGameObject(GetComponentInChildren<Pickup>().GetPickupName()));
			player.AnimateSwapWeapon(player.GetCurrentWeapon(), GetComponentInChildren<Pickup>().GetPickupName());
			player.gameObject.GetComponent<AudioSource>().PlayOneShot(sl.GetPickupGet());
			player.gameObject.GetComponent<AudioSource>().PlayOneShot(sl.GetGauntlets());
			foreach (GauntletPickup gp in FindObjectsOfType<GauntletPickup>())
			{
				Destroy(gp.gameObject, 0.2f);
			}

		}
	}
}
