using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMonologue : MonoBehaviour
{
    public List<Dialogue> dialogues; // ���� ������ ����� ������ ���� ����Ʈ
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
            Debug.LogError("PlayerMonologue: �ʿ��� ������Ʈ�� �������� �ʾҽ��ϴ�.");
            return;
        }
        
        int deathCount = playerAttributesManager.deathCount;
        int deathSentence = dialogues[0].sentences.Length;

        if(deathCount >= deathSentence)
        {
            deathCount = deathSentence - 1;
        }


        // deathCount��ŭ ����Ʈ���� �ε����� �θ��� �; selectedDialogue����
        Dialogue selectedDialogue = new Dialogue
        {
            name = dialogues[0].name,
            sentences = new string[] { dialogues[0].sentences[deathCount] }
        };

        dialogueManager.StartMonologueDialogue(selectedDialogue);
    }

   
}
