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
    
    HP��� ������ �� ���Ϻ�ȭ

    100% ~ 65% = �⺻�ӵ� �÷��̾� ����, �Ǽհ���
    
    30~65% = ��ȯ�� ���� (����), �ӵ�����, ���ݷ� ����
    
    0~30% = ���ϰ� ��ȯ ( �����̸� ���� hp�ȴ��� ) , ��ȯ�� �� ����
    
    ���� LockOn�� �����̳� �׽�Ʈ �� �� �غ����ҵ�
    
    EnemyAttributesManager �ٿ��ְ� , Layer���� ��������

    ------------------------------
    �ָ�(11/02)���� TODO

    1. ������

    ���½ý���, ����, ����, ���ݱ��� �����Ϸ�.
    (1) ���� HP, ������ ����.
    ������ �� �������� �پ�ȭ
    ��ȯ�� �� ��ȯ�Ǵ��� üũ.

    ������ ���� �տ��� �ݶ��̴� �־����. �̰ɷ� ��� ���� ���.

    2. ������ī��, ������ǰ �߰�. // �ָ������� ���� �ּ� �����

    �߸� �� �� �ؾ��ϴ� ��.
    �������۾� �� �ϱ�. �Ҹ�ȿ�� �ֱ�. �����ΰɷ�
    */

    public event System.Action Ondeath;
    
    [Header("���� �Ӽ�")]
    [SerializeField] private int bossMaxHp = 2000;
    private int currentHp;
    [SerializeField] private int bossAtk = 40;
    [SerializeField] private int bossMoveSpeed = 15;

    [Header("���� �� Ž������ ����")]
    public float attackRange = 5f;
    public float sightRange = 20f;
    private bool playerInSightRange, playerInAttackRange;
    public LayerMask playerLayerMask;

    //Attacking
    [SerializeField]
    private float timeBetweenAttacks = 1.1f;
    bool alreadyAttacked;


    [Header("��ȯ����")]
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
        // areaMask�� 1�� Built-in Walkable�̴�
        // -1�� AllAreas�ε� Walkable�� �������� ���� ȥ�� �������� ������ ������ �����Ǿ��־ �׳� AllAreas�� ����
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
        // ���� ��� ����

        // OnDeath �̺�Ʈ ȣ��
        Ondeath?.Invoke();
    }
}
