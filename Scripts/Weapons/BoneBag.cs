using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBag : PlayerWeapon
{
	//[SerializeField] private GameObject aura = default;
	//[SerializeField] private AudioClip shoot = default;
	private InputController ic;
	//GameObject aurainstance = null;
	private Animator animator;

	public override void Start()
	{
		animator = GetComponentInChildren<Animator>();
		SetCamera(FindObjectOfType<Camera>());
		SetUIC(FindObjectOfType<UIController>());
		SetUnit(GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>());
		SetSL(FindObjectOfType<StatLibrary>());
		SetASO(FindObjectOfType<Player>().GetComponent<AudioSource>());
		ic = FindObjectOfType<InputController>();
		//GetASO().PlayOneShot(shoot);
	}

	public override void Update()
	{
		SetReadyToFire(Time.time > GetSRTS());
		if (/*aurainstance != null && */ Input.GetMouseButtonUp(ic.LeftClick))
		{
			animator.SetBool("IsAttacking", false);
			//Destroy(aurainstance);
			//aurainstance = null;
		}
		if (Input.GetMouseButtonDown(ic.LeftClick))
		{
			//GetASO().PlayOneShot(shoot);
		}
	}

	public override void Shoot()
	{
		if (GetReadyToFire())
		{
			FireProjectile();
			if (Input.GetMouseButton(ic.LeftClick)/*&& aurainstance == null*/)
			{
				animator.SetBool("IsAttacking", true);
				//aurainstance = Instantiate(aura, GetMouth().transform.position, Quaternion.identity, transform);
				//aurainstance.transform.forward = GetCamera().transform.forward;
			}
		}
	}
}
