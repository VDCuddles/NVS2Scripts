using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
	List<Material> originalMaterials;
	List<MeshRenderer> renderers;
	public List<Material> GetOriginalMaterials() { return originalMaterials; }
	public List<MeshRenderer> GetRendererList() { return renderers; }
	private SpawnController sc;
	[SerializeField] private int resCost = default;
	[SerializeField] private int resMax = default;
	[SerializeField] private GameObject resUnit = default;
	[SerializeField] private GameObject spawnAnim = default;
	[SerializeField] private GameObject summontracer = default;
	[SerializeField] private GameObject reslocation = default;

	public int GetResCost() { return resCost; }
	public int GetResMax() { return resMax; }
	public void SetResCost(int value) { resCost = value; }
	public void SetResMax(int value) { resMax = value; }
	public GameObject GetResUnit() { return resUnit; }
	public void SetResUnit(GameObject value) { resUnit = value; }

	[SerializeField] private AudioClip startGameClip = default;
	[SerializeField] private GameObject startGameLight = default;
	private SoundLibrary sl;
	private GameOptions gops;
	private AudioSource musicplayer;
	private ParticleSystem ps;
	private UIController uic;
	private GameObject player;

	private static float startTime;
	public static float GetStartTime() { return startTime; }
	public static float GetTimeSinceStart() { return Time.timeSinceLevelLoad - startTime; }
	public void SetStartTime(float value) { startTime = value; }
	private AudioSource aso;
	private SpawnPoint[] spawnlocs;

	void Start()
	{
		player = FindObjectOfType<Player>().gameObject;
		uic = FindObjectOfType<UIController>();
		ps = GetComponentInChildren<ParticleSystem>();
		musicplayer = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
		sl = FindObjectOfType<SoundLibrary>();
		spawnlocs = FindObjectsOfType<SpawnPoint>();
		aso = GameObject.Find("Environment").GetComponent<AudioSource>();
		sc = FindObjectOfType<SpawnController>();
		gops = FindObjectOfType<GameOptions>();
		originalMaterials = new List<Material>();
		renderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
		FindObjectOfType<Detrilight>().saveOriginalMaterials(originalMaterials, gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!sc.GetGameActive() && other.gameObject.CompareTag("Player"))
		{
			sc.SetGameActive(true);
			aso.PlayOneShot(startGameClip);
			ps.Stop();
			GameObject.Find("AltarGlow").GetComponent<Animator>().SetTrigger("EndGlow");
			StartCoroutine("PlayMusic");
			SetStartTime(Time.timeSinceLevelLoad);
			GameObject flash = Instantiate(startGameLight, transform.position, Quaternion.identity, transform);
			Destroy(flash, 1.7f);
			foreach (SpawnPoint sp in spawnlocs)
			{
				sp.gameObject.GetComponentInChildren<ParticleSystem>().Play();
			}
			StartCoroutine("DisplayKillTut");
		}

	}

	IEnumerator PlayMusic()
	{
		MusicPlayer mp = musicplayer.gameObject.GetComponent<MusicPlayer>();
		yield return new WaitForSeconds(1.8f);
		musicplayer.volume = mp.GetChosenVolume();
		musicplayer.clip = sl.GetTrack(mp);
		musicplayer.Play();
	}

	IEnumerator DisplayKillTut()
	{
		yield return new WaitForSeconds(5);
		uic.DisplayTutorial("kills");
	}

	public void ResurrectUnit()
	{
		float offsetamount = 30;
		Vector3 randomoffset = new Vector3(Random.Range(-offsetamount, offsetamount), 0, Random.Range(-offsetamount, offsetamount));
		GameObject newUnit = Instantiate(resUnit, reslocation.transform.position + randomoffset, Quaternion.identity, GameObject.Find("Friendlies").transform);
		GameObject anim = Instantiate(spawnAnim, reslocation.transform.position + randomoffset, Quaternion.identity, GameObject.Find("Friendlies").transform);
		GameObject tracer = Instantiate(summontracer, player.transform.position, player.transform.rotation);
		tracer.GetComponent<HomingProjectile>().SetDefaultTarget(newUnit.transform);
		Destroy(anim, 1.0f);
		newUnit.name = resUnit.name;
	}
}

