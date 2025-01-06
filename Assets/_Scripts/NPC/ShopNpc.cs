using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNpc : MonoBehaviour
{
    public Dialogue dialogue;

    private DialogueManager dialogueManager;
    private Animator npcAnimator;

    private bool hasPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        npcAnimator = GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    public void TriggerDialogue()
    {
        dialogueManager.StartShopDialogue(dialogue, this);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasPlayer && Input.GetKeyDown(KeyCode.E))
        {
            npcAnimator.SetBool("isTalking", true);
            LookAtPlayer();

            TriggerDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = true;
            // 플레이어가 상호작용 가능한 범위에 들어오면 UI활성화
            dialogueManager.ShowInteractionText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = false;
            npcAnimator.SetBool("isTalking", false);
            // 플레이어가 상호작용 범위에서 멀어지면 상호작용 비활
            dialogueManager.ShowInteractionText(false);
        }
    }


    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = (PlayerAttributesManager.Instance.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        transform.DORotate(targetRotation.eulerAngles, 1.5f);
    }
}
