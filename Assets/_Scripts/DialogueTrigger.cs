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
            // npc가 플레이어 바라보게 설정
            LookAtPlayer();

            npcAnimator.SetBool("isTalking", true);


            Debug.Log("hasPlayer = true, TriggerDialogue 실행");
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
            // 플레이어가 상호작용 가능한 범위에 들어오면
            // Press E to talk 처럼 상호작용가능하다고 UI 띄우기
            dialogueManager.ShowInteractionText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = false;
            npcAnimator.SetBool("isTalking", false);
            // 플레이어가 상호작용 범위에서 멀어지면
            // 상호작용 UI 지우기
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