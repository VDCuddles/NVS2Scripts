using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject gamePersistent = default;
    [SerializeField] private AudioClip startclip = default;
    [SerializeField] private float startDelay = default;
    private AudioSource mpaso;
    private MusicPlayer mp;

    bool volumefading = false;
    bool highscoreupdated = false;

    AsyncOperation operation;
    [SerializeField] private GameObject loadingBar = default;
    [SerializeField] private Slider loadingSlider = default;

    private void Start()
    {
        GameObject gp;
        if (!FindObjectOfType<GameOptions>())
        {
            gp = Instantiate(gamePersistent);
            gp.name = gamePersistent.name;
        }
            


        mp = FindObjectOfType<MusicPlayer>();
        mpaso = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
        mpaso.clip = mp.GetMainMenu();
        mpaso.Play();
        StartCoroutine(LoadAsynchronously());

    }

    private void Update()
    {
        if(!highscoreupdated && FindObjectOfType<GameOptions>().GetSave() != null)
        {
            GameObject.Find("TMPHS").GetComponent<TextMeshProUGUI>().text = "PERSONAL BEST: " + FindObjectOfType<GameOptions>().GetSave().highScore.ToString();
            highscoreupdated = true;
        }
        if (volumefading && mpaso.volume> 0)
        {
            mpaso.volume -= (0.5f * Time.deltaTime);
        }
    }
    public void RefreshHighScore()
    {
        if (FindObjectOfType<GameOptions>().GetSave() != null)
        {
            GameObject.Find("TMPHS").GetComponent<TextMeshProUGUI>().text = "PERSONAL BEST: " + FindObjectOfType<GameOptions>().GetSave().highScore.ToString();
            Debug.Log("Save reset successfully.");
        }
    }
    public void LoadGame()
    {
        GameObject.Find("Environment").GetComponent<AudioSource>().PlayOneShot(startclip);
        loadingBar.SetActive(true);
        //StartCoroutine(LoadAsynchronously());
        StartFade();
    }
    private void StartFade()
    {
        FindObjectOfType<MenuFadeWall>().StartGame();
        StartCoroutine(WaitForFadeAndLoad());
        volumefading = true;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator WaitForFadeAndLoad()
    {
        yield return new WaitForSeconds(startDelay);
        operation.allowSceneActivation = true;
    }
    IEnumerator LoadAsynchronously()
    {
        yield return new WaitForSeconds(0.1f);
        operation = SceneManager.LoadSceneAsync("Game");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }
}
