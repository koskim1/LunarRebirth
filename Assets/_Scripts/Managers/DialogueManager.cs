using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
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

    public void StartDialogue(Dialogue dialogue, DialogueTrigger npc)
    {
        isTutoDialogue = true;
        StartCoroutine(StartDialogueRoutine(dialogue, npc));
    }

    public void StartShopDialogue(Dialogue dialogue, ShopNpc npc)
    {
        isShopDialogue = true;
        StartCoroutine(StartShopDialogueRoutine(dialogue, npc));
    }

    public void StartMonologueDialogue(Dialogue dialogue)
    {
        StartCoroutine(StartMonologueDialogueRoutine(dialogue));
    }

    private IEnumerator StartDialogueRoutine(Dialogue dialogue, DialogueTrigger npc)
    {
        // ���ö����� ���ڿ� �ε�
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
            Debug.LogWarning("NPC �Ǵ� Cinemachine LookAt ���� ����");
        }

        animator.SetBool("IsOpen", true);

        DisplayNextSentence();
        ShowInteractionText(false);
    }

    private IEnumerator StartShopDialogueRoutine(Dialogue dialogue, ShopNpc npc)
    {
        // ���ö����� ���ڿ� �ε�
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
            Debug.LogWarning("ShopNpc �Ǵ� Cinemachine LookAt ���� ����");
        }

        animator.SetBool("IsOpen", true);

        DisplayNextSentence();
        ShowInteractionText(false);
    }

    private IEnumerator StartMonologueDialogueRoutine(Dialogue dialogue)
    {
        // ���ö����� ���ڿ� �ε�
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

    // Localization Ű�� �̿��� ���� ���ڿ��� �������� �ڷ�ƾ
    private IEnumerator LoadLocalizedDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        // �̸� �ε�
        var nameHandle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("MyTable", dialogue.name);
        yield return nameHandle;
        nameText.text = nameHandle.Result;

        // �� ���� �ε�
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

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void EndDialogue()
    {
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