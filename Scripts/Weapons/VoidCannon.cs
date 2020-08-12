using System.Collections;
using UnityEngine;

public class VoidCannon : ChargingWeapon
{
	[SerializeField] private AudioClip fireclip = default;
	[SerializeField] private static int voidcost = 500;
	private Animator animator;

	public override void Shoot()
	{
		LMBtimedown += Time.deltaTime;
		if (!isFiring && cooldownTimer <= 0)
		{
			StartCoroutine("PlayChargeSound");
		}
		if (LMBtimedown > GetDelayTime() && LMBtimedown < GetOverheatTime() && cooldownTimer <= 0 && !isFiring)
		{
			if (GetStatLibrary().GetDetrizideBank() > voidcost)
			{
				Debug.Log("Void cost = " + voidcost);
				GetEmBoi();
				isFiring = true;
				GetStatLibrary().SetDetrizideBank(GetStatLibrary().GetDetrizideBank() - voidcost);
				voidcost += 500;
			}
			else
			{
				StartCoroutine(StopFiring(0.8f));
				isFiring = true;
			}
		}
		else if (LMBtimedown >= GetOverheatTime() && !isOverheatSoundPlaying)
		{
			StartCoroutine("PlayOHSound");
			cooldownTimer = GetCoolDownTime();
			isFiring = false;
		}
	}

	private void GetEmBoi()
	{
		if (!animator)
			animator = GetComponent<Animator>();
		animator.SetTrigger("Fire");
		FireProjectile();
		GetASO().PlayOneShot(fireclip);

	}

}
