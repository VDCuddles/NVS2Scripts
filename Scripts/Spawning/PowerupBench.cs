using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBench : MonoBehaviour
{
	private Player player;
	private SoundLibrary sl;
	private string currentPickup = "";
	private GameObject aurainstance;
	private int countdowntime = 10;
	[SerializeField] private float putimeout = default;
	[SerializeField] private AudioClip countdown = default;
	[SerializeField] private GameObject aura = default;
	

	private void Start()
	{
		player = FindObjectOfType<Player>();
		sl = FindObjectOfType<SoundLibrary>();

	}
	private void OnTriggerStay(Collider other)
	{
		if (player && other.gameObject.GetComponent<Player>() && GetComponentInChildren<Pickup>())
		{
			currentPickup = GetComponentInChildren<Pickup>().GetPickupName();
			StartCoroutine("ApplyPowerup");
			GetComponent<AudioSource>().PlayOneShot(sl.GetPickupGet());
			Destroy(GetComponentInChildren<Pickup>().gameObject);
		}
	}

	IEnumerator ApplyPowerup()
	{
		Vector3 auraposition = player.transform.position;
		auraposition.z += 3;
		auraposition.y += 3;
		switch (currentPickup)
		{
			case "QuadDamage":
				aurainstance = Instantiate(aura, auraposition, Quaternion.identity, player.transform);
				aurainstance.name = aura.name;
				Destroy(aurainstance, putimeout);
				GetComponent<AudioSource>().PlayOneShot(sl.GetAnnihilation());
				player.SetDamageModifier(4);
				yield return new WaitForSeconds(putimeout-countdowntime);
				StartCoroutine("CountDown");
				break;
		}
	}

	IEnumerator CountDown()
	{
		for (int i = 0; i < countdowntime; i++)
		{
			GetComponent<AudioSource>().PlayOneShot(countdown);
			yield return new WaitForSeconds(1);
		}
		player.SetDamageModifier(1);
	}
}
