using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
	private UIController uic;
	private float damageTimeStamp = 0f;
	private bool hasDedSoundPlayed = false;
	private int health;
	[SerializeField] private int detrizideGains = default;
	[SerializeField] private GameObject deathAnim = default;
	[SerializeField] private GameObject souls = default;
	private SoundLibrary sl;
	private SpawnController sc;
	private AudioClip dead;
	private AudioClip hurt;

	[SerializeField] private int maxHealth = default;
	[SerializeField] private float damageIFrameRate = default;

	private List<Material> originalMaterials;
	[SerializeField] private Material flickerMaterial = default;

	public int GetHealth() { return health; }
	public int GetMaxHealth() { return maxHealth; }
	public void SetHealth(int value) { health = value; }
	public void SetMaxHealth(int value) { maxHealth = value; }

	public AudioClip GetDeadClip() { return dead; }
	public AudioClip GetHurtClip() { return hurt; }
	public void SetDeadClip(AudioClip value) { dead = value; }
	public void SetHurtClip(AudioClip value) { hurt = value; }

	void Start()
	{
		sl = FindObjectOfType<SoundLibrary>();
		sc = FindObjectOfType<SpawnController>();
		uic = FindObjectOfType<UIController>();
		health = maxHealth;
		saveOriginalMaterials();
	}

	void Update()
	{
		if (gameObject.GetComponent<Player>())
		{
			Image border = GameObject.FindGameObjectWithTag("HealthBorder").GetComponent<Image>();
			Color newcolor = border.color;
			newcolor.a = (1.0f - ((float)health / (float)maxHealth));
			border.color = newcolor;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.CompareTag("FriendlyProjectile") && gameObject.CompareTag("Enemies")) ||
			(other.CompareTag("FriendlyProjectile") && gameObject.CompareTag("NecroSpawn")) ||
			(other.CompareTag("FriendlyProjectile") && gameObject.CompareTag("LichSpawn")) ||
			(other.CompareTag("EnemyProjectile") && (gameObject.CompareTag("Friendlies") || gameObject.CompareTag("Player"))))
			HandleDamage(other);
	}

	private void saveOriginalMaterials()
	{
		originalMaterials = new List<Material>();
		MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer boi in renderers)
			originalMaterials.Add(boi.material);
	}

	public void HandleDamage(Collider co)
	{
		SoundLibrary.ResetPitch(GetComponent<AudioSource>());


		if (!IsDead())
		{
			if (Time.time >= damageTimeStamp)
			{
				if (!gameObject.GetComponent<Player>())
					SoundLibrary.varySoundPitch(GetComponent<AudioSource>(), 0.05f);
				Bullet projGo = co.GetComponentInParent<Bullet>();
				SetHealth(GetHealth() - projGo.GetDamage());
				damageTimeStamp = Time.time + damageIFrameRate;

				if (!gameObject.CompareTag("Player") && gameObject.GetComponent<Unit>())
				{
					GetComponent<AudioSource>().PlayOneShot(hurt);
					StartCoroutine(DamageFlicker());
				}
				else if (gameObject.CompareTag("Player") && gameObject.GetComponent<Unit>())
				{
					GetComponent<AudioSource>().PlayOneShot(sl.ChoosePlayerHurt());
				}
				HandleDeath(co);
			}
		}

	}

	IEnumerator DamageFlicker()
	{
		foreach (MeshRenderer mat in gameObject.GetComponentsInChildren<MeshRenderer>())
		{
			mat.material = flickerMaterial;
		}
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < originalMaterials.Count; i++)
		{
			gameObject.GetComponentsInChildren<MeshRenderer>()[i].material = originalMaterials[i];
		}
	}

	private void HandleDeath(Collider col)
	{
		if (IsDead())
		{
			//set layer to corpses layer
			foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
			{
				child.gameObject.layer = 10;
			}
			if (!gameObject.CompareTag("Player"))
			{
				GameObject anim = Instantiate(deathAnim, transform.position, Quaternion.identity);
				anim.GetComponentInChildren<ParticleSystem>();
				Destroy(anim, anim.GetComponentInChildren<ParticleSystem>().main.duration);
			}
			if (gameObject.CompareTag("Enemies") || gameObject.CompareTag("NecroSpawn") || gameObject.CompareTag("LichSpawn"))
			{
				Destroy(gameObject);
				GameObject soulinst = Instantiate(souls, transform.position, gameObject.transform.rotation);
				sc.SetTimeSinceLastKill(0);

			}
			if (gameObject.CompareTag("Friendlies"))
			{
				Destroy(gameObject);
				Player.CountFriends("Friendlies");
			}
			if (gameObject.CompareTag("Player"))
			{
				DisableWeaponRenderers();

				RawImage image = GameObject.FindGameObjectWithTag("DeadImage").GetComponent<RawImage>();
				Color newcolor = new Color();
				while (image.color.a < 0.8f)
				{
					newcolor = image.color;
					newcolor.a += 0.01f;
					image.color = newcolor;
				}
				if (sc.GetGameActive())
				{
					uic.DisplayStats();
					uic.DisplayHighScore();
				}

			}
			if (!gameObject.CompareTag("Player"))
			{
				gameObject.layer = 10;
				StatLibrary sl = FindObjectOfType<StatLibrary>();
				//Debug.Log("sl.GetDetrizide() = " + sl.GetDetrizide());
				//Debug.Log("col.gameObject.GetComponentInParent<Bullet>().GetParent().GetComponentInChildren<Gauntlets>() = " + col.gameObject.GetComponentInParent<Bullet>().GetParent().GetComponentInChildren<Gauntlets>());
				sl.SetDetrizide(sl.GetDetrizide() + detrizideGains);

				if (col.gameObject.GetComponentInParent<Bullet>())
					if (col.gameObject.GetComponentInParent<Bullet>().GetParent())
						if (col.gameObject.GetComponentInParent<Bullet>().GetParent().GetComponentInChildren<Gauntlets>())
						{
							sl.SetDetrizide(sl.GetDetrizide() + detrizideGains);
						}

	
			}

			if (!hasDedSoundPlayed)
			{
				SoundLibrary.ResetPitch(GameObject.Find("Environment").GetComponent<AudioSource>());
				SoundLibrary.varySoundPitch(GameObject.Find("Environment").GetComponent<AudioSource>(), 0.2f);

				GameObject.Find("Environment").GetComponent<AudioSource>().clip = dead;
				GameObject.Find("Environment").GetComponent<AudioSource>().PlayOneShot(dead);
				hasDedSoundPlayed = true;
			}
		}
	}

	private static void DisableWeaponRenderers()
	{
		if (FindObjectsOfType<PlayerWeapon>().Length > 0)
		{
			foreach (PlayerWeapon weapon in FindObjectsOfType<PlayerWeapon>())
			{
				foreach (MeshRenderer renderer in weapon.gameObject.GetComponentsInChildren<MeshRenderer>())
				{
					renderer.enabled = false;
				}
			}
		}
		if (FindObjectOfType<Gauntlets>())
			foreach (SkinnedMeshRenderer renderer in FindObjectOfType<Gauntlets>().gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				renderer.enabled = false;
			}
	}

	public bool IsDead()
	{
		if (GetHealth() <= 0)
			return true;
		return false;
	}

	public static Transform GetClosestEnemy(GameObject thisUnit)
	{
		if (thisUnit)
		{
			var enemylist = new List<GameObject>();
			GameObject[] enemyArray = null;
			Transform closest = null;
			float minDist = Mathf.Infinity;
			Vector3 currentPos = thisUnit.transform.position;
			enemyArray = FillEnemyArray(thisUnit, enemylist, enemyArray);
			Transform[] enemTransArray = new Transform[enemyArray.Length];

			for (int i = 0; i < enemyArray.Length; i++)
			{
				enemTransArray[i] = enemyArray[i].transform;
			}

			foreach (Transform enemy in enemTransArray)
			{
				bool isenemydead = false;
				if (enemy && enemy.GetComponent<Unit>())
					isenemydead = enemy.GetComponent<Unit>().IsDead();

				float dist = Vector3.Distance(enemy.position, currentPos);

				if ((dist < minDist) && enemy && enemy.GetComponent<Unit>() && !isenemydead)
				{
					closest = enemy;

					minDist = dist;
				}
			}
			return closest;
		}
		else
			Debug.LogError("No Source Gameobject to check from. Gameobject is null.");
			return null;
		//return FindObjectOfType<PlayerShooting>().transform;
	}

	public static GameObject ChooseRandomEnemy(GameObject thisUnit)
	{
		GameObject[] enemylist = GetAllEnemies(thisUnit);
		int random = UnityEngine.Random.Range(0, enemylist.Length);
		try
		{
			return enemylist[random];
		}
		catch (IndexOutOfRangeException e)
		{
			Debug.Log("Null enemy in array.");
			Debug.Log(e);
			return null;
		}		
	}

	public static GameObject[] GetAllEnemies(GameObject thisUnit)
	{
		var enemylist = new List<GameObject>();
		GameObject[] enemyArray = null;
		enemyArray = FillEnemyArray(thisUnit, enemylist, enemyArray);
		return enemyArray;
	}

	public static GameObject[] FillEnemyArray(GameObject thisUnit, List<GameObject> enemylist, GameObject[] enemyArray)
	{
		if (thisUnit.CompareTag("Player"))
		{
			enemylist.AddRange(GameObject.FindGameObjectsWithTag("Enemies"));
			enemylist.AddRange(GameObject.FindGameObjectsWithTag("NecroSpawn"));
			enemylist.AddRange(GameObject.FindGameObjectsWithTag("LichSpawn"));
			enemyArray = enemylist.ToArray();
		}
		else if (thisUnit.CompareTag("Friendlies"))
		{
			enemylist.AddRange(GameObject.FindGameObjectsWithTag("Enemies"));
			enemylist.AddRange(GameObject.FindGameObjectsWithTag("NecroSpawn"));
			enemylist.AddRange(GameObject.FindGameObjectsWithTag("LichSpawn"));
			enemyArray = enemylist.ToArray();
		}
		else if (thisUnit.CompareTag("Enemies") || thisUnit.CompareTag("NecroSpawn") || thisUnit.CompareTag("LichSpawn"))
		{
			enemylist.AddRange(GameObject.FindGameObjectsWithTag("Friendlies"));
			if (GameObject.FindGameObjectWithTag("Player"))
				enemylist.AddRange(GameObject.FindGameObjectsWithTag("Player"));
			enemyArray = enemylist.ToArray();
		}
		return enemyArray;
	}

	public static List<string> ConfirmEnemies(GameObject thisUnit)
	{
		List<string> enemylist = new List<string>();
		if (thisUnit.CompareTag("Player"))
		{
			enemylist.Add("Enemies");
			enemylist.Add("NecroSpawn");
			enemylist.Add("LichSpawn");
		}
		else if (thisUnit.CompareTag("Friendlies"))
		{
			enemylist.Add("Enemies");
			enemylist.Add("NecroSpawn");
			enemylist.Add("LichSpawn");
		}
		else if (thisUnit.CompareTag("Enemies") || thisUnit.CompareTag("NecroSpawn") || thisUnit.CompareTag("LichSpawn"))
		{
			enemylist.Add("Friendlies");
			enemylist.Add("Player");
		}
		return enemylist;
	}
}


