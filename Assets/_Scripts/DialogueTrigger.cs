using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private DialogueManager dialogueManager;
    private Animator npcAnimator;

    bool hasPlayer = false;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        npcAnimator = GetComponent<Animator>();

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
            // npc�� �÷��̾� �ٶ󺸰� ����
            LookAtPlayer();

            npcAnimator.SetBool("isTalking", true);


            Debug.Log("hasPlayer = true, TriggerDialogue ����");
            float interactRange = 6f;
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
            npcAnimator.SetBool("isTalking", false);
            // �÷��̾ ��ȣ�ۿ� �������� �־�����
            // ��ȣ�ۿ� UI �����
            dialogueManager.ShowInteractionText(false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = (PlayerAttributesManager.Instance.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        transform.DORotate(targetRotation.eulerAngles, 2.5f);
    }
}