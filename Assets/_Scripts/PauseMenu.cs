using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionMenu;

    // 버그땜에 테스트
    public GameObject goToTitleMenu;
    public AudioMixer audioMixer;
    public static bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        optionMenu.SetActive(false);
        Cursor.visible = true;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        ResumeGame();
        Cursor.visible = true;
        SceneManagers.Instance.LoadTitleScene();       
    }

    public void OptionMenu()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void BackBtn()
    {
        optionMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
