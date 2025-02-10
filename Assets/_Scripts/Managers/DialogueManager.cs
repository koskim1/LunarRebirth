using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.InputSystem;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private PlayerController playerController;
    [SerializeField]private CinemachineFreeLook cinemachineFreeLook;
    [SerializeField]private CinemachineFreeLook enemyLook;
    [SerializeField]private CinemachineVirtualCamera afterDeadCam;

    public Animator animator;

    public Queue<string> sentences;
    
    private Transform currentNPC;
    [SerializeField] private GameObject playerInteractUI;
    private DialogueBoxController dialogueBoxController;

    private Tuto_Npc tutorialNpc;
    private bool isShopDialogue = false;
    private bool isTutoDialogue = false;
    private bool isTyping = false; // 문장 출력 중인지 확인
    private string currentSentence = ""; // 현재 출력 중인 문장 저장


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerController = FindObjectOfType<PlayerController>();
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        tutorialNpc = FindObjectOfType<Tuto_Npc>();
        sentences = new Queue<string>();

        Cursor.visible = false;
    }

    void Update()
    {
        // 스페이스바 입력 처리
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextSentence();
        }
    }

    // 버튼에서 호출할 메서드
    public void OnNextButtonClicked()
    {
        HandleNextSentence();
    }

    // 스페이스바와 버튼에서 공통으로 사용할 메서드
    private void HandleNextSentence()
    {
        if (isTyping)
        {
            // 문장 출력 중이면 즉시 완료
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
        }
        else
        {
            // 문장 출력이 완료되었으면 다음 문장으로 넘기기
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue, DialogueTrigger npc)
    {
        // 대화 시작 시 플레이어에게 대화 상태 알림
        playerController.SetDialogueState(true);

        isTutoDialogue = true;
        StartCoroutine(StartDialogueRoutine(dialogue, npc));
    }

    public void StartShopDialogue(Dialogue dialogue, ShopNpc npc)
    {
        // 대화 시작 시 플레이어에게 대화 상태 알림
        playerController.SetDialogueState(true);

        isShopDialogue = true;
        StartCoroutine(StartShopDialogueRoutine(dialogue, npc));
    }

    public void StartMonologueDialogue(Dialogue dialogue)
    {
        // 대화 시작 시 플레이어에게 대화 상태 알림
        playerController.SetDialogueState(true);

        StartCoroutine(StartMonologueDialogueRoutine(dialogue));
    }

    private IEnumerator StartDialogueRoutine(Dialogue dialogue, DialogueTrigger npc)
    {
        // 로컬라이즈 문자열 로딩
        yield return StartCoroutine(LoadLocalizedDialogue(dialogue));

        dialogueBoxController.gameObject.SetActive(true);
        playerController.canMove = false;
        GameManager.Instance.TogglePlayerMovement(false);
        Cursor.visible = true;
        afterDeadCam.enabled = true;
        cinemachineFreeLook.enabled = false;

        if (enemyLook != null && npc != null)
        {
            enemyLook.enabled = true;
            enemyLook.m_LookAt = npc.transform;
            enemyLook.m_XAxis.m_MaxSpeed = 0.0f;
            enemyLook.m_YAxis.m_MaxSpeed = 0.0f;
        }
        else
        {
            Debug.LogWarning("NPC 또는 Cinemachine LookAt 설정 문제");
        }

        animator.SetBool("IsOpen", true);

        DisplayNextSentence();
        ShowInteractionText(false);
    }

    private IEnumerator StartShopDialogueRoutine(Dialogue dialogue, ShopNpc npc)
    {
        // 로컬라이즈 문자열 로딩
        yield return StartCoroutine(LoadLocalizedDialogue(dialogue));

        dialogueBoxController.gameObject.SetActive(true);
        playerController.canMove = false;
        GameManager.Instance.TogglePlayerMovement(false);
        Cursor.visible = true;
        afterDeadCam.enabled = true;
        cinemachineFreeLook.enabled = false;

        if (enemyLook != null && npc != null)
        {
            enemyLook.enabled = true;
            enemyLook.m_LookAt = npc.transform;
            enemyLook.m_XAxis.m_MaxSpeed = 0.0f;
            enemyLook.m_YAxis.m_MaxSpeed = 0.0f;
        }
        else
        {
            Debug.LogWarning("ShopNpc 또는 Cinemachine LookAt 설정 문제");
        }

        animator.SetBool("IsOpen", true);

        DisplayNextSentence();
        ShowInteractionText(false);
    }

    private IEnumerator StartMonologueDialogueRoutine(Dialogue dialogue)
    {
        // 로컬라이즈 문자열 로딩
        yield return StartCoroutine(LoadLocalizedDialogue(dialogue));

        dialogueBoxController.gameObject.SetActive(true);
        playerController.canMove = false;
        GameManager.Instance.TogglePlayerMovement(false);
        cinemachineFreeLook.enabled = false;
        enemyLook.enabled = false;
        afterDeadCam.enabled = true;
        Cursor.visible = true;

        animator.SetBool("IsOpen", true);

        DisplayNextSentence();
    }

    // Localization 키를 이용해 실제 문자열을 가져오는 코루틴
    private IEnumerator LoadLocalizedDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        // 이름 로딩
        var nameHandle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("MyTable", dialogue.name);
        yield return nameHandle;
        nameText.text = nameHandle.Result;

        // 각 문장 로딩
        foreach (string sentenceKey in dialogue.sentences)
        {
            var sentenceHandle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("MyTable", sentenceKey);
            yield return sentenceHandle;
            sentences.Enqueue(sentenceHandle.Result);
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();


        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));        
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; // 문장 출력 시작
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false; // 문장 출력 완료
    }

    void EndDialogue()
    {
        // 대화 시작 시 플레이어에게 대화 상태 알림
        playerController.SetDialogueState(false);

        playerController.canMove = true;
        GameManager.Instance.TogglePlayerMovement(true);
        cinemachineFreeLook.enabled = true;
        enemyLook.enabled = true;
        afterDeadCam.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        
        animator.SetBool("IsOpen", false);

        if (isShopDialogue)
        {
            dialogueBoxController.gameObject.SetActive(false);
            ShopManager.Instance.OpenShop();
            isShopDialogue = false;
        }
        if (isTutoDialogue)
        {
            dialogueBoxController.gameObject.SetActive(false);
            tutorialNpc.OpenTutorial();
            isTutoDialogue = false;
        }

    }

    public void ShowInteractionText(bool show)
    {
        if (playerInteractUI != null)
        {
            playerInteractUI.SetActive(show);
        }
    }
}