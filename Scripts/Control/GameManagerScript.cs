using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject gamePersistent = default;
    private MusicPlayer mp;
    private GameObject gopsgo;
    private GameOptions gops;
    private InputController ic;
    private Unit playerunit;

    bool hasSetHealth;

    private void Awake()
    {
        if (!FindObjectOfType<GameOptions>())
            gopsgo = Instantiate(gamePersistent);
    }
    void Start()
    {
        if (FindObjectOfType<GameOptions>())
        {
            ic = FindObjectOfType<InputController>();
            ic.SetCrosshair(GameObject.Find("Crosshair"));
            gops = FindObjectOfType<GameOptions>();
            gopsgo = gops.gameObject;
            ic.GetCrosshair().SetActive(FindObjectOfType<GameOptions>().GetUIEnabled());
        }
        if (FindObjectOfType<MusicPlayer>())
        {
            mp = FindObjectOfType<MusicPlayer>();
            mp.gameObject.GetComponent<AudioSource>().volume = mp.GetChosenVolume();
        }
    }

    private void Update()
    {
        SetHealthForPlayer();
    }

    private void SetHealthForPlayer()
    {
        if (!hasSetHealth && FindObjectOfType<Player>() && gops && gops.GetSave() != null)
        {
            gops.SetMaxHealth(gops.GetSave().defaultMaxHealth);
            playerunit = FindObjectOfType<Player>().gameObject.GetComponent<Unit>();
            playerunit.SetMaxHealth(gops.GetSave().defaultMaxHealth);
            playerunit.SetHealth(gops.GetSave().defaultMaxHealth);
            Debug.Log("player health set to " + playerunit.GetHealth());
            hasSetHealth = true;
        }
    }
}
