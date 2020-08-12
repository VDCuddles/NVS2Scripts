using System.Collections;
using UnityEngine;

public class Shotgun : PlayerWeapon
{
	[SerializeField] private int bulletNumber = 15;
	[SerializeField] private AudioClip shotgunSound = default;
	[SerializeField] private AudioClip reloadSound = default;
	private Animator animator;
	private GameObject player;

	public override void Start()
	{
		if (GetComponent<Animator>())
			animator = GetComponent<Animator>();
		SetCamera(FindObjectOfType<Camera>());
		SetUIC(FindObjectOfType<UIController>());
		player = FindObjectOfType<Player>().gameObject;
		SetUnit(GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>());
		SetSL(FindObjectOfType<StatLibrary>());
		SetASO(FindObjectOfType<Player>().GetComponent<AudioSource>());
	}

	public override void FireProjectile()
	{
		if (gameObject.name == "Shotgun")
			StartCoroutine(ReloadAnimation());

		Vector3 projPosition = GetMouth().transform.position;
		for (int i = 0; i < bulletNumber; i++)
		{
			GameObject projectileInstance = Instantiate(GetProjPref(), projPosition, Quaternion.identity, GetPjtsf());
			projectileInstance.name = GetProjPref().name;
			projectileInstance.GetComponent<Bullet>().SetParent(player);
			aimProjectile(projectileInstance);
		}
		GameObject flash = Instantiate(GetMuzzleFlash(), transform.position, Quaternion.identity, GetMouth().transform);
		aimProjectile(flash);
		flash.transform.Rotate(Vector3.forward, Random.Range(0, 360));

		Destroy(flash, 0.05f);
		GetASO().PlayOneShot(shotgunSound);
		SetSRTS(Time.time + GetShootRate());
	}

	IEnumerator ReloadAnimation()
	{
		yield return new WaitForSeconds(0.5f);
		animator.SetTrigger("Reload"); 
		yield return new WaitForSeconds(0.2f);
		GetASO().PlayOneShot(reloadSound);
	}
}
