using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private DialogueManager dialogueManager;

    bool hasPlayer = false;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager == null)
        {
            Debug.LogError("dialogueManager이 설정되지 않았습니다.");
        }
    }

    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue);
    }

    private void Update()
    {
        if(hasPlayer && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("hasPlayer = true, TriggerDialogue 실행");
            TriggerDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = true;

            dialogueManager.ShowInteractionText(true);// 플레이어가 상호작용 가능한 범위에 들어오면
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = false;
            dialogueManager.ShowInteractionText(false);
        }
    }
}