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
        Debug.Log("������ �Ŵ��� Start�κ�");
        boss.StopBossMovement();        
        if(bossSceneTimeline != null)
        {
            Debug.Log("�÷��̺κ�");
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
        if (director == bossSceneTimeline) // Ÿ�Ӷ��� ���� �� ���α� ����
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
        Debug.Log("�������׸�ƽ ��");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManagers.Instance.LoadBossRoom();
        }
    }
}
