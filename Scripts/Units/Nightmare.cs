using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightmare : HomingUnit
{
	[SerializeField] private int totaldamage;
	private float updatefrequency = 2;
	private float updateTimestamp;
	private bool laughplayed = false;
	private GameObject altar;
	private GameObject player;

	public override void Update()
	{
		if (!laughplayed)
		{
			altar = GameObject.Find("Altar");
			player = GameObject.Find("Player");
			SetTarget(altar.transform);
			GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<SoundLibrary>().ChooseLichSpawn());
			laughplayed = true;
		}
		ChaseOrUpdateTarget();
		PeriodicUpdate();
	}

	private void PeriodicUpdate()
	{
		if (Time.time > updateTimestamp)
		{
			if(Unit.ChooseRandomEnemy(player))
				SetTarget(Unit.ChooseRandomEnemy(player).transform);
			updateTimestamp += updatefrequency;
		}
	}
	private void ChaseOrUpdateTarget()
	{
		if (GetTarget())
		{
			if (GetTarget() == GetComponent<Bullet>().GetParent().transform)
				if (Unit.ChooseRandomEnemy(player))
					SetTarget(Unit.ChooseRandomEnemy(player).transform);
			
			TurnSelf(GetTurnSpeed(), GetTarget(), transform);
			ChaseForward(GetSpeed());
		}
		else if (Unit.ChooseRandomEnemy(player))
			SetTarget(Unit.ChooseRandomEnemy(player).transform);
		else
			SetTarget(altar.transform);
	}
}
