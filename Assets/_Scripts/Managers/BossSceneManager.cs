using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class BossSceneManager : MonoBehaviour
{
    public PlayableDirector bossSceneTimeline;
    public Boss boss;
    public GameObject openingBoss;
    public GameObject GolemBoss;

    void Awake()
    {
        Debug.Log("보스씬 매니져 Start부분");
        boss.StopBossMovement();        
        if(bossSceneTimeline != null)
        {
            Debug.Log("플레이부분");
            bossSceneTimeline.Play();
            bossSceneTimeline.stopped += OnTimelineEnd;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTimelineEnd(PlayableDirector director)
    {
        if (director == bossSceneTimeline) // 타임라인 종료 시 모놀로그 실행
        {
            BossCinematicEnd();
        }
    }

    public void BossCinematicEnd()
    {
        UIManager.Instance.BossUI.SetActive(true);
        GameManager.Instance.TogglePlayerMovement(true);
        GolemBoss.SetActive(true);
        boss.ActivateBossMovement();
        openingBoss.SetActive(false);        
        Debug.Log("보스씨네마틱 끝");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManagers.Instance.LoadBossRoom();
        }
    }
}
