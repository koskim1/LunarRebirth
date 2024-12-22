using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public static SceneManagers Instance;

    public Animator animator;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        animator = GetComponentInChildren<Animator>();

        Debug.Log($"�÷��̾� ����ī��Ʈ : {PlayerPrefs.GetInt("DeathCount")}");
    }

    public IEnumerator animatorOnOff()
    {
        animator.gameObject.SetActive(false);
        yield return null;
        animator.gameObject.SetActive(true);
    }

    public void LoadIntroScene()
    {
        StartCoroutine(animatorOnOff());

        PlayerPrefs.DeleteAll();
        DataManager.Instance.deathCount = 0;
        if(CameraFollow.Instance != null && UIManager.Instance != null)
        {
            CameraFollow.Instance.gameObject.SetActive(false);
            UIManager.Instance.gameObject.SetActive(false);
            Destroy(CameraFollow.Instance.gameObject);
            Destroy(UIManager.Instance.gameObject);
        }

        if(PlayerAttributesManager.Instance != null)
        {
            PlayerAttributesManager.Instance.ResetPlayerAttribute();
            PlayerAttributesManager.Instance.UpdateXPUI();
            PlayerAttributesManager.Instance.UpdateMLPUI(PlayerAttributesManager.Instance.previousMLP =0, PlayerAttributesManager.Instance.currentMLP =0);
            Destroy(PlayerAttributesManager.Instance.gameObject);
        }

        if(DialogueManager.Instance != null)
        {
            Destroy(DialogueManager.Instance.gameObject);
        }

        if(GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        MainMenu.Instance.MainMenuUI.SetActive(false);
        LoadingSceneController.LoadScene("MainStory");        
        animator.SetTrigger("FadeOut");
    }

    public void LoadMainRoom()
    {
        StartCoroutine(animatorOnOff());
        Time.timeScale = 1f;
        //PlayerAttributesManager.Instance.LoadPlayerData();
        LoadingSceneController.LoadScene("MainRoom");
        if(MainMenu.Instance != null)
        {
            MainMenu.Instance.MainMenuUI.SetActive(false);
        }
        if (UIManager.Instance != null)
        {
            UIManager.Instance.gameObject.SetActive(true);
        }
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.soundFXObject.gameObject.SetActive(true);
            AudioManager.Instance.BGM.gameObject.SetActive(true);
        }
        animator.SetTrigger("FadeOut");

        // �� �ε� �Ŀ� Player �ʱ�ȭ
        if (PlayerAttributesManager.Instance != null)
        {
            PlayerAttributesManager.Instance.transform.position = new Vector3(0, 0, -13f);
            PlayerAttributesManager.Instance.RestoreHealth();
        }
       
        StartCoroutine(InitializePlayer());

    }

    public void LoadDungeon()
    {
        StartCoroutine(animatorOnOff());
        LoadingSceneController.LoadScene("GameScene");
        animator.SetTrigger("FadeOut");
        
        PlayerAttributesManager.Instance.transform.position = new Vector3(0, 0, -10f);
    }

    public void LoadTitleScene()
    {
        StartCoroutine(animatorOnOff());
        animator.SetTrigger("FadeOut");

        Debug.Log($"�÷��̾� ����ī��Ʈ : {PlayerPrefs.GetInt("DeathCount")}");

        // Player ������Ʈ�� ��Ȱ��ȭ
        if (PlayerAttributesManager.Instance != null)
        {
            PlayerAttributesManager.Instance.SavePlayerData();
            PlayerAttributesManager.Instance.gameObject.SetActive(false);
        }
        if(MainMenu.Instance != null)
        {
            MainMenu.Instance.MainMenuUI.SetActive(true);
        }
        if (UIManager.Instance != null)
        {
            UIManager.Instance.gameObject.SetActive(false);
        }
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.soundFXObject.gameObject.SetActive(false);
            AudioManager.Instance.BGM.gameObject.SetActive(false);
        }
        //UIManager.Instance.gameObject.SetActive(false);
        LoadingSceneController.LoadScene("TitleScene");
    }

    public void LoadBossRoom()
        {
        StartCoroutine(animatorOnOff());
        animator.SetTrigger("FadeOut");
        GameManager.Instance.TogglePlayerMovement(false);
        PlayerAttributesManager.Instance.gameObject.transform.position = new Vector3(0, 0, -18);
        LoadingSceneController.LoadScene("BossScene");
    }

    public void QuitGame()
    {
        if (PlayerAttributesManager.Instance != null)
        {
            PlayerAttributesManager.Instance.SavePlayerData();            
        }

        StartCoroutine(animatorOnOff());
        animator.SetTrigger("FadeOut");
        Application.Quit();
    }

    private IEnumerator InitializePlayer()
    {
        yield return new WaitForSeconds(0.1f);

        if (PlayerAttributesManager.Instance != null)
        {
            // Player ������Ʈ Ȱ��ȭ
            PlayerAttributesManager.Instance.gameObject.SetActive(true);

            // Player Input ������Ʈ ��Ȱ��ȭ
            var playerInput = PlayerAttributesManager.Instance.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.enabled = false;
                yield return null; // �� ������ ���
                playerInput.enabled = true;
            }

            // Player ��ġ �� ���� �ʱ�ȭ
            PlayerAttributesManager.Instance.transform.position = new Vector3(0, 0f, -13f);            
            //PlayerAttributesManager.Instance._health = PlayerAttributesManager.Instance.maxHealth;
            //PlayerAttributesManager.Instance.healthBar.SetHealth(PlayerAttributesManager.Instance._health);
        }
    }
}