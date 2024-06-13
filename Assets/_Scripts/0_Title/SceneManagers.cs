using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public void LoadScene()
    {
        LoadingSceneController.LoadScene("GameScene");
    }

    public void LoadTitleScene()
    {
        LoadingSceneController.LoadScene("TitleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
