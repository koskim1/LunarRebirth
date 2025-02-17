using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueMonologue : MonoBehaviour
{
    public List<Dialogue> dialogues; // 죽을 때마다 출력할 대사들을 담은 리스트
    private DialogueManager dialogueManager;
    private PlayerAttributesManager playerAttributesManager;
    private PlayerController playerController;
    public PlayableDirector introTimeline; // 맨 초반 타임라인


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
        //    introTimeline.stopped += OnTimelineEnd; // 타임라인이 끝나면 실행될 함수 등록
        //    introTimeline.Play(); // 타임라인 재생
        //}else
        //{
        //    PlayerMonologue();
        //}
        if (introTimeline != null && DataManager.Instance.deathCount == 0)
        {
            introTimeline.stopped += OnTimelineEnd; // 타임라인이 끝나면 실행될 함수 등록
            introTimeline.Play(); // 타임라인 재생
        }
        else
        {
            PlayerMonologue();
        }
    }

    private void OnTimelineEnd(PlayableDirector director)
    {
        if (director == introTimeline) // 타임라인 종료 시 모놀로그 실행
        {
            PlayerMonologue();
        }
    }

    private void PlayerMonologue()
    {
        if (dialogueManager == null || playerAttributesManager == null)
        {
            Debug.LogError("PlayerMonologue: 참조 확인바람.");
            return;
        }
        
        int deathCount = DataManager.Instance.deathCount;
        int deathSentence = dialogues[0].sentences.Length;

        if(deathCount >= deathSentence)
        {
            deathCount = deathSentence - 1;
        }


        // deathCount만큼 리스트안의 인덱스를 부르고 싶어서 selectedDialogue생성
        // 게임자체의 컨셉이 아무래도 죽을 때 마다 되살아나는 능력이니,, 로컬라이징 잊지 말기
        Dialogue selectedDialogue = new Dialogue
        {
            name = dialogues[0].name,
            sentences = new string[] { dialogues[0].sentences[deathCount] }
        };

        dialogueManager.StartMonologueDialogue(selectedDialogue);
    }

   
}
