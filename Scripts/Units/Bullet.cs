using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private float projSpeed = default;
	[SerializeField] private float projDecay = default;
	[SerializeField] private int damage = default;

	[SerializeField] private bool hasInaccurateFire = default;
	[SerializeField] private bool isPiercing = default;
	[SerializeField] private float inaccuracyOffset = default;

	private GameObject parent;
	private AudioSource aso;
	private AudioSource playeraso;
	public GameObject GetParent() { return parent; }
	public void SetParent(GameObject value) { parent = value; }

	private TrailRenderer tr;


	public int GetDamage() { return damage; }

	void Start()
	{
		if (parent)
		{
			if (GetComponent<Homing>() && parent.GetComponent<BasicAI>())
			{
				GetComponent<Homing>().SetTarget(parent.GetComponent<BasicAI>().GetTarget());
			}
			aso = GetComponent<AudioSource>();
			playeraso = GameObject.Find("Player").GetComponent<AudioSource>();
			tr = GetComponentInChildren<TrailRenderer>();


			if (parent == FindObjectOfType<Player>().gameObject || parent == FindObjectOfType<Player>().gameObject)
			{

				//quad damage
				damage *= FindObjectOfType<Player>().GetDamageModifier();
				if (FindObjectOfType<Player>().GetDamageModifier() > 1)
				{
					if(GameObject.Find("Shotgun"))
						playeraso.PlayOneShot(FindObjectOfType<SoundLibrary>().GetQuadBullet(), 0.15f);
					else if (FindObjectOfType<Crucifier>())
						playeraso.PlayOneShot(FindObjectOfType<SoundLibrary>().GetQuadBullet());
					else
						playeraso.PlayOneShot(FindObjectOfType<SoundLibrary>().GetQuadBullet(), 0.33f);
					if ( tr && tr.time<0.5f)
						GetComponentInChildren<TrailRenderer>().time = 0.5f;
				}
					
			}
			//ignore collisions between parent and bullet
			if (parent.gameObject.GetComponentInChildren<Collider>() && gameObject.GetComponentInChildren<Collider>())
				Physics.IgnoreCollision(gameObject.GetComponentInChildren<Collider>(), parent.gameObject.GetComponentInChildren<Collider>(), true);

			if (parent.gameObject.GetComponentInChildren<CharacterController>() && gameObject.GetComponentInChildren<Collider>())
				Physics.IgnoreCollision(parent.gameObject.GetComponentInChildren<CharacterController>(), gameObject.GetComponentInChildren<Collider>(), true);

			if (hasInaccurateFire)
			{
				transform.forward += (new Vector3(Random.Range(-inaccuracyOffset, inaccuracyOffset), Random.Range(-inaccuracyOffset, inaccuracyOffset), Random.Range(-inaccuracyOffset, inaccuracyOffset)));
				transform.Rotate(new Vector3(0, 0, Random.Range(0, 180)));
			}
			if (aso)
				SoundLibrary.varySoundPitch(aso, 0.2f);
			Destroy(gameObject, projDecay);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!isPiercing && other.gameObject != parent)
		{
			Destroy(gameObject, 0.02f);
		}
	}

	void FixedUpdate()
	{
		if (!GetComponent<Homing>())
		{
			transform.GetComponent<Rigidbody>().velocity = transform.forward * projSpeed * Time.deltaTime;
		}
	}
}