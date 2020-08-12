using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
	//Player
	[SerializeField] private AudioClip playerDead = default;
	public AudioClip GetPlayerDead() { return playerDead; }
	[SerializeField] private AudioClip playerJump = default;
	public AudioClip GetPlayerJump() { return playerJump; }
	[SerializeField] private AudioClip playerStep = default;
	public AudioClip GetPlayerStep() { return playerStep; }
	[SerializeField] private AudioClip detriHeal = default;
	public AudioClip GetDetriHeal() { return detriHeal; }
	[SerializeField] private AudioClip detriRes = default;
	public AudioClip GetDetriRes() { return detriRes; }
	[SerializeField] private AudioClip detriBank = default;
	public AudioClip GetDetriBank() { return detriBank; }

	//PlayerHurtSounds
	[SerializeField] private AudioClip playerHurt1 = default;
	[SerializeField] private AudioClip playerHurt2 = default;
	[SerializeField] private AudioClip playerHurt3 = default;
	[SerializeField] private AudioClip playerHurt4 = default;
	[SerializeField] private AudioClip playerHurt5 = default;
	[SerializeField] private AudioClip playerHurt6 = default;
	private List<AudioClip> playerHurtList;

	//Revenant
	[SerializeField] private AudioClip revenantDead = default;
	public AudioClip GetRevenantDead() { return revenantDead; }
	[SerializeField] private AudioClip revenantHurt = default;
	public AudioClip GetRevenantHurt() { return revenantHurt; }

	//Dreadnought
	[SerializeField] private AudioClip dreadnoughtDead = default;
	public AudioClip GetDreadnoughtDead() { return dreadnoughtDead; }
	[SerializeField] private AudioClip dreadnoughtHurt = default;
	public AudioClip GetDreadnoughtHurt() { return dreadnoughtHurt; }

	//Juggernaught
	[SerializeField] private AudioClip juggernautDead = default;
	public AudioClip GetJuggernautDead() { return juggernautDead; }
	[SerializeField] private AudioClip juggernautHurt = default;
	public AudioClip GetJuggernautHurt() { return juggernautHurt; }

	//Necromancer
	[SerializeField] private AudioClip necroDead = default;
	public AudioClip GetNecroDead() { return necroDead; }
	[SerializeField] private AudioClip necroHurt = default;
	public AudioClip GetNecroHurt() { return necroHurt; }

	//Lich
	[SerializeField] private AudioClip lichDead = default;
	public AudioClip GetLichDead() { return lichDead; }
	[SerializeField] private AudioClip lichHurt = default;
	public AudioClip GetLichHurt() { return lichHurt; }

	[SerializeField] private AudioClip lichspawn1 = default;
	[SerializeField] private AudioClip lichspawn2 = default;
	[SerializeField] private AudioClip lichspawn3 = default;
	private List<AudioClip> lichspawnList;

	//Lacerator
	[SerializeField] private AudioClip lacerDead = default;
	public AudioClip GetLacerDead() { return lacerDead; }
	[SerializeField] private AudioClip lacerHurt = default;
	public AudioClip GetLacerHurt() { return lacerHurt; }

	//Pickups
	[SerializeField] private AudioClip pickupSpawned = default;
	public AudioClip GetPickupSpawned() { return pickupSpawned; }
	[SerializeField] private AudioClip quadbullet = default;
	public AudioClip GetQuadBullet() { return quadbullet; }
	[SerializeField] private AudioClip pickupGet = default;
	public AudioClip GetPickupGet() { return pickupGet; }
	[SerializeField] private AudioClip annihilation = default;
	public AudioClip GetAnnihilation() { return annihilation; }
	[SerializeField] private AudioClip summonUpgrade = default;
	public AudioClip GetSummonUpgrade() { return summonUpgrade; }
	[SerializeField] private AudioClip healthUpgrade = default;
	public AudioClip GetHealthUpgrade() { return healthUpgrade; }
	[SerializeField] private AudioClip shotgun = default;
	public AudioClip GetShotgun() { return shotgun; }
	[SerializeField] private AudioClip gauntlets = default;
	public AudioClip GetGauntlets() { return gauntlets; }
	[SerializeField] private AudioClip snubnose = default;
	public AudioClip GetSnubnose() { return snubnose; }
	[SerializeField] private AudioClip crucifier = default;
	public AudioClip GetCrucifier() { return crucifier; }
	[SerializeField] private AudioClip reaver = default;
	public AudioClip GetReaver() { return reaver; }
	[SerializeField] private AudioClip devourer = default;
	public AudioClip GetDevourer() { return devourer; }
	[SerializeField] private AudioClip scythe = default;
	public AudioClip GetScythe() { return scythe; }
	[SerializeField] private AudioClip voidcannon = default;
	public AudioClip GetVoidCannon() { return voidcannon; }

	//Music
	[SerializeField] private AudioClip track1 = default;
	[SerializeField] private AudioClip track2 = default;
	[SerializeField] private AudioClip track3 = default;
	[SerializeField] public static int trackCount = 3; 
	private List<AudioClip> tracklist;
	


	private void Start()
	{
		playerHurtList = new List<AudioClip>();
		playerHurtList.AddRange(new AudioClip[] { playerHurt1, playerHurt2, playerHurt3, playerHurt4, playerHurt5, playerHurt6 });
		lichspawnList = new List<AudioClip>();
		lichspawnList.AddRange(new AudioClip[] { lichspawn1, lichspawn2, lichspawn3 });
		tracklist = new List<AudioClip>();
		tracklist.AddRange(new AudioClip[] { track1, track2, track3 });
	}

	public static void varySoundPitch(AudioSource aso, float pitchvariance)
	{
		aso.pitch += Random.Range(-1 * pitchvariance, pitchvariance);
	}

	public static void ResetPitch(AudioSource aso)
	{
		aso.pitch = 1;
	}

	public AudioClip ChoosePlayerHurt()
	{
		int random = Random.Range(0, 5);
		return playerHurtList[random];
	}

	public AudioClip ChooseLichSpawn()
	{
		int random = Random.Range(0, 2);
		return lichspawnList[random];
	}

	public AudioClip GetTrack(MusicPlayer mp)
	{
		int index = 0;
		for (int i = 0; i < tracklist.Count; i++)
		{
			if (tracklist[mp.GetLastTrack()] == tracklist[i])
			{
				int container = i;
				if (i == tracklist.Count - 1)
					container = 0;
				else
					container = i + 1;
				index = container;
			}
		}
		mp.SetLastTrack(index);
		return tracklist[index];
	}
}
