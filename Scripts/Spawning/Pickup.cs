using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	[SerializeField] private string pickupName = default;
	public string GetPickupName() { return pickupName; }
	private List<Material> originalMaterials;
	// Start is called before the first frame update
	void Start()
	{
		originalMaterials = new List<Material>();
		MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer boi in renderers)
			originalMaterials.Add(boi.material);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		transform.Rotate(0, 30.0f * Time.deltaTime, 0, Space.World);

		//for (int i = 0; i < originalMaterials.Count; i++)
		//{
		//	gameObject.GetComponentsInChildren<MeshRenderer>()[i].material = originalMaterials[i];
		//}
	}
}
