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
    private CinemachineBrain cinemachineBrain;

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
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        sentences = new Queue<string>();

        Cursor.visible = false;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        playerController.canMove = false;

        Cursor.visible = true;

        cinemachineBrain.enabled = false;

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
        cinemachineBrain.enabled = false;

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
        cinemachineBrain.enabled = true;
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