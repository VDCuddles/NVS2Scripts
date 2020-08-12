using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidFadeWall : MonoBehaviour
{
    public void StartMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            FastFade();
        }
    }
    public void FastFade()
    {
        GameObject.Find("SecondFadeWall").GetComponent<Animator>().SetTrigger("FastFade");
    }
}
