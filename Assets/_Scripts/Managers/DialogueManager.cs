using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
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
    // Start is called before the first frame update
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
        sentences = new Queue<string>();

        Cursor.visible = false;
    }

    public void StartDialogue(Dialogue dialogue, DialogueTrigger npc)
    {
        playerController.canMove = false;

        Cursor.visible = true;
        afterDeadCam.enabled = true;
        cinemachineFreeLook.enabled = false;
        // NPC에게 LookAt 설정
        if (enemyLook != null)
        {            
            if (npc != null)
            {
                enemyLook.enabled = true;
                enemyLook.m_LookAt = npc.transform;
                enemyLook.m_XAxis.m_MaxSpeed = 0.0f;
                enemyLook.m_YAxis.m_MaxSpeed = 0.0f;
            }
            else
            {
                Debug.LogWarning("DialogueTrigger 컴포넌트를 NPC에서 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Cinemachine LookAt 카메라가 설정되지 않았습니다.");
        }
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

        ShowInteractionText(false);
    }

    public void StartMonologueDialogue(Dialogue dialogue)
    {
        playerController.canMove = false;
        cinemachineFreeLook.enabled = false;
        enemyLook.enabled = false;
        afterDeadCam.enabled = true;
        Cursor.visible = true;

        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
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
        cinemachineFreeLook.enabled = true;
        enemyLook.enabled = true;
        afterDeadCam.enabled = false;
        Cursor.visible = false;        

        animator.SetBool("IsOpen", false);
        //ShowInteractionText(true);
        Debug.Log("End of conversation");
    }

    public void ShowInteractionText(bool show)
    {
        if (show)
        {
            if (playerInteractUI != null)
            {
                playerInteractUI.SetActive(true);
            }
        }
        else
        {
            playerInteractUI.SetActive(false);
        }
    }
}