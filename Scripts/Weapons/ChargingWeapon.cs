using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingWeapon : PlayerWeapon
{
	[SerializeField] private AudioClip chargeSound = default;
	[SerializeField] private AudioClip overheatSound = default;
	[SerializeField] private float overheatTime = default;
	[SerializeField] private float cooldownTime = default;
	private InputController ic;
	private float delayTime;

	public float cooldownTimer { get; set; }
	public float LMBtimedown { get; set; }
	public bool isChargeSoundPlaying { get; set; }
	public bool isOverheatSoundPlaying { get; set; }
	public bool isFiring { get; set; } //used to tell whether to play charge sound or not

	public float GetOverheatTime() { return overheatTime; }
	public float GetDelayTime() { return delayTime; }
	public float GetCoolDownTime() { return cooldownTime; }

	public override void Start()
	{
		cooldownTimer = 0;
		LMBtimedown = 0;
		isChargeSoundPlaying = false;
		isOverheatSoundPlaying = false;
		isFiring = false;
		delayTime = chargeSound.length;

		SetCamera(FindObjectOfType<Camera>());
		SetUIC(FindObjectOfType<UIController>());
		ic = FindObjectOfType<InputController>();
		SetASO(FindObjectOfType<Player>().GetComponent<AudioSource>());
		SetUnit(FindObjectOfType<Player>().GetComponent<Unit>());
		SetSL(FindObjectOfType<StatLibrary>());
	}
	public override void Update()
	{
		if (Input.GetMouseButtonUp(ic.LeftClick))
			if (!isFiring)
				ResetState();

		if (cooldownTimer > 0)
		{
			cooldownTimer -= Time.deltaTime;
		}
	}

	public void ResetState()
	{
		GetASO().Stop();
		LMBtimedown = 0;
		isFiring = false;
		isChargeSoundPlaying = false;
	}

	IEnumerator PlayChargeSound()
	{
		if (!isChargeSoundPlaying)
		{
			isChargeSoundPlaying = true;
			GetASO().PlayOneShot(chargeSound);
			yield return new WaitForSeconds(chargeSound.length);
			isChargeSoundPlaying = false;
		}
	}

	IEnumerator PlayOHSound()
	{
		if (!isOverheatSoundPlaying)
		{
			isOverheatSoundPlaying = true;
			GetASO().PlayOneShot(overheatSound);
			yield return new WaitForSeconds(overheatSound.length);
			isOverheatSoundPlaying = false;	
		}
	}

	public IEnumerator StopFiring(float delay)
	{
		yield return new WaitForSeconds(delay);
		isFiring = false;
	}

	public override void Shoot()
	{
		LMBtimedown += Time.deltaTime;
		if (!isFiring && cooldownTimer <= 0)
		{
			StartCoroutine("PlayChargeSound");
		}
		if (LMBtimedown > delayTime && LMBtimedown < overheatTime && cooldownTimer <= 0 && !isFiring)
		{
			isFiring = true;
			FireProjectile();
			StartCoroutine(StopFiring(4.0f));
		}
		else if (LMBtimedown >= overheatTime)
		{
			StartCoroutine("PlayOHSound");
			cooldownTimer = cooldownTime;
			isFiring = false;
		}
	}
}
