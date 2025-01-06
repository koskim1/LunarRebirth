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
        //if (introTimeline != null && PlayerAttributesManager.Instance.deathCount == 0)
        //{
        //    introTimeline.stopped += OnTimelineEnd; // Ÿ�Ӷ����� ������ ����� �Լ� ���
        //    introTimeline.Play(); // Ÿ�Ӷ��� ���
        //}else
        //{
        //    PlayerMonologue();
        //}
        if (introTimeline != null && DataManager.Instance.deathCount == 0)
        {
            introTimeline.stopped += OnTimelineEnd; // Ÿ�Ӷ����� ������ ����� �Լ� ���
            introTimeline.Play(); // Ÿ�Ӷ��� ���
        }
        else
        {
            PlayerMonologue();
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
            Debug.LogError("PlayerMonologue: ���� Ȯ�ιٶ�.");
            return;
        }
        
        int deathCount = DataManager.Instance.deathCount;
        int deathSentence = dialogues[0].sentences.Length;

        if(deathCount >= deathSentence)
        {
            deathCount = deathSentence - 1;
        }


        // deathCount��ŭ ����Ʈ���� �ε����� �θ��� �; selectedDialogue����
        // ������ü�� ������ �ƹ����� ���� �� ���� �ǻ�Ƴ��� �ɷ��̴�,, ���ö���¡ ���� ����
        Dialogue selectedDialogue = new Dialogue
        {
            name = dialogues[0].name,
            sentences = new string[] { dialogues[0].sentences[deathCount] }
        };

        dialogueManager.StartMonologueDialogue(selectedDialogue);
    }

   
}
