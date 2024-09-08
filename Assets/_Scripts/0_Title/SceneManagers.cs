using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public static SceneManagers Instance;
    private Animator animator;
    private PlayerAttributesManager playerAttributesManager;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else if(Instance != null)
        {
            Destroy(gameObject);
        }
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        playerAttributesManager = FindAnyObjectByType<PlayerAttributesManager>();
    }

    private IEnumerator animatorOnOff()
    {
        animator.gameObject.SetActive(false);
        yield return new WaitForSeconds(0f);
        animator.gameObject.SetActive(true);        
    }

    public void LoadIntroScene()
    {
        StartCoroutine(animatorOnOff());        
        LoadingSceneController.LoadScene("MainStory");
        animator.SetTrigger("FadeOut");
    }

    public void LoadMainRoom()
    {
        StartCoroutine(animatorOnOff());
        LoadingSceneController.LoadScene("MainRoom");
        animator.SetTrigger("FadeOut");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            player.transform.position = new Vector3(0, 10f, -13f);
            PlayerAttributesManager.Instance._health = PlayerAttributesManager.Instance.maxHealth;
        }
    }

    public void LoadDungeon()
    {
        StartCoroutine(animatorOnOff());        
        LoadingSceneController.LoadScene("GameScene");
        animator.SetTrigger("FadeOut");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0, 10f, -10f);
        }
    }

    public void LoadTitleScene()
    {
        StartCoroutine(animatorOnOff());
        animator.SetTrigger("FadeOut");

        if(PlayerAttributesManager.Instance != null)
        {
            Destroy(PlayerAttributesManager.Instance.gameObject);
        }
        LoadingSceneController.LoadScene("TitleScene");
    }

    public void QuitGame()
    {
        StartCoroutine(animatorOnOff());
        animator.SetTrigger("FadeOut");
        Application.Quit();
    }
}
