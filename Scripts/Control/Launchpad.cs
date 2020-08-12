using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Launchpad : MonoBehaviour
{
	[SerializeField] private AudioClip jump = default;
	[SerializeField] private float jumpHeight = default;
	private AudioSource aso;
	private PlayerMovement pm;
	private Vector3 velocity;

	private void Start()
	{
		velocity = GetComponentInChildren<ParticleSystem>().transform.forward;
		velocity *= jumpHeight;
		aso = FindObjectOfType<Player>().GetComponent<AudioSource>();
		pm = FindObjectOfType<Player>().GetComponent<PlayerMovement>();
	}
	private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.GetComponent<Player>())
		{
			Vector3 nv = velocity;
			pm.SetVelocity(nv);
			pm.SetMDisabled(true);
			aso.PlayOneShot(jump);
		}
    }
}
