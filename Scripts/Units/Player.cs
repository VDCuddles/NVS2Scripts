using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	//private GameObject resUnit;
	private GameObject currentWeapon;
	private string previousWeapon;
	private WeaponBench wb;
	private Unit unit;
	private Altar altar;
	private SoundLibrary sl;
	private StatLibrary statl;
	private AudioSource aso;
	private Animator fwanimator;
	private InputController ic;
	private SpawnController sc;
	private List<GameObject> ownedWeaponGOs;
	private string nextWeapon = "";
	private WeaponManager weaponmanager;

	private bool isSwapping = false;
	public void SetIsSwapping(bool value) { isSwapping = value; }
	public List<GameObject> GetPlayerOwnedWeapons() { return ownedWeaponGOs; }
	public bool GetIsSwapping() { return isSwapping; }

	private bool isCasting = false;
	private bool spellavailable = false;
	private bool healavailable = false;
	private bool summonavailable = false;
	private bool bankavailable = false;
	public bool GetIsCasting() { return isCasting; }
	public void SetIsCasting(bool value) { isCasting = value; }
	[SerializeField] private Detrilight detrilight = default;
	[SerializeField] private GameObject spelleffect = default;
	[SerializeField] private GameObject spendtracer = default;
	[SerializeField] private Transform spellpos = default;
	[SerializeField] private Animator gunpositionanim = default;
	[SerializeField] private Animator lefthandanim = default;
	[SerializeField] private Animator casterhandbody = default;

	private int damageModifier = 1;
	public void SetDamageModifier(int value) { damageModifier = value; }
	public int GetDamageModifier() { return damageModifier; }

	public GameObject GetCurrentWeapon() { return currentWeapon; }

	[SerializeField] public bool fastmode = false;

	[SerializeField] public static int costlimit = 250;
	private float temphealthcost;
	private static int healthcost;
	private static int rescost;
	public static int GetHealthCost() { return healthcost; }
	public static int GetResCost() { return rescost; }

	void Start()
	{
		wb = FindObjectOfType<WeaponBench>();
		fwanimator = GameObject.FindGameObjectWithTag("FadeWall").GetComponent<Animator>();
		altar = FindObjectOfType<Altar>();
		//resUnit = altar.GetResUnit();
		ic = FindObjectOfType<InputController>();
		weaponmanager = FindObjectOfType<WeaponManager>();
		sc = FindObjectOfType<SpawnController>();
		aso = gameObject.GetComponent<AudioSource>();
		sl = FindObjectOfType<SoundLibrary>();
		statl = FindObjectOfType<StatLibrary>();
		unit = gameObject.GetComponent<Unit>();
		unit.SetDeadClip(sl.GetPlayerDead());
		ownedWeaponGOs = new List<GameObject>();
		previousWeapon = FindObjectOfType<PlayerWeapon>().name;
		ownedWeaponGOs.Add(sc.provideGameObject("BoneBag"));

	}

	private void Update()
	{
		CalculateCosts();

		if (FindObjectsOfType<PlayerWeapon>().Length > 1)
			currentWeapon = GameObject.Find("Snubnose");
		else
			currentWeapon = FindObjectOfType<PlayerWeapon>().gameObject;
		if (!GetComponent<Unit>().IsDead())
		{
			if (Input.GetKey(ic.Detrizide))
			{
				AnimateCast();
				CountFriends("Friendlies");
			}
			CheckSwapWeapon(currentWeapon);
		}
		if (Input.GetKeyDown(ic.Reset))
			fwanimator.SetTrigger("FadeToBlack");

		if (Input.GetKeyDown(ic.Exit))
		{
			fwanimator.SetTrigger("FadeToBlack");
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			SceneManager.LoadScene("Main Menu");
		}

		else if (Input.GetKeyDown(ic.CycleWeapons) && previousWeapon != currentWeapon.name)
			AnimateSwapWeapon(currentWeapon, previousWeapon);
		if (Input.GetKeyDown(ic.NextWeapon) && ownedWeaponGOs.Count > 1)
		{
			SwapToNextWeapon();
		}
	}

	private void AnimateCast()
	{
		if (!isSwapping && !isCasting && spellavailable)
		{
			isCasting = true;
			gunpositionanim.SetTrigger("SpellCast");
		}
	}

	public void RaiseAndCastLHand()
	{
		casterhandbody.SetTrigger("Raise");
	}

	private void SwapToNextWeapon()
	{
		CheckRemoveBoneBag();

		int index = 0;
		for (int i = 0; i < ownedWeaponGOs.Count; i++)
		{
			if (currentWeapon.name == ownedWeaponGOs[i].name)
			{
				int container = i;
				if (i == ownedWeaponGOs.Count - 1)
					container = 0;
				else
					container = i + 1;
				index = container;
			}
		}

		AnimateSwapWeapon(currentWeapon, ownedWeaponGOs[index].name);
	}

	public void restartGame()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
		SceneManager.SetActiveScene(scene);
	}

	private void CheckSwapWeapon(GameObject currentWeapon)
	{
		CheckRemoveBoneBag();

		if (Input.GetKey(ic.BoneBag) && checkWeaponOwned("Snubnose") && currentWeapon.name != "Snubnose")
			AnimateSwapWeapon(currentWeapon, "Snubnose");
		else if (Input.GetKey(ic.BoneBag) && currentWeapon.name != "BoneBag")
			AnimateSwapWeapon(currentWeapon, "BoneBag");
		if (Input.GetKey(ic.Shotgun) && checkWeaponOwned("Shotgun") && currentWeapon.name != "Shotgun")
			AnimateSwapWeapon(currentWeapon, "Shotgun");
		if (Input.GetKey(ic.Gauntlets) && checkWeaponOwned("Gauntlets") && currentWeapon.name != "Gauntlets")
			AnimateSwapWeapon(currentWeapon, "Gauntlets");
		if (Input.GetKey(ic.Reaver) && checkWeaponOwned("Reaver") && currentWeapon.name != "Reaver")
			AnimateSwapWeapon(currentWeapon, "Reaver");
		if (Input.GetKey(ic.Devourer) && checkWeaponOwned("Devourer") && currentWeapon.name != "Devourer")
			AnimateSwapWeapon(currentWeapon, "Devourer");
		if (Input.GetKey(ic.Crucifier) && checkWeaponOwned("Crucifier") && currentWeapon.name != "Crucifier")
			AnimateSwapWeapon(currentWeapon, "Crucifier");
		if (Input.GetKey(ic.VoidCannon) && checkWeaponOwned("Void Cannon") && currentWeapon.name != "Void Cannon")
			AnimateSwapWeapon(currentWeapon, "Void Cannon");
	}

	private void CheckRemoveBoneBag()
	{
		bool hasbag = false;
		GameObject bag = null;
		foreach (GameObject go in ownedWeaponGOs)
			if (go.name == "BoneBag")
			{
				hasbag = true;
				bag = go;
			}
				
		if (ownedWeaponGOs.Contains(sc.provideGameObject("Snubnose")) && hasbag)
		{
			ownedWeaponGOs.Remove(bag);
			ownedWeaponGOs.Remove(sc.provideGameObject("Snubnose"));
			List<GameObject> newlist = new List<GameObject>();
			newlist.Add(sc.provideGameObject("Snubnose"));
			foreach (GameObject go in ownedWeaponGOs)
			{
				newlist.Add(go);
			}
			ownedWeaponGOs = newlist;
		}
	}

	public void AnimateSwapWeapon(GameObject currentWeapon, string weaponName)
	{
		if (!isSwapping && !isCasting)
		{
			if (FindObjectsOfType<PlayerWeapon>().Length > 1)
				previousWeapon = "Snubnose";
			else
				previousWeapon = currentWeapon.name;
			nextWeapon = weaponName;
			weaponmanager.GetAnimator().SetTrigger("StartSwap");
			weaponmanager.GetAnimator().SetBool("IsShooting",false);
			isSwapping = true;
		}
	}

	public void SpawnWeapon()
	{
		if (!unit.IsDead())
		{
			if (nextWeapon == "Snubnose")
			{
				Destroy(currentWeapon);
				GameObject newWeapon = Instantiate(sc.provideGameObject(nextWeapon), FindObjectOfType<WeaponManager>().transform.position, FindObjectOfType<Camera>().transform.rotation, GameObject.Find("GunPosition").transform);
				GameObject newWeapon2 = Instantiate(sc.provideGameObject("BoneBag"), FindObjectOfType<WeaponManager>().transform.position, FindObjectOfType<Camera>().transform.rotation, GameObject.Find("GunPosition").transform);
				newWeapon.name = nextWeapon;
				newWeapon2.name = "BoneBag";
				nextWeapon = "";
			}
			else
			{
				foreach (PlayerWeapon weapon in FindObjectsOfType<PlayerWeapon>())
				{
					Destroy(weapon.gameObject);
				}
				GameObject newWeapon = Instantiate(sc.provideGameObject(nextWeapon), FindObjectOfType<WeaponManager>().transform.position, FindObjectOfType<Camera>().transform.rotation, GameObject.Find("GunPosition").transform);
				newWeapon.name = nextWeapon;
				nextWeapon = "";
			}
		}
	}

	bool checkWeaponOwned(string name)
	{
		foreach (GameObject weapon in ownedWeaponGOs)
		{
			if (weapon.name == name)
				return true;
		}
		return false;
	}

	public void SpendDetrizide()
	{
		if(!GetComponent<Unit>().IsDead())
		{
			if (healavailable)
			{
				lefthandanim.SetTrigger("Heal");

				Debug.Log("healthcost = " + healthcost);

				unit.SetHealth(unit.GetMaxHealth());
				statl.SetDetrizide(statl.GetDetrizide() - healthcost);
				aso.PlayOneShot(sl.GetDetriHeal());
				detrilight.CreateSpellEffect("Heal", spelleffect, spellpos);
			}
			else if (summonavailable)
			{
				lefthandanim.SetTrigger("Summon");
				aso.PlayOneShot(sl.GetDetriRes());
				detrilight.CreateSpellEffect("Summon", spelleffect, spellpos);

				for (int i = 0; i < altar.GetResMax(); i++)
				{
					if (statl.GetDetrizide() > rescost && CountFriends("Friendlies") < altar.GetResMax())
					{
						Debug.Log("rescost = " + rescost);
						statl.SetDetrizide(statl.GetDetrizide() - rescost);
						altar.ResurrectUnit();
					}
				}
			}
			else if (bankavailable)
			{
				lefthandanim.SetTrigger("Bank");
				statl.SetDetrizideBank(statl.GetDetrizideBank() + (fastmode? statl.GetDetrizide()*4 : statl.GetDetrizide()));
				statl.SetDetrizide(0);
				aso.PlayOneShot(sl.GetDetriBank());
				detrilight.CreateSpellEffect("Bank", spelleffect, spellpos);
				GameObject tracer = Instantiate(spendtracer, transform.position, transform.rotation);
				tracer.GetComponent<HomingProjectile>().SetDefaultTarget(wb.transform);
			}
		}
	}

	private void CalculateCosts()
	{
		int timefactor = 3;
		temphealthcost = unit.GetMaxHealth() - unit.GetHealth();
		temphealthcost /= unit.GetMaxHealth();
		temphealthcost *= 100.0f;
		temphealthcost += Altar.GetTimeSinceStart() / timefactor;
		healthcost = Mathf.FloorToInt(Mathf.Clamp(temphealthcost, 0, costlimit));
		rescost = Mathf.Clamp(altar.GetResCost() + Mathf.FloorToInt(Altar.GetTimeSinceStart() / timefactor), 0, costlimit);

		if (unit.GetHealth() < unit.GetMaxHealth() && statl.GetDetrizide() > healthcost)
		{
			spellavailable = true;
			healavailable = true;
		}
		else if (unit.GetHealth() == unit.GetMaxHealth() && statl.GetDetrizide() > rescost && CountFriends("Friendlies") < altar.GetResMax())
		{
			spellavailable = true;
			summonavailable = true;
			healavailable = false;
		}
		else if (unit.GetHealth() == unit.GetMaxHealth() && statl.GetDetrizide() > 0)
		{
			spellavailable = true;
			bankavailable = true;
			summonavailable = false;
			healavailable = false;
		}
		else
		{
			spellavailable = false;
			bankavailable = false;
			summonavailable = false;
			healavailable = false;
		}
	}

	public static int CountFriends(string group)
	{
		var allies = new List<GameObject>();
		foreach (GameObject ally in GameObject.FindGameObjectsWithTag(group))
		{
			if (ally.GetComponent<Unit>())
			{
				if (ally && !ally.GetComponent<Unit>().IsDead())
					allies.Add(ally);
			}
		}
		return allies.Count;
	}
}
