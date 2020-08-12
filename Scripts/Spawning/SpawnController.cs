using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
	[SerializeField] private float fastModeFactor = 1.5f;
	[SerializeField] private int maxspawns = 20;
	[SerializeField] private float timeTillAdvent = 30.0f;
	private bool fastmode;

	private bool isEGLActive = false;
	private bool gameActive = false;
	public const string epath = "spawns";
	public const string eglpath = "EGLspawns";
	public const string wpath = "weaponspawns";
	public const string papath = "passivespawns";
	public const string poupath = "powerupspawns";
	private float timeSinceLastKill = 0.0f;
	private bool adventspawned = false;
	private float spawnTimestamp = 0;
	private float puTimestamp = 0;
	private float EGLMultiplier = 1;
	private TextMeshPro TMPweaponnum;
	private Unit playerUnit;
	private SpawnsetEnemies es;
	private SpawnsetEnemies egl;
	private WeaponSpawnset ws;
	private PowerupSpawnset pus;
	private PassiveSpawnset pas;
	private StatLibrary statl;
	private List<Spawn> spawnList;
	private List<Spawn> EGLSpawnList;
	private List<Spawn> usedSpawnList;
	private List<Spawn> usedEGLSpawnList;
	private List<Weapon> weaponList;
	private List<Weapon> ownedWeaponList = new List<Weapon>();
	private List<PassiveBonus> paList;
	private List<PassiveBonus> ownedpaList = new List<PassiveBonus>();
	private List<Powerup> pulist;
	private List<Powerup> ownedpuList = new List<Powerup>();
	private Vector3 weaponPickupPos;
	private Vector3 passivePickupPos;
	private Vector3 powerupPickupPos;

	public List<Weapon> GetOwnedWeapons() { return ownedWeaponList; }
	public bool GetGameActive() { return gameActive; }
	public void SetGameActive(bool value) { gameActive = value; }
	private AudioSource aso;
	private AudioClip pickupSpawned;

	public float GetTimeSinceLastKill() { return timeSinceLastKill; }
	public void SetTimeSinceLastKill(float value) { timeSinceLastKill = value; }

	[SerializeField] private GameObject weaponSpawnAnim = default;
	[SerializeField] private GameObject enemyCollection = default;
	[SerializeField] private GameObject wbench = default;
	[SerializeField] private GameObject pabench = default;
	[SerializeField] private GameObject pubench = default;

	[SerializeField] private int maxSpawnPoints = default;
	[SerializeField] private float EGLShrinkrate = default;
	[SerializeField] private float EGLMinimum = default;
	//enemies
	[SerializeField] private GameObject revenant = default;
	[SerializeField] private GameObject eliterevenant = default;
	[SerializeField] private GameObject juggernaut = default;
	[SerializeField] private GameObject dreadnought = default;
	[SerializeField] private GameObject necromancer = default;
	[SerializeField] private GameObject lich = default;
	[SerializeField] private GameObject lacerator = default;
	//guns
	[SerializeField] private GameObject bonebag = default;
	[SerializeField] private GameObject scythes = default;
	[SerializeField] private GameObject shotgun = default;
	[SerializeField] private GameObject gauntlets = default;
	[SerializeField] private GameObject snubnose = default;
	[SerializeField] private GameObject reaver = default;
	[SerializeField] private GameObject devourer = default;
	[SerializeField] private GameObject voidcannon = default;
	[SerializeField] private GameObject crucifier = default;
	public void SetBoneBag(GameObject value) { bonebag = value; }

	//gun pickups
	[SerializeField] private GameObject shotgunpickup = default;
	[SerializeField] private GameObject gauntletspickup = default;
	[SerializeField] private GameObject snubnosepickup = default;
	[SerializeField] private GameObject crucifierpickup = default;
	[SerializeField] private GameObject reaverpickup = default;
	[SerializeField] private GameObject devourerpickup = default;
	[SerializeField] private GameObject voidcannonpickup = default;
	//other pickups
	[SerializeField] private GameObject quaddamagepickup = default;
	[SerializeField] private GameObject summonplus2pickup = default;
	[SerializeField] private GameObject scythepickup = default;
	[SerializeField] private GameObject healthpickup = default;

	void Start()
	{
		TMPweaponnum = GameObject.FindGameObjectWithTag("TMPweaponnum").GetComponent<TextMeshPro>();
		weaponPickupPos = GameObject.Find("WBPickupPos").transform.position;
		passivePickupPos = GameObject.Find("PAPUPos").transform.position;
		powerupPickupPos = GameObject.Find("PUPUPos").transform.position;
		playerUnit = FindObjectOfType<Player>().GetComponent<Unit>();
		aso = FindObjectOfType<Player>().GetComponent<AudioSource>();
		es = SpawnsetEnemies.Load(epath);
		egl = SpawnsetEnemies.Load(eglpath);
		ws = WeaponSpawnset.Load(wpath);
		pus = PowerupSpawnset.Load(poupath);
		pas = PassiveSpawnset.Load(papath);
		statl = FindObjectOfType<StatLibrary>();
		spawnList = es.spawns;
		weaponList = ws.weapons;
		paList = pas.passives;
		pulist = pus.powerups;
		EGLSpawnList = egl.spawns;
		usedSpawnList = new List<Spawn>();
		usedEGLSpawnList = new List<Spawn>();
		pickupSpawned = FindObjectOfType<SoundLibrary>().GetPickupSpawned();
		fastmode = playerUnit.gameObject.GetComponent<Player>().fastmode;

	}
	private void Update()
	{
		CheckSpawnTiming();
		CheckWeaponReady();
		CheckPowerupTiming();
		CheckPassiveReady();
		if (gameActive)
			timeSinceLastKill += Time.deltaTime;
		CheckLichAdvent();
	}

	private void CheckLichAdvent()
	{
		if (timeSinceLastKill > timeTillAdvent && !adventspawned && gameActive)
		{
			distributeAndSpawn(7, "Lich");
			adventspawned = true;
		}
	}

	void CheckPowerupTiming()
	{
		if (gameActive)
		{
			foreach (Powerup powerup in pulist)
			{
				if (Time.timeSinceLevelLoad > puTimestamp && !ownedpuList.Contains(powerup) && !playerUnit.IsDead() && !pubench.GetComponentInChildren<Pickup>())
				{
					spawnPowerup(powerup);
					ownedpuList.Add(powerup);
					puTimestamp = Time.timeSinceLevelLoad + powerup.spawnDelay;
				}
			}
			if (pulist.Count == ownedpuList.Count)
				ownedpuList.Clear();
		}
	}

	void spawnPowerup(Powerup powerup)
	{
		if (providePickup(powerup.spawnName))
		{
			GameObject pickup = Instantiate(providePickup(powerup.spawnName), powerupPickupPos, Quaternion.identity, pubench.transform);
			GameObject anim = Instantiate(weaponSpawnAnim, powerupPickupPos, Quaternion.identity, pubench.transform);
			Destroy(anim, 3.0f);
			pickup.name = powerup.spawnName;
			aso.PlayOneShot(pickupSpawned);
		}
	}

	void CheckPassiveReady()
	{
		foreach (PassiveBonus pb in paList)
		{
			if (statl.GetDetrizideBank() >= pb.spawnCost && !ownedpaList.Contains(pb) && !pabench.GetComponentInChildren<Pickup>())
			{
				ownedpaList.Add(pb);
				statl.SetDetrizideBank(statl.GetDetrizideBank() - pb.spawnCost);
				//make the pickup
				GameObject pickup = Instantiate(providePickup(pb.spawnName), passivePickupPos, Quaternion.identity, pabench.transform);
				GameObject anim = Instantiate(weaponSpawnAnim, passivePickupPos, Quaternion.identity, pabench.transform);
				Destroy(anim, 3.0f);
				pickup.name = pb.spawnName;
				if (statl.GetDetrizideBank() < 30000)
					aso.PlayOneShot(pickupSpawned);
			}
		}

	}

	void CheckWeaponReady()
	{
		//Debug.Log(statl.GetDetrizideBank());
		foreach (Weapon weapon in weaponList)
		{
			if (statl.GetDetrizideBank() >= weapon.spawnCost && !ownedWeaponList.Contains(weapon) && !wbench.GetComponentInChildren<Pickup>())
			{
				ownedWeaponList.Add(weapon);
				statl.SetDetrizideBank(statl.GetDetrizideBank() - weapon.spawnCost);
				//make the pickup
				TMPweaponnum.text = UIController.MakeRoman(weapon.defaultKey);
				GameObject pickup = Instantiate(providePickup(weapon.spawnName), weaponPickupPos, Quaternion.identity, wbench.transform);
				GameObject anim = Instantiate(weaponSpawnAnim, weaponPickupPos, Quaternion.identity, wbench.transform);
				Destroy(anim, 3.0f);
				pickup.name = weapon.spawnName;
				if (statl.GetDetrizideBank() < 30000)
					aso.PlayOneShot(pickupSpawned);
			}
		}
	}

	private void CheckSpawnTiming()
	{
		//Debug.Log("enemy count = " + enemyCollection.transform.childCount);
		if (gameActive && enemyCollection.transform.childCount < maxspawns)
		{
			if (!isEGLActive)
			{
				if (spawnList.Count == usedSpawnList.Count)
				{
					isEGLActive = true;
				}
				foreach (Spawn spawn in spawnList)
				{
					if (Time.timeSinceLevelLoad > spawnTimestamp && !usedSpawnList.Contains(spawn) && !playerUnit.IsDead())
					{
						distributeAndSpawn(spawn);
						usedSpawnList.Add(spawn);
						spawnTimestamp = Time.timeSinceLevelLoad + (fastmode ? spawn.spawnDelay / fastModeFactor : spawn.spawnDelay);
					}
				}
			}
			else
			{
				if (EGLSpawnList.Count == usedEGLSpawnList.Count)
				{
					usedEGLSpawnList.Clear();
					if (EGLMultiplier > EGLMinimum)
						EGLMultiplier *= EGLShrinkrate;

				}
				foreach (Spawn spawn in EGLSpawnList)
				{
					if (Time.timeSinceLevelLoad > spawnTimestamp && !usedEGLSpawnList.Contains(spawn) && !playerUnit.IsDead())
					{
						distributeAndSpawn(spawn);
						usedEGLSpawnList.Add(spawn);
						spawnTimestamp = Time.timeSinceLevelLoad + ((fastmode ? spawn.spawnDelay / fastModeFactor : spawn.spawnDelay) * EGLMultiplier);
					}
				}
			}
		}
	}

	void distributeAndSpawn(Spawn spawn)
	{
		List<SpawnPoint> possiblePoints = new List<SpawnPoint>(FindObjectsOfType<SpawnPoint>());
		List<SpawnPoint> usedPoints = new List<SpawnPoint>();
		int randomNr = Random.Range(0, possiblePoints.Count);
		if (spawn.spawnCount > maxSpawnPoints)
			spawn.spawnCount = maxSpawnPoints;
		for (int i = 0; i < spawn.spawnCount; i++)
		{
			while (possiblePoints[randomNr] == null || usedPoints.Contains(possiblePoints[randomNr]))
			{
				randomNr = Random.Range(0, possiblePoints.Count);
			}
			possiblePoints[randomNr].SpawnUnit(provideGameObject(spawn.spawnUnit));
			usedPoints.Add(possiblePoints[randomNr]);
		}

	}

	void distributeAndSpawn(int count, string spawn)
	{
		List<SpawnPoint> possiblePoints = new List<SpawnPoint>(FindObjectsOfType<SpawnPoint>());
		List<SpawnPoint> usedPoints = new List<SpawnPoint>();
		int randomNr = Random.Range(0, possiblePoints.Count);
		if (count > maxSpawnPoints)
			count = maxSpawnPoints;
		for (int i = 0; i < count; i++)
		{
			while (possiblePoints[randomNr] == null || usedPoints.Contains(possiblePoints[randomNr]))
			{
				randomNr = Random.Range(0, possiblePoints.Count);
			}
			possiblePoints[randomNr].SpawnUnit(provideGameObject(spawn));
			usedPoints.Add(possiblePoints[randomNr]);
		}

	}

	public GameObject provideGameObject(string name)
	{
		switch (name)
		{
			//enemies
			case "Revenant":
				return revenant;
			case "EliteRevenant":
				return eliterevenant;
			case "Juggernaut":
				return juggernaut;
			case "Dreadnought":
				return dreadnought;
			case "Necromancer":
				return necromancer;
			case "Lich":
				return lich;
			case "Lacerator":
				return lacerator;
			//guns
			case "BoneBag":
				return bonebag;
			case "Scythes":
				return scythes;
			case "Gauntlets":
				return gauntlets;
			case "Snubnose":
				return snubnose;
			case "Shotgun":
				return shotgun;
			case "Reaver":
				return reaver;
			case "Devourer":
				return devourer;
			case "Void Cannon":
				return voidcannon;
			case "Crucifier":
				return crucifier;
			default:
				return null;
		}
	}
	public GameObject providePickup(string name)
	{
		switch (name)
		{
			case "Shotgun":
				return shotgunpickup;
			case "Snubnose":
				return snubnosepickup;
			case "Gauntlets":
				return gauntletspickup;
			case "Reaver":
				return reaverpickup;
			case "Devourer":
				return devourerpickup;
			case "Void Cannon":
				return voidcannonpickup;
			case "Crucifier":
				return crucifierpickup;
			case "QuadDamage":
				return quaddamagepickup;
			case "SummonPlus2":
				return summonplus2pickup;
			case "Health":
				return healthpickup;
			case "Scythe":
				return scythepickup;
			default:
				return null;
		}
	}

}