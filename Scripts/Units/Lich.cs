using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lich : BasicAI
{
	[SerializeField] private AudioClip summontrack = default;
	[SerializeField] private AudioClip laughtrack = default;
	[SerializeField] private int maxNecroSpawns = default;

	public AudioClip GetLaugh() { return laughtrack; }
	public int GetMaxSpawns() { return maxNecroSpawns; }

	public override void Start()
	{
		SetSL(FindObjectOfType<SoundLibrary>());
		SetUnit(gameObject.GetComponent<Unit>());
		SetASO(GetComponent<AudioSource>());

		GetASO().PlayOneShot(GetSL().ChooseLichSpawn());
		GetUnit().SetDeadClip(GetSL().GetLichDead());
		GetUnit().SetHurtClip(GetSL().GetLichHurt());
	}

	public override bool FireProjectile()
	{
		Vector3 spawnLoc = GetComponentInChildren<SpriteRenderer>().transform.position;
		if (Player.CountFriends("LichSpawn") >= GetMaxSpawns())
			return false;
		SoundLibrary.ResetPitch(gameObject.GetComponentInChildren<AudioSource>());
		SoundLibrary.varySoundPitch(gameObject.GetComponentInChildren<AudioSource>(), 0.5f);
		gameObject.GetComponentInChildren<AudioSource>().PlayOneShot(laughtrack);
		gameObject.GetComponentInChildren<AudioSource>().PlayOneShot(summontrack);
		GameObject instance = Instantiate(GetProjPrefab(), spawnLoc, Quaternion.identity, GameObject.Find("NecroSpawns").transform);
		instance.tag = "LichSpawn";
		instance.name = GetProjPrefab().name;
		SetSRTS(Time.time + GetShootRate());
		instance.transform.forward = transform.forward;

		if (gameObject.GetComponentInChildren<Collider>())
			Physics.IgnoreCollision(instance.GetComponentInChildren<Collider>(), gameObject.GetComponentInChildren<Collider>(), true);

		return true;
	}
}
