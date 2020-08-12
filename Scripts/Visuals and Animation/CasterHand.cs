using UnityEngine;

public class CasterHand : MonoBehaviour
{
	private Player player;

	private void Start()
	{
		player = FindObjectOfType<Player>();
	}
	public void SpendPlayerDt()
	{
		player.SpendDetrizide();
	}
	public void NoLongerCasting()
	{
		player.SetIsCasting(false);
	}

}
