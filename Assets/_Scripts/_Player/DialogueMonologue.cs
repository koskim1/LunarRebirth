using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMonologue : MonoBehaviour
{
    public List<Dialogue> dialogues; // 죽을 때마다 출력할 대사들을 담은 리스트
    private DialogueManager dialogueManager;
    private PlayerAttributesManager playerAttributesManager;
    private PlayerController playerController;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerAttributesManager = FindObjectOfType<PlayerAttributesManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {        
        PlayerMonologue();
    }

    private void PlayerMonologue()
    {
        if (dialogueManager == null || playerAttributesManager == null)
        {
            Debug.LogError("PlayerMonologue: 필요한 컴포넌트가 설정되지 않았습니다.");
            return;
        }
        
        int deathCount = playerAttributesManager.deathCount;
        int deathSentence = dialogues[0].sentences.Length;

        if(deathCount >= deathSentence)
        {
            deathCount = deathSentence - 1;
        }


        // deathCount만큼 리스트안의 인덱스를 부르고 싶어서 selectedDialogue생성
        Dialogue selectedDialogue = new Dialogue
        {
            name = dialogues[0].name,
            sentences = new string[] { dialogues[0].sentences[deathCount] }
        };

        dialogueManager.StartMonologueDialogue(selectedDialogue);
    }

   
}
