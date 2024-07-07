using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public Animator animator;

    public void LoadScene()
    {
        animator.SetTrigger("FadeOut");
        LoadingSceneController.LoadScene("GameScene");        
    }

    public void LoadTitleScene()
    {
        animator.SetTrigger("FadeOut");
        LoadingSceneController.LoadScene("TitleScene");        
    }

    public void QuitGame()
    {
        animator.SetTrigger("FadeOut");
        Application.Quit();
    }
}
