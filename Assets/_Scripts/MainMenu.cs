using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public Button startGameBtn;
    //public Button goBackToGameBtn;
    public Button optionBtn;
    public Button quitBtn;

    public AudioMixer audioMixer;

    public GameObject mainMenu;
    public GameObject optionMenu;
    void Awake()
    {
        SceneManagers sceneManager = SceneManagers.Instance;

        startGameBtn.onClick.AddListener(sceneManager.LoadIntroScene);
        optionBtn.onClick.AddListener(OpenOptionMenu);
        quitBtn.onClick.AddListener(Application.Quit);
    }

    private void OpenOptionMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void GoBackToMain()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
