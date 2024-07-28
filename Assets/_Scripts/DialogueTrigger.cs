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
            Debug.LogError("dialogueManager�� �������� �ʾҽ��ϴ�.");
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
            Debug.Log("hasPlayer = true, TriggerDialogue ����");
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach(Collider collider in colliderArray)
            {
                if(collider.TryGetComponent(out PlayerController player))
                {
                    TriggerDialogue();
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = true;


            // �÷��̾ ��ȣ�ۿ� ������ ������ ������
            // Press E to talk ó�� ��ȣ�ۿ밡���ϴٰ� UI ����
            dialogueManager.ShowInteractionText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = false;

            // �÷��̾ ��ȣ�ۿ� �������� �־�����
            // ��ȣ�ۿ� UI �����
            dialogueManager.ShowInteractionText(false);
        }
    }
}