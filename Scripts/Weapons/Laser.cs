using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject hitblast = default;
    [SerializeField] private GameObject muzzleflash = default;
    [SerializeField] private AudioClip shot = default;
    [SerializeField] private float laserdecay = 4.0f;
    private LineRenderer lr;
    private GameObject parent;
    private GameObject mouth;
    private Transform pjtsf;
    bool isparentplayermouth;
    public GameObject GetParent() { return parent; }
    public void SetParent(GameObject value) { parent = value; }
    public float GetLaserDecay() { return laserdecay; }

    //for Void Cannon
    Vector3 hitpoint;
    public Vector3 GetHitPoint() { return hitpoint; }
    public void SetHitPoint(Vector3 value) { hitpoint = value; }
    public float GetShotLength() { return shot.length; }

    void Start()
    {
        pjtsf = GameObject.Find("Projectiles").transform;
        mouth = GameObject.Find("Mouth");
        if (parent == mouth)
            isparentplayermouth = true;
        else
            isparentplayermouth = false;
        if (isparentplayermouth)
        {
            parent.GetComponentInParent<PlayerWeapon>().GetASO().PlayOneShot(shot);
            parent.GetComponentInParent<PlayerWeapon>().GetASO().PlayOneShot(shot, 0.3f);
        }
        lr = GetComponent<LineRenderer>();
        StartCoroutine(DestroySelf());
    }

    void LateUpdate()
    {
        if (mouth)
        {
            GameObject mf = Instantiate(muzzleflash, mouth.transform.position, Quaternion.identity, pjtsf);
            Destroy(mf, 0.2f);

            lr.SetPosition(0, parent.transform.position);

            // Bit shift the index of the bullets + triggerground layer to get a bit mask
            int layerMask = 1 << LayerMask.NameToLayer("Bullets") | 1 << LayerMask.NameToLayer("TriggerGround");

            // This would cast rays only against colliders in the bullets + triggerground layer.
            // But instead we want to collide against everything except this layer. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(parent.transform.position, parent.transform.forward, out hit, 50000f, layerMask))
            {
                if (hit.collider)
                {
                    hitpoint = hit.point;
                    lr.SetPosition(1, hit.point);
                    if(hitblast != null)
                    {
                        GameObject hb = Instantiate(hitblast, hit.point, Quaternion.identity, parent.transform);
                        hb.name = hb.name;
                        if (hb.GetComponent<Bullet>())
                            hb.GetComponent<Bullet>().SetParent(mouth);
                    }
                }
            }
            else
            {
                lr.SetPosition(1, parent.transform.forward * 50000f);
                hitpoint = parent.transform.position;
            }
        }
    }

    public static bool CheckLineOfSight(GameObject thisunit)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Bullets") | 1 << LayerMask.NameToLayer("TriggerGround");
        // collide with everything except above layers
        layerMask = ~layerMask;
        RaycastHit hit;


        if (Physics.Raycast(thisunit.transform.position, thisunit.transform.forward, out hit, 50000f, layerMask))
            foreach (string str in Unit.ConfirmEnemies(thisunit))
            {
                if (hit.transform.tag == str)
                    return true; 
            }
        return false;
    }

    public virtual IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(laserdecay);
        if(FindObjectOfType<ChargingWeapon>() && isparentplayermouth)
        {
            FindObjectOfType<ChargingWeapon>().ResetState();
            FindObjectOfType<WeaponManager>().GetAnimator().SetBool("IsShooting", false);
        }
        Destroy(gameObject);

    }
}