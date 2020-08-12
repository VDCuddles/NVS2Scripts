using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detrilight : MonoBehaviour
{
	private GameObject detriLight;
	private Animator detrianim;
	private GameObject dsprite;

	[SerializeField] private GameObject dspriteposition = default;
	[SerializeField] private GameObject dspriteb = default;
	[SerializeField] private GameObject dsprites = default;
	[SerializeField] private GameObject dspriteh = default;
	
	private StatLibrary sl;
	private Altar altar;
	private WeaponBench wb;
	List<Material> originalMaterials;
	[SerializeField] private Material benchMaterial = default;
	[SerializeField] private Unit playerunit = default;

	public Color healcolor { get; set; }
	public Color rescolor { get; set; }
	public Color bankcolor { get; set; }
	public Color blankcolor { get; set; }

	void Start()
	{
		altar = FindObjectOfType<Altar>();
		wb = FindObjectOfType<WeaponBench>();
		sl = FindObjectOfType<StatLibrary>();
		detrianim = GetComponent<Animator>();
		detriLight = gameObject;
		SetDTColours();
		saveOriginalMaterials();
	}

	void Update()
    {
		ColourDetrizideSpell();
	}

	public void ChangeDLColour(Color color)
	{
		Light light = detriLight.GetComponent<Light>();
		light.color = color;
	}

	public void SetDTColours()
	{
		healcolor = new Color(0, 255, 0, 255);
		rescolor = new Color(0, 0, 100, 255);
		bankcolor = new Color(255, 161, 8, 255);
		blankcolor = new Color(0, 0, 0, 0);
	}

	public void CreateSpellEffect(string type, GameObject effect, Transform pos)
	{
		GameObject instance = effect;
		ParticleSystem ps = effect.GetComponentInChildren<ParticleSystem>();
		var main = ps.main;
		switch (type)
		{
			case "Heal":
				instance.GetComponentInChildren<Light>().color = healcolor;
				main.startColor = healcolor;
				break;
			case "Summon":
				instance.GetComponentInChildren<Light>().color = rescolor;
				main.startColor = rescolor;
				break;
			case "Bank":
				instance.GetComponentInChildren<Light>().color = bankcolor;
				main.startColor = bankcolor;
				break;
		}
		instance = Instantiate(effect, pos.position, playerunit.transform.rotation, playerunit.transform);
		Destroy(instance, 0.5f);

	}

	public void ColourDetrizideSpell()
	{
		if (!detrianim || !detriLight)
		{
			detriLight = GameObject.Find("DetriLight");
			detrianim = detriLight.GetComponent<Animator>();
		}
		int healthcost = Player.GetHealthCost();
		int rescost = Player.GetResCost();
		// priority: health > resurrections > upgrades

		if (playerunit.GetHealth() < playerunit.GetMaxHealth() && sl.GetDetrizide() > healthcost && healthcost != 0)
		{

			detrianim.SetBool("IsDetriReady", true);
			detrianim.SetBool("IsSummonReady", false);
			ChangeDLColour(healcolor);
			ResetMaterials(wb.gameObject);
			ResetMaterials(altar.gameObject);
			if (!dsprite)
				dsprite = Instantiate(dspriteh, dspriteposition.transform.position, transform.rotation, dspriteposition.transform);
			else if (dsprite.GetComponent<Dsprite>().GetSType() != "Heal")
				Destroy(dsprite);
		}
		else if (playerunit.GetHealth() == playerunit.GetMaxHealth() && sl.GetDetrizide() > rescost && Player.CountFriends("Friendlies") < altar.GetResMax())
		{
			detrianim.SetBool("IsDetriReady", false);
			detrianim.SetBool("IsSummonReady", true);
			ChangeDLColour(rescolor);

			if (!dsprite)
				dsprite = Instantiate(dsprites, dspriteposition.transform.position, transform.rotation, dspriteposition.transform);
			else if (dsprite.GetComponent<Dsprite>().GetSType() != "Summon")
				Destroy(dsprite);
		}
		else if (playerunit.GetHealth() == playerunit.GetMaxHealth() && sl.GetDetrizide() > 0)
		{
			detrianim.SetBool("IsDetriReady", true);
			detrianim.SetBool("IsSummonReady", false);
			ChangeDLColour(bankcolor);
			overwriteMaterials(benchMaterial, wb.gameObject);
			ResetMaterials(altar.gameObject);

			if (!dsprite)
				dsprite = Instantiate(dspriteb, dspriteposition.transform.position, transform.rotation, dspriteposition.transform);
			else if (dsprite.GetComponent<Dsprite>().GetSType() != "Bank")
				Destroy(dsprite);

		}
		else
		{
			detrianim.SetBool("IsDetriReady", false);
			detrianim.SetBool("IsSummonReady", false);
			ChangeDLColour(blankcolor);
			ResetMaterials(altar.gameObject);
			ResetMaterials(wb.gameObject);

			if (dsprite)
				Destroy(dsprite);
		}
	}

	public void saveOriginalMaterials()
	{
		originalMaterials = new List<Material>();
		MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer boi in renderers)
			originalMaterials.Add(boi.material);
	}

	public void saveOriginalMaterials(List<Material> list, GameObject go)
	{
		MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer boi in renderers)
			list.Add(boi.material);
	}

	public void overwriteMaterials(Material mat, GameObject go)
	{
		MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer boi in renderers)
			boi.material = mat;
	}

	public void ResetMaterials(GameObject go)
	{
		if (go.GetComponent<WeaponBench>())
		{
			for (int i = 0; i < go.GetComponent<WeaponBench>().GetOriginalMaterials().Count; i++)
			{
				go.GetComponent<WeaponBench>().GetComponentsInChildren<MeshRenderer>()[i].material = go.GetComponent<WeaponBench>().GetOriginalMaterials()[i];
			}
		}
		else if (go.GetComponent<Altar>())
		{
			for (int i = 0; i < go.GetComponent<Altar>().GetOriginalMaterials().Count; i++)
			{
				go.GetComponent<Altar>().GetComponentsInChildren<MeshRenderer>()[i].material = go.GetComponent<Altar>().GetOriginalMaterials()[i];
			}
		}
	}
}
