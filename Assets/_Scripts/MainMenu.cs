using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGameBtn;
    public Button goBackToGameBtn;
    public Button quitBtn;
    void Start()
    {
        SceneManagers sceneManager = SceneManagers.Instance;

        startGameBtn.onClick.AddListener(sceneManager.LoadIntroScene);
        goBackToGameBtn.onClick.AddListener(sceneManager.LoadMainRoom);
        quitBtn.onClick.AddListener(Application.Quit);
    }
}
