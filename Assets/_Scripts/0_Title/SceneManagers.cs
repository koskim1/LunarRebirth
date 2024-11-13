using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public static SceneManagers Instance;
    private Animator animator;
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

        LoadingSceneController.LoadScene("MainStory");
        animator.SetTrigger("FadeOut");
    }

    public void LoadMainRoom()
    {
        StartCoroutine(animatorOnOff());
        Time.timeScale = 1f;
        LoadingSceneController.LoadScene("MainRoom");
        animator.SetTrigger("FadeOut");
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //if(player != null)
        //{
        //    player.transform.position = new Vector3(0, 10f, -13f);
        //    PlayerAttributesManager.Instance._health = PlayerAttributesManager.Instance.maxHealth;
        //}

        // �� �ε� �Ŀ� Player �ʱ�ȭ
        StartCoroutine(InitializePlayer());

    }

    public void LoadDungeon()
    {
        StartCoroutine(animatorOnOff());
        LoadingSceneController.LoadScene("GameScene");
        animator.SetTrigger("FadeOut");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0, 0f, -10f);
        }
    }

    public void LoadTitleScene()
    {
        StartCoroutine(animatorOnOff());
        animator.SetTrigger("FadeOut");

        // Player ������Ʈ�� ��Ȱ��ȭ
        if (PlayerAttributesManager.Instance != null)
        {
            PlayerAttributesManager.Instance.gameObject.SetActive(false);
        }

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
            PlayerAttributesManager.Instance._health = PlayerAttributesManager.Instance.maxHealth;
            PlayerAttributesManager.Instance.healthBar.SetHealth(PlayerAttributesManager.Instance._health);
        }
    }
}