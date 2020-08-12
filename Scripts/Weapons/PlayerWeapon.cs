using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
	[SerializeField] private GameObject projectilePrefab = default;
	[SerializeField] private GameObject mouth = default;
	[SerializeField] private AudioClip outOfAmmo = default;
	[SerializeField] private int ammoCount = default;
	[SerializeField] private bool usesAmmo = default;
	[SerializeField] private GameObject muzzleFlash = default;

	private AudioSource aso;
	private Transform pjtsf;
	public Transform GetPjtsf() { return pjtsf; }
	private bool outOfAmmoClipPlaying = false;
	private bool ammotutdisplayed = false;
	private bool readytofire;

	public GameObject GetProjectilePrefab() { return projectilePrefab; }
	public void SetProjectilePrefab(GameObject value) { projectilePrefab = value; }
	public bool GetReadyToFire() { return readytofire; }
	public void SetReadyToFire(bool value) { readytofire = value; }

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
	private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

	StatLibrary sl;
	Unit unit;
	private UIController uic;

	private float shootRateTimeStamp = 0f;
	[SerializeField] private float shootRate = default;

	public void SetUIC(UIController value) { uic = value; }
	public void SetSRTS(float value) { shootRateTimeStamp = value; }
	public void SetUnit(Unit value) { unit = value; }
	public void SetSL(StatLibrary value) { sl = value; }
	public void SetASO(AudioSource value) { aso = value; }
	public void SetCamera(Camera value) { camera = value; }

	public Unit GetUnit() { return unit; }
	public GameObject GetMuzzleFlash() { return muzzleFlash; }
	public GameObject GetMouth() { return mouth; }
	public float GetShootRate() { return shootRate; }
	public float GetSRTS() { return shootRateTimeStamp; }
	public AudioSource GetASO() { return aso; }
	public GameObject GetProjPref() { return projectilePrefab; }
	public Camera GetCamera() { return camera; }
	public StatLibrary GetStatLibrary() { return sl; }

	public virtual void Start()
	{

		camera = FindObjectOfType<Camera>();
		uic = FindObjectOfType<UIController>();
		pjtsf = GameObject.Find("Projectiles").transform;
		unit = FindObjectOfType<Player>().GetComponent<Unit>();
		sl = FindObjectOfType<StatLibrary>();
		aso = FindObjectOfType<Player>().GetComponent<AudioSource>();
	}

	public virtual void Update()
	{
		readytofire = Time.time > shootRateTimeStamp && ammoCount > 0 || Time.time > shootRateTimeStamp && !usesAmmo;
	}

	public virtual void Shoot()
	{
		if (readytofire && !unit.gameObject.GetComponent<Player>().GetIsCasting())
		{
			FireProjectile();
			ammoCount--;
		}
		if (ammoCount <= 0 && usesAmmo && !outOfAmmoClipPlaying)
		{
			if (!ammotutdisplayed)
			{
				uic.DisplayTutorial("ammo");
				ammotutdisplayed = true;
			}
			StartCoroutine("PlayOOAClip");
		}

	}

	IEnumerator PlayOOAClip()
	{
		outOfAmmoClipPlaying = true;
		aso.PlayOneShot(outOfAmmo);
		yield return new WaitForSeconds(outOfAmmo.length);
		outOfAmmoClipPlaying = false;
	}

	public virtual void FireProjectile()
	{

		Vector3 projPosition = mouth.transform.position;
		GameObject projectileInstance = Instantiate(projectilePrefab, projPosition, Quaternion.identity, pjtsf);
		projectileInstance.name = projectilePrefab.name;
		if (projectileInstance.GetComponent<Bullet>())
			projectileInstance.GetComponent<Bullet>().SetParent(FindObjectOfType<Player>().gameObject);
		else if (projectileInstance.GetComponent<Laser>())
			projectileInstance.GetComponent<Laser>().SetParent(mouth);

		shootRateTimeStamp = Time.time + shootRate;
		aimProjectile(projectileInstance);

		if (GetMuzzleFlash())
		{
			GameObject flash = Instantiate(GetMuzzleFlash(), transform.position, Quaternion.identity, GetMouth().transform);
			aimProjectile(flash);
			flash.transform.Rotate(Vector3.forward, Random.Range(0, 360));
			Destroy(flash, 0.05f);
		}

	}

	public void aimProjectile(GameObject projectile)
	{
		projectile.transform.forward = camera.transform.forward;
	}
}
