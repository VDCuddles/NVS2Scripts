using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lacerator : BasicAI
{
	[SerializeField] private int shrapnelCount = default;
	[SerializeField] private float acceleration = default;
	[SerializeField] private AudioClip spawnClip = default;
	[SerializeField] private AudioClip explodeClip = default;
	private AudioSource playeraso = default;

	public override void Start()
	{
		playeraso = GameObject.Find("Player").GetComponent<AudioSource>();
		SetSL(FindObjectOfType<SoundLibrary>());
		SetUnit(gameObject.GetComponent<Unit>());

		GetUnit().SetDeadClip(GetSL().GetLacerDead());
		GetUnit().SetHurtClip(GetSL().GetLacerHurt());

		if (!gameObject.CompareTag("LichSpawn"))
			GetComponent<AudioSource>().PlayOneShot(spawnClip);

	}

	public override void Update()
	{
		SetSpeed(GetSpeed() + acceleration*Time.deltaTime);
		ConfirmTarget(gameObject);
	}

	public override void ChaseTarget()
	{
		if (GetTarget() && !Singularity.isSingularityAwake)
		{
			if (Vector3.Distance(transform.position, GetTarget().position) <= GetStoppingDistance())
			{
				Explode();
			}
			else if (Vector3.Distance(transform.position, GetTarget().position) > GetStoppingDistance())
			{
				transform.position = Vector3.MoveTowards(transform.position, GetTarget().position, GetSpeed() * Time.deltaTime);
			}
		}
	}

	public void Explode()
	{
		SetShootRate(0.001f);
		for (int i=0; i<shrapnelCount; i++)
		{
			FireProjectile();
		}
		playeraso.PlayOneShot(explodeClip);
		Destroy(gameObject);
	}
}
