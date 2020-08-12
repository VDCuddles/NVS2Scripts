using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatLibrary : MonoBehaviour
{
	[SerializeField] private int detrizide = 0;
	public void SetDetrizide(int gains) { detrizide = gains; }
	public int GetDetrizide() { return detrizide; }

	private int detrizideBank;
	public void SetDetrizideBank(int gains) { detrizideBank = gains; }
	public int GetDetrizideBank() { return detrizideBank; }

}
