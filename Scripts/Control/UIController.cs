using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
	private WeaponBench wb;
	private PassiveBench pab;
	private PowerupBench pob;
	private SpawnController sc;
	private InputController ic;
	private Altar altar;
	private StatLibrary statl;
	private Player player;
	private TextMeshProUGUI TMPBUI;
	private TextMeshProUGUI TMPTUT;
	private TextMeshProUGUI TMPTUI;
	private TextMeshProUGUI TMPHS;
	private TextMeshPro TMPBank;
	private TextMeshPro TMPTime;
	private GameOptions gops;
	private MusicPlayer mp;
	private Animator tutanim;
	private bool killtutdone = false;
	private bool dttutdone = false;
	private bool weapontutdone = false;
	private bool passivetutdone = false;
	private bool poweruptutdone = false;
	private bool ammotutdone = false;
	private bool finaldone = false;
	private bool swdone = false;
	private string killtext;
	private string dttext;
	private string weapontext;
	private string passivetext;
	private string poweruptext;
	private string ammotext;
	private string swtext;

	string temptuttext = "";

	void Start()
	{
		wb = FindObjectOfType<WeaponBench>();
		pab = FindObjectOfType<PassiveBench>();
		pob = FindObjectOfType<PowerupBench>();
		mp = FindObjectOfType<MusicPlayer>();
		ic = FindObjectOfType<InputController>();
		gops = FindObjectOfType<GameOptions>();
		TMPBUI = GameObject.Find("TMPBUI").GetComponent<TextMeshProUGUI>();
		TMPTUT = GameObject.Find("TMPTUT").GetComponent<TextMeshProUGUI>();
		TMPTUI = GameObject.Find("TMPTUI").GetComponent<TextMeshProUGUI>();
		TMPHS = GameObject.Find("TMPHS").GetComponent<TextMeshProUGUI>();
		TMPBank = GameObject.Find("TMPBank").GetComponent<TextMeshPro>();
		TMPTime = GameObject.Find("TMPTime").GetComponent<TextMeshPro>();
		tutanim = TMPTUT.GetComponent<Animator>();
		altar = FindObjectOfType<Altar>();
		player = FindObjectOfType<Player>();
		statl = FindObjectOfType<StatLibrary>();
		sc = FindObjectOfType<SpawnController>();
		TMPHS.text = "";
		TMPTUT.text = "";
		ClearStats();

		killtext = "Kill enemies to get necromantic energy (Detrizide).";
		dttext = "Press or hold the " + ic.Detrizide + " key when weapon is flashing to spend energy and cast spells. Do this constantly.";
		weapontext = "Collect weapons from the gold flashing altar in the middle once their cost has been paid.";
		passivetext = "Collect passive upgrades from the blue flashing altar to the west once their cost has been paid. ";
		poweruptext = "Powerups will periodically become available. Acquire them from the red flashing altar in the east to temporarily gain a bonus.";
		ammotext = "Some weapons run out of ammo. Switch weapons using " + ic.CycleWeapons + " or " + ic.NextWeapon + " to refill ammo.";
		swtext = "The Void Cannon and Devourer weapons are superweapons. These do immense damage, but cost banked Detrizide to use. Cost increases each use. ";
	}
	private void Update()
	{
		TMPBank.text = MakeRoman(statl.GetDetrizideBank());
		if (!player.gameObject.GetComponent<Unit>().IsDead() && sc.GetGameActive())
		{
			TMPTime.text = MakeRoman(Mathf.FloorToInt(Altar.GetTimeSinceStart()));
		}
		if (gops.GetUIEnabled() && !player.gameObject.GetComponent<Unit>().IsDead() && sc.GetGameActive())
		{
			DisplayStats();
		}
		else if (!player.gameObject.GetComponent<Unit>().IsDead() && !gops.GetUIEnabled())
			ClearStats();

		UpdateAndCheckTutorials();
	}

	private void UpdateAndCheckTutorials()
	{
		if (gops.GetTutorialsEnabled())
		{

			if (TMPTUT.text == killtext && statl.GetDetrizide() > 0)
			{
				StartCoroutine(ClearTutorials());
				killtutdone = true;
			}
			else if (killtutdone && !dttutdone && TMPTUT.text == "")
			{
				StartCoroutine(DelayedTut(5, "detrizide"));
				dttutdone = true;
			}
			else if (dttutdone && TMPTUT.text == dttext && Input.GetKeyDown(ic.Detrizide))
			{
				StartCoroutine(ClearTutorials());
			}
			else if (wb.GetComponentInChildren<Pickup>() && !weapontutdone && TMPTUT.text == "")
			{
				DisplayTutorial("weapons");
				weapontutdone = true;
			}
			else if (weapontutdone && TMPTUT.text == weapontext && !wb.GetComponentInChildren<Pickup>())
			{
				StartCoroutine(ClearTutorials());
			}
			else if (pab.GetComponentInChildren<Pickup>() && !passivetutdone && TMPTUT.text == "")
			{
				DisplayTutorial("passives");
				passivetutdone = true;
			}
			else if (passivetutdone && TMPTUT.text == passivetext && !pab.GetComponentInChildren<Pickup>())
			{
				StartCoroutine(ClearTutorials());
			}
			else if (pob.GetComponentInChildren<Pickup>() && !poweruptutdone && TMPTUT.text == "")
			{
				DisplayTutorial("powerups");
				poweruptutdone = true;
			}
			else if (poweruptutdone && TMPTUT.text == poweruptext && !pob.GetComponentInChildren<Pickup>())
			{
				StartCoroutine(ClearTutorials());
			}
			if (TMPTUT.text == ammotext && (Input.GetKeyDown(ic.CycleWeapons) || Input.GetKeyDown(ic.NextWeapon)))
			{
				StartCoroutine(ClearTutorials());
				ammotutdone = true;
			}

			if (player.GetCurrentWeapon())
				if ((player.GetCurrentWeapon().name == "Void Cannon" || player.GetCurrentWeapon().name == "Devourer") && !swdone && TMPTUT.text == "")
				{
					DisplayTutorial("sw");
					swdone = true;
					StartCoroutine(ClearTutorials(25));
				}

			if (swdone && !finaldone && TMPTUT.text == "")
			{
				finaldone = true;
				StartCoroutine(DelayedTut(15, "final"));
				StartCoroutine(ClearTutorials(25));

			}

		}
	}

	public void DisplayTutorial(string tut)
	{
		if (gops.GetTutorialsEnabled())
			switch (tut)
			{
				case "kills":
					if(!killtutdone)
					{
						temptuttext = killtext;
						tutanim.SetBool("TutOn", true);
					}
					break;
				case "detrizide":
					temptuttext = dttext;
					tutanim.SetBool("TutOn", true);
					break;
				case "weapons":
					if (!weapontutdone)
					{
						temptuttext = weapontext;
						tutanim.SetBool("TutOn", true);
					}
					break;
				case "passives":
					if (!passivetutdone)
					{
						temptuttext = passivetext;
						tutanim.SetBool("TutOn", true);
					}
					break;
				case "powerups":
					if (!poweruptutdone)
					{
						temptuttext = poweruptext;
						tutanim.SetBool("TutOn", true);
					}
					break;
				case "ammo":
					if (!ammotutdone && TMPTUT.text == "")
					{
						temptuttext = ammotext;
						tutanim.SetBool("TutOn", true);
					}
					break;
				case "final":
					if (TMPTUT.text == "")
					{
						temptuttext = "Tutorials can be disabled in the options.\n\nGood luck, Necromancer.";
						tutanim.SetBool("TutOn", true);
					}
					break;
				case "sw":
					if (TMPTUT.text == "")
					{
						temptuttext = swtext;
						tutanim.SetBool("TutOn", true);
					}
					break;

			}
		TMPTUT.text = temptuttext;
		temptuttext = "";
	}

	IEnumerator ClearTutorials()
	{
		tutanim.SetBool("TutOn" , false);
		yield return new WaitForSeconds(1);
		TMPTUT.text = "";
	}

	IEnumerator ClearTutorials(float delay)
	{
		yield return new WaitForSeconds(delay);
		tutanim.SetBool("TutOn", false);
		yield return new WaitForSeconds(1);
		TMPTUT.text = "";
	}

	IEnumerator DelayedTut(float delay, string tut)
	{
		yield return new WaitForSeconds(delay);
		DisplayTutorial(tut);
	}

	public void DisplayHighScore()
	{
		if (sc.GetGameActive() && gops.GetSave().highScore < Mathf.FloorToInt(Altar.GetTimeSinceStart()) && gops.GetMaxHealth() <= 250)
		{
			gops.GetSave().highScore = Mathf.FloorToInt(Altar.GetTimeSinceStart());
			SaveSystem.SaveGame(gops.GetSave().highScore, gops, ic, mp.GetChosenVolume(), Screen.currentResolution.width, Screen.currentResolution.height, QualitySettings.GetQualityLevel());
		}
		TMPHS.text = "BEST TIME: " + gops.GetSave().highScore + "\n\nPress " + ic.Reset + " to try again.";
	}

	public void DisplayStats()
	{
		TMPBUI.text = "DETRIZIDE : " + TMPBank.text + " [" + statl.GetDetrizideBank() + "]";
		TMPTUI.text = "TIME : " + TMPTime.text + " [" + Mathf.FloorToInt(Altar.GetTimeSinceStart()) + "]";
	}

	public void ClearStats()
	{
		TMPBUI.text = "";
		TMPTUI.text = "";
	}

	public static string MakeRoman(int value)
	{
		var originalvalue = value;
		var arabic = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
		var roman = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
		var result = "";
		for (int i = 0; i < 13; i++)
		{
			while (value >= arabic[i])
			{
				result = result + roman[i].ToString();
				value = value - arabic[i];
			}
		}
		if (originalvalue < 10000)
			return result;
		else
			return originalvalue.ToString();
	}
}
