using System.Collections;
using UnityEngine;

public class Devourer : ChargingWeapon
{

	[SerializeField] private AudioClip fireclip = default;
	[SerializeField] private int projCount = 10;
	[SerializeField] private float firerate = 0.25f;
	[SerializeField] private static int devcost = 500;

	public override void Shoot()
	{
		LMBtimedown += Time.deltaTime;
		if (!isFiring && cooldownTimer <= 0)
		{
			StartCoroutine("PlayChargeSound");
		}
		if (LMBtimedown > GetDelayTime() && LMBtimedown < GetOverheatTime() && cooldownTimer <= 0 && !isFiring)
		{
			if (GetStatLibrary().GetDetrizideBank() > devcost)
			{
				Debug.Log("Devourer cost = " + devcost);
				StartCoroutine(ReleaseTheBoys());
				isFiring = true;
				GetStatLibrary().SetDetrizideBank(GetStatLibrary().GetDetrizideBank() - devcost);
				devcost += 500;				
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

	IEnumerator ReleaseTheBoys()
	{
		for (int i = 0; i < projCount; i++)
		{
			yield return new WaitForSeconds(firerate);
			FireProjectile();
			GetASO().PlayOneShot(fireclip);
		}
		ResetState();
		FindObjectOfType<WeaponManager>().GetAnimator().SetBool("IsShooting", false);
	}

}
