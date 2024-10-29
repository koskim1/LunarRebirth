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

    ------------------------------
    주말(11/02)까지 TODO

    1. 보스쪽

    락온시스템, 정찰, 추적, 공격까진 설정완료.
    (1) 이제 HP, 데미지 연동.
    페이즈 별 공격패턴 다양화
    소환수 잘 소환되는지 체크.

    공격을 위한 손에만 콜라이더 넣어놨음. 이걸로 어떻게 할지 고민.

    2. 레벨업카드, 상점물품 추가. // 주말지나면 여기 주석 지우기

    발매 전 꼭 해야하는 것.
    폴리싱작업 꼭 하기. 소리효과 넣기. 찰떡인걸로
    */

    public event System.Action Ondeath;
    
    [Header("보스 속성")]
    [SerializeField] private int bossMaxHp = 2000;
    private int currentHp;
    [SerializeField] private int bossAtk = 40;
    [SerializeField] private int bossMoveSpeed = 15;

    [Header("공격 및 탐색범위 설정")]
    public float attackRange = 5f;
    public float sightRange = 20f;
    private bool playerInSightRange, playerInAttackRange;
    public LayerMask playerLayerMask;

    //Attacking
    [SerializeField]
    private float timeBetweenAttacks = 1.1f;
    bool alreadyAttacked;


    [Header("소환관련")]
    public GameObject spawnSkeletonPrefab;
    public GameObject spawnGolemPrefab;

    [Header("Patroling")]
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 20f;

    private BossPhase currentBossPhase;   

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Transform player;


    private void Awake()
    {
        currentHp = bossMaxHp;
        currentBossPhase = BossPhase.Phase1;

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    public bool IsBoss { get { return true; } }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
         
        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            navMeshAgent.SetDestination(walkPoint);

        Vector3 distanceToWalk = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalk.magnitude < 1f)
            walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;
        // NavMesh.SamplePosition(Vector3 sourcePosition, out hit, float maxDistance, int areaMask)
        // areaMask의 1은 Built-in Walkable이다
        // -1은 AllAreas인데 Walkable로 했을때는 가끔 혼자 갈수없는 구역에 가려고 정지되어있어서 그냥 AllAreas로 수정
        if (NavMesh.SamplePosition(walkPoint, out hit, walkPointRange, NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();
            if (navMeshAgent.CalculatePath(hit.position, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    walkPoint = hit.position;
                    walkPointSet = true;
                }
            }
        }

    }

    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.speed = 8f;
    }

    private void AttackPlayer()
    {
        //navMeshAgent.SetDestination(transform.position);
        navMeshAgent.isStopped = true;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);

        if (!alreadyAttacked)
        {
            PerformBossAttack();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }        
    }

    private void PerformBossAttack()
    {
        animator.SetBool("canAttack", true);
    }

    private void ResetAttack()
    {
        animator.SetBool("canAttack", false);
        alreadyAttacked = false;

        navMeshAgent.isStopped = false;
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
