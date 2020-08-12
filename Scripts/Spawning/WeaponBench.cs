using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponBench : MonoBehaviour
{
	List<Material> originalMaterials;
	List<MeshRenderer> renderers;
	public List<Material> GetOriginalMaterials() { return originalMaterials; }
	public List<MeshRenderer> GetRendererList() { return renderers; }
	private SpawnController sc;
	private StatLibrary statl;
	private SoundLibrary sl;
	private Player player;
	private TextMeshPro TMPWN;
	private AudioSource aso;

	void Start()
	{
		aso = GetComponent<AudioSource>();
		TMPWN = GameObject.Find("TMPWeaponNum").GetComponent<TextMeshPro>();
		player = FindObjectOfType<Player>();
		sl = FindObjectOfType<SoundLibrary>();
		statl = FindObjectOfType<StatLibrary>();
		sc = FindObjectOfType<SpawnController>();
		originalMaterials = new List<Material>();
		renderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
		FindObjectOfType<Detrilight>().saveOriginalMaterials(originalMaterials, gameObject);

	}

	private void OnTriggerStay(Collider other)
	{
		if (player && other.gameObject.GetComponent<Player>() && GetComponentInChildren<Pickup>())
		{
			player.GetPlayerOwnedWeapons().Add(sc.provideGameObject(GetComponentInChildren<Pickup>().GetPickupName()));
			player.AnimateSwapWeapon(player.GetCurrentWeapon(), GetComponentInChildren<Pickup>().GetPickupName());
			if (statl.GetDetrizideBank() < 30000)
			{
				GetComponent<AudioSource>().PlayOneShot(sl.GetPickupGet());
				PlayGet();
			}
			Destroy(GetComponentInChildren<Pickup>().gameObject);
			TMPWN.text = "";
		}
	}

	private void PlayGet()
	{
		switch (GetComponentInChildren<Pickup>().GetPickupName())
		{
			case "Shotgun":
				aso.PlayOneShot(sl.GetShotgun());
				break;
			case "Gauntlets":
				aso.PlayOneShot(sl.GetGauntlets());
				break;
			case "Snubnose":
				aso.PlayOneShot(sl.GetSnubnose());
				break;
			case "Crucifier":
				aso.PlayOneShot(sl.GetCrucifier());
				break;
			case "Reaver":
				aso.PlayOneShot(sl.GetReaver());
				break;
			case "Devourer":
				aso.PlayOneShot(sl.GetDevourer());
				break;
			case "Void Cannon":
				aso.PlayOneShot(sl.GetVoidCannon());
				break;
		}
	}
}
