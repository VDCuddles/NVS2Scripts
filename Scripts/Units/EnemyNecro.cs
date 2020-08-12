using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNecro : BasicAI
{
	[SerializeField] private AudioClip laughtrack = default;
	[SerializeField] private AudioClip summontrack = default;
	[SerializeField] private int maxNecroSpawns = default;

	public int GetMaxSpawns() { return maxNecroSpawns; }

	public override bool FireProjectile()
	{
		Vector3 spawnLoc = GetComponentInChildren<SpriteRenderer>().transform.position;
		if (Player.CountFriends("NecroSpawn") >= GetMaxSpawns())
		{
			return false;
		}
		SoundLibrary.ResetPitch(gameObject.GetComponentInChildren<AudioSource>());
		SoundLibrary.varySoundPitch(gameObject.GetComponentInChildren<AudioSource>(), 0.5f);
		gameObject.GetComponentInChildren<AudioSource>().PlayOneShot(laughtrack);
		gameObject.GetComponentInChildren<AudioSource>().PlayOneShot(summontrack);
		GameObject instance = Instantiate(GetProjPrefab(), spawnLoc, Quaternion.identity, GameObject.Find("NecroSpawns").transform);
		instance.tag = "NecroSpawn";
		instance.name = GetProjPrefab().name;
		SetSRTS(Time.time + GetShootRate());
		instance.transform.forward = transform.forward;
		return true;
	}
}
