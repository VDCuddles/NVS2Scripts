using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] private GameObject exit = default;
	[SerializeField] private AudioClip portaled = default;
	private CharacterController cc;

	private void OnTriggerEnter(Collider other)
    {
		cc = other.gameObject.GetComponent<CharacterController>();

		if (other.gameObject.GetComponent<Player>() && other.gameObject.GetComponent<CharacterController>())
		{
			cc.enabled = false;
			other.gameObject.transform.position = exit.transform.position;
			GetComponent<AudioSource>().PlayOneShot(portaled);
			cc.enabled = true;

		}
	}
}
