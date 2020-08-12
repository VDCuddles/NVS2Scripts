using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetMenu : MonoBehaviour
{
    private GameOptions gops;
    //[SerializeField] private GameObject gamePersistent = default;

    void Start()
    {
        gops = FindObjectOfType<GameOptions>();
    }

    public void DeleteSettings()
    {
        //GameObject gp;

        SaveSystem.ClearSave();
        if (FindObjectOfType<GameOptions>())
        {
            Destroy(gops.gameObject);

   //         gp = Instantiate(gamePersistent);
   //         gp.name = gamePersistent.name;
			//gops = gp.GetComponent<GameOptions>();
            Debug.Log("Old game options destroyed.");
        }
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        SceneManager.SetActiveScene(scene);
    }

}
