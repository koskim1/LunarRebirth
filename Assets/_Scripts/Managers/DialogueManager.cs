using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private PlayerController playerController;

    public Animator animator;

    public Queue<string> sentences;
    
    private Transform currentNPC;
    [SerializeField] private GameObject playerInteractUI;
    // Start is called before the first frame update
    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        sentences = new Queue<string>();

    }

    public void StartDialogue(Dialogue dialogue)
    {
        playerController.canMove = false;
        
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