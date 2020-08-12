using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingUnit : BasicAI
{
	[SerializeField] private float acceleration = default;
	[SerializeField] private float chargerange = default;
	[SerializeField] private float chargemultiplier = default;
	[SerializeField] private float chargeduration = default;
	[SerializeField] private AudioClip spawnClip = default;
	[SerializeField] private AudioClip chargesound = default;
	[SerializeField] private Material chargemat = default;
	[SerializeField] private float chargecooldown = default;
	private float chargecdtimer;
	private List<Material> originalMaterials;
	private AudioSource playeraso = default;
	private float originalspeed;
	private float originalturnspeed;
	private bool ischarging;
	private float chargetimer = 0;
	private HomingUnit homingunit;
	private new Collider collider;
	private Player player;

	public override void Start()
	{
		chargecdtimer = chargecooldown;

		collider = GetComponentInChildren<Collider>();
		homingunit = GetComponent<HomingUnit>();
		originalspeed = GetSpeed();
		originalturnspeed = homingunit.GetTurnSpeed();
		playeraso = GameObject.Find("Player").GetComponent<AudioSource>();
		SetSL(FindObjectOfType<SoundLibrary>());
		SetUnit(gameObject.GetComponent<Unit>());

		GetUnit().SetDeadClip(GetSL().GetJuggernautDead());
		GetUnit().SetHurtClip(GetSL().GetJuggernautHurt());

		GetComponent<AudioSource>().PlayOneShot(spawnClip);

		saveOriginalMaterials();

	}

	public override void Update()
	{
		ConfirmTarget(gameObject);
		if (chargecdtimer > 0)
			chargecdtimer -= Time.deltaTime;
		if (chargetimer > 0)
			chargetimer -= Time.deltaTime;
		if (!Singularity.isSingularityAwake)
		{
			if (!ischarging && chargecdtimer <= 0)
			{
				SetSpeed(GetSpeed() + acceleration * Time.deltaTime);
			}
			else if (chargetimer >= 0)
			{
				transform.position += transform.forward * GetSpeed() * chargemultiplier * Time.deltaTime;
			}
			else
			{
				EndCharge();
			}
			homingunit.ChaseForward(GetSpeed());
		}
	}

	private void Charge()
	{
		homingunit.SetTurnSpeed(0);
		//collider.isTrigger = true;
		FireProjectile();
		ischarging = true;
		playeraso.PlayOneShot(chargesound);
		overwriteMaterials();
		chargecdtimer = chargecooldown;
		chargetimer = chargeduration;
	}

	private void EndCharge()
	{
		Homing.TurnSelf(3, Vector3.right, transform);
		SetSpeed(originalspeed);
		//collider.isTrigger = false;
		ResetMaterials();
		ResetTurnSpeed();
		ischarging = false;
	}

	private void ResetTurnSpeed()
	{
		//needs gradual reset
		homingunit.SetTurnSpeed(originalturnspeed);
	}

	public override void ChaseTarget()
	{

		if (GetTarget() && !Singularity.isSingularityAwake)
		{
			if (Vector3.Distance(transform.position, GetTarget().position) <= chargerange && !ischarging)
				if(Laser.CheckLineOfSight(gameObject))
					Charge();	
		}
	}

	public void saveOriginalMaterials()
	{
		originalMaterials = new List<Material>();
		MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer boi in renderers)
			originalMaterials.Add(boi.material);
	}

	public void overwriteMaterials()
	{
		MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer boi in renderers)
			boi.material = chargemat;
	}

	public void ResetMaterials()
	{
		for (int i = 0; i < originalMaterials.Count; i++)
		{
			gameObject.GetComponentsInChildren<MeshRenderer>()[i].material = originalMaterials[i];
		}
	}
}
