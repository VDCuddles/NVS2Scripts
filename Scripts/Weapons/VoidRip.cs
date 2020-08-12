using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidRip : Laser
{
    [SerializeField] private GameObject singularity = default;

    public override IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(GetShotLength());
        if (FindObjectOfType<ChargingWeapon>())
        {
            FindObjectOfType<ChargingWeapon>().ResetState();
            FindObjectOfType<WeaponManager>().GetAnimator().SetBool("IsShooting", false);
        }
        SpawnSingularity();
    }

    private void SpawnSingularity()
    {
        GameObject hb = Instantiate(singularity, GetHitPoint() , Quaternion.identity);
        hb.name = hb.name;
        if (hb.GetComponent<Bullet>())
            hb.GetComponent<Bullet>().SetParent(GetParent());
        Destroy(gameObject);

    }
}
