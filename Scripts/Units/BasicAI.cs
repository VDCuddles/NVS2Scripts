using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BasicAI : MonoBehaviour
{
	private Unit unit;
	private Transform target;
	private SoundLibrary sl;
	private AudioSource aso;

	[SerializeField] private float speed = default;
	[SerializeField] private float stoppingDistance = default;
	[SerializeField] private GameObject projectilePrefab = default;
	private float shootRateTimeStamp = 0f;
	[SerializeField] private float shootRate = default;

	public GameObject GetProjPrefab() { return projectilePrefab; }
	public void SetSRTS(float value) { shootRateTimeStamp = value; }
	public float GetShootRate() { return shootRate; }
	public float GetStoppingDistance() { return stoppingDistance; }
	public float GetSpeed() { return speed; }
	public AudioSource GetASO() { return aso; }
	public Unit GetUnit() { return unit; }
	public Transform GetTarget() { return target; }
	public SoundLibrary GetSL() { return sl; }
	public void SetSL(SoundLibrary value) { sl = value; }
	public void SetUnit(Unit value) { unit = value; }
	public void SetShootRate(float value) { shootRate = value; }
	public void SetSpeed(float value) { speed = value; }
	public void SetASO(AudioSource value) { aso = value; }
	public virtual void Start()
	{
		sl = FindObjectOfType<SoundLibrary>();
		unit = gameObject.GetComponent<Unit>();
		aso = GetComponent<AudioSource>();

		switch (unit.name)
		{
			case "Revenant":
				unit.SetDeadClip(sl.GetRevenantDead());
				unit.SetHurtClip(sl.GetRevenantHurt());
				break;
			case "EliteRevenant":
				unit.SetDeadClip(sl.GetRevenantDead());
				unit.SetHurtClip(sl.GetRevenantHurt());
				break;
			case "FriendlyRevenant":
				unit.SetDeadClip(sl.GetRevenantDead());
				unit.SetHurtClip(sl.GetRevenantHurt());
				break;
			case "Necromancer":
				unit.SetDeadClip(sl.GetNecroDead());
				unit.SetHurtClip(sl.GetNecroHurt());
				break;
		}
	}

	public virtual void Update()
	{
		ConfirmTarget(gameObject);
		AttackTarget();
	}

	public void ConfirmTarget(GameObject thisUnit)
	{
		if (!target)
		{
			target = Unit.GetClosestEnemy(thisUnit);
			if (GetComponent<Homing>())
				GetComponent<Homing>().SetTarget(target);
		}

		else if (target.GetComponent<Unit>() && target.GetComponent<Unit>().IsDead())
		{
			target = Unit.GetClosestEnemy(thisUnit);
			if (GetComponent<Homing>())
				GetComponent<Homing>().SetTarget(target);
		}
		else if (!unit.IsDead())
		{
			ChaseTarget();
		}
		else return;
	}

	public virtual void ChaseTarget()
	{
		if (target && !Singularity.isSingularityAwake)
		{
			if (Vector3.Distance(transform.position, target.position) <= stoppingDistance)
			{
				//melee would be here
			}
			else if (Vector3.Distance(transform.position, target.position) > stoppingDistance)
			{
				transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			}
		}
	}

	public void AttackTarget()
	{
		if (target)
		{
			if (target.GetComponent<Unit>() && !target.GetComponent<Unit>().IsDead() || target)
			{
				if (Time.time > shootRateTimeStamp)
					FireProjectile();
			}
		}
	}

	public virtual bool FireProjectile()
	{
		Vector3 spawnLoc = transform.position;
		GameObject instance = Instantiate(projectilePrefab, spawnLoc, Quaternion.identity, GameObject.Find("Projectiles").transform);
		if (instance.GetComponent<Bullet>())
			instance.GetComponent<Bullet>().SetParent(gameObject);
		instance.name = projectilePrefab.name;
		shootRateTimeStamp = Time.time + shootRate;
		instance.transform.forward = transform.forward;
		return true;
	}
}
