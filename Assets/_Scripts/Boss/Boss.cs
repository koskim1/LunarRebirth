using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BossPhase
{
    Phase1,
    Phase2,
    Phase3
}

public class Boss : MonoBehaviour, ILockOnTarget
{
    /*
    
    HP기반 페이즈 별 패턴변화

    100% ~ 65% = 기본속도 플레이어 추적, 맨손공격
    
    30~65% = 소환수 생성 (마법), 속도증가, 공격력 증가
    
    0~30% = 부하골렘 소환 ( 안죽이면 보스 hp안닳음 ) , 소환수 더 생성
    
    보스 LockOn은 시점이나 테스트 좀 더 해봐야할듯
    
    EnemyAttributesManager 붙여주고 , Layer설정 잊지말기

    */

    public event System.Action Ondeath;
    
    [Header("보스 속성")]
    [SerializeField] private int bossMaxHp = 2000;
    private int currentHp;
    [SerializeField] private int bossAtk = 40;
    [SerializeField] private int bossMoveSpeed = 15;

    [Header("소환관련")]
    public GameObject spawnSkeletonPrefab;
    public GameObject spawnGolemPrefab;


    private BossPhase currentBossPhase;   

    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;


    private void Start()
    {
        currentHp = bossMaxHp;
        currentBossPhase = BossPhase.Phase1;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    public bool IsBoss { get { return true; } }

    private void Update()
    {
        
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    private void Die()
    {
        // 보스 사망 로직

        // OnDeath 이벤트 호출
        Ondeath?.Invoke();
    }
}
