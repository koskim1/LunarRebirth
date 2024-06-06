using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    private Queue<string> sentences;
    private DialogueTrigger currentDialogueTrigger;

    public GameObject interactionPrefab; // 말풍선 UI프리팹
    public GameObject interactionUIInstance; // 동적으로 생성된 UI 인스턴스
    private Transform currentNPC;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
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
        animator.SetBool("IsOpen", false);

        Debug.Log("End of conversation");

        ShowInteractionText(true);
    }

    public void ShowInteractionText(bool show)
    {
        if (show)
        {
            if (interactionUIInstance != null && currentNPC != null)
            {
                Debug.Log("interactionUIInstance == null && currentNPC != null");
                interactionUIInstance = Instantiate(Resources.Load<GameObject>("Press To Talk"), currentNPC);
                interactionUIInstance.transform.localPosition = new Vector3(0, 2, 0); // NPC 머리 위로 위치 조정
            }
            if (interactionUIInstance != null)
            {
                Debug.Log("interactionUIInstance != null");
                interactionUIInstance = Instantiate(Resources.Load<GameObject>("Press To Talk"), currentNPC);
                interactionUIInstance.transform.localPosition = new Vector3(0, 2, 0); // NPC 머리 위로 위치 조정
                interactionUIInstance.SetActive(true);
            }
        }
        else
        {
            if (interactionUIInstance != null)
            {
                interactionUIInstance.SetActive(false);
            }
        }
    }
}