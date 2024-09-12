using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueMonologue : MonoBehaviour
{
    public List<Dialogue> dialogues; // ���� ������ ����� ������ ���� ����Ʈ
    private DialogueManager dialogueManager;
    private PlayerAttributesManager playerAttributesManager;
    private PlayerController playerController;
    public PlayableDirector introTimeline; // �� �ʹ� Ÿ�Ӷ���


    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerAttributesManager = FindObjectOfType<PlayerAttributesManager>();
        playerController = FindObjectOfType<PlayerController>(); 
    }

    void Start()
    {
        if (introTimeline != null)
        {
            introTimeline.stopped += OnTimelineEnd; // Ÿ�Ӷ����� ������ ����� �Լ� ���
            introTimeline.Play(); // Ÿ�Ӷ��� ���
        }
    }

    private void OnTimelineEnd(PlayableDirector director)
    {
        if (director == introTimeline) // Ÿ�Ӷ��� ���� �� ���α� ����
        {
            PlayerMonologue();
        }
    }

    private void PlayerMonologue()
    {
        if (dialogueManager == null || playerAttributesManager == null)
        {
            Debug.LogError("PlayerMonologue: �ʿ��� ������Ʈ�� �������� �ʾҽ��ϴ�.");
            return;
        }
        
        int deathCount = PlayerAttributesManager.Instance.deathCount;
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
