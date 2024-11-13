using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum BossPhase
{
    WaitingToStart,
    Phase1,
    Phase2,
    Phase3
}
public class Boss : MonoBehaviour, ILockOnTarget
{
    /*    
    11/04 TODO
    UpdateBossBehaviorForPhase() ���� ������, ������ ������. ���� ������ �ø��� ���� �ְ� �������ؾ���
    
    HP��� ������ �� ���Ϻ�ȭ

    100% ~ 65% = �⺻�ӵ� �÷��̾� ����, �Ǽհ���
    
    30~65% = ��ȯ�� ���� (����), �ӵ�����, ���ݷ� ����
    
    0~30% = ���ϰ� ��ȯ ( �����̸� ���� hp�ȴ��� ) , ��ȯ�� �� ����
    
    ���� LockOn�� �����̳� �׽�Ʈ �� �� �غ����ҵ�
    
    EnemyAttributesManager �ٿ��ְ� , Layer���� ��������

    ------------------------------
    1. ������
    2������ ��ȯ�� �۾�


    2. ������ī��, ������ǰ �߰�. // �ָ������� ���� �ּ� �����

    �߸� �� �� �ؾ��ϴ� ��.
    �������۾� �� �ϱ�. �Ҹ�ȿ�� �ֱ�. �����ΰɷ�
    */



    public event System.Action Ondeath;
    
    [Header("���� �Ӽ�")]
    [SerializeField] private int bossMaxHp = 2000;
    [SerializeField] private int currentHp;
    [SerializeField] private int bossAtk = 40;
    // navMeshAgent�ӵ��� �����ӵ� ���� ��
    // [SerializeField] private int bossMoveSpeed = 15;

    [Header("���� �� Ž������ ����")]
    public float attackRange = 5f;
    public float sightRange = 20f;
    private bool playerInSightRange, playerInAttackRange;
    public LayerMask playerLayerMask;

    //Attacking
    private float timeBetweenAttacks = 2f;
    bool alreadyAttacked;

    [Header("Patroling")]
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 20f;

    private BossPhase currentBossPhase;   

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Transform player;

    [SerializeField]
    private BoxCollider boxCollider;

    private BossHealthBar bossHealthBar;
    private BossMinionSpawn bossMinionSpawn;
    private Coroutine spawnMinionsCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        bossHealthBar = FindAnyObjectByType<BossHealthBar>();
        bossMinionSpawn = FindAnyObjectByType<BossMinionSpawn>();
    }

    private void Start()
    {
        currentHp = bossMaxHp;
        currentBossPhase = BossPhase.WaitingToStart;

        player = GameObject.Find("Player").transform;

        StopBossMovement();
        animator.SetTrigger("bossOpening");
        Invoke(nameof(ActivateBossMovement), 2.3f);
    }

    public bool IsBoss { get { return true; } }
    private bool isBossDead = false;

    private void Update()
    {
        CheckBossPhase();

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
         
        if (!playerInSightRange && !playerInAttackRange && !isBossDead) Patrolling();
        if (playerInSightRange && !playerInAttackRange && !isBossDead) ChasePlayer();
        if (playerInSightRange && playerInAttackRange && !isBossDead) AttackPlayer();
    }

    private void CheckBossPhase()
    {        
        if(currentHp > bossMaxHp * 0.65f)
        {
            SetPhase(BossPhase.Phase1);
        }
        else if(currentHp <= bossMaxHp * 0.65f && currentHp > bossMaxHp * 0.3f)
        {
            SetPhase(BossPhase.Phase2);
        }
        else
        {
            SetPhase(BossPhase.Phase3);
        }
    }

    private void SetPhase(BossPhase phase)
    {
        if(currentBossPhase != phase)
        {
            currentBossPhase = phase;
            UpdateBossBehaviorForPhase();
        }
    }

    public void SetBossHealth()
    {
        bossHealthBar.SetMaxHealth(bossMaxHp);
    }

    public void UpdateBossHealth()
    {
        bossHealthBar.SetHealth(currentHp);
    }

    private void UpdateBossBehaviorForPhase()
    {
        switch (currentBossPhase)
        {
            case BossPhase.Phase1:
                //�⺻ ���� �� �Ǽհ���
                Debug.Log("������ 1");
                bossAtk = 1;                
                navMeshAgent.speed = 8f;
                break;
            case BossPhase.Phase2:
                // ��ȯ �� �ӵ� ����
                Debug.Log("������ 2");
                bossAtk = 50;
                navMeshAgent.speed = 12f;
                SpawnMinions();
                StartSpawnMinionsCoroutine();
                break;
            case BossPhase.Phase3:
                // ���� ��, ���� ��Ÿ�� ����(���� ��ư�)
                Debug.Log("������ 3");
                bossAtk = 80;
                //bossAtk = 0; // �׽�Ʈ
                timeBetweenAttacks = 1.1f;
                navMeshAgent.speed = 20f;
                StopSpawnMinionsCoroutine();
                break;
        }
    }

    private void Patrolling()
    {
        //ActivateBossMovement();

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            navMeshAgent.SetDestination(walkPoint);

        Vector3 distanceToWalk = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalk.magnitude < 1f)
            walkPointSet = false;

        animator.SetBool("canChasePlayer", false);
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

        animator.SetBool("canChasePlayer", true);
    }

    private void AttackPlayer()
    {
        //navMeshAgent.SetDestination(transform.position);
        StopBossMovement();

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

    public void TakeDamage(int damage)
    {        
        currentHp -= damage;
        UpdateBossHealth();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void SpawnMinions()
    {
        foreach(Transform spawnPoint in bossMinionSpawn.spawnPoints)
        {
            Instantiate(bossMinionSpawn.skeletonPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private IEnumerator SpawnMinionsBySeconds()
    {
        yield return new WaitForSeconds(6f);
        while (true)
        {
            List<Transform> spawnPoints = new List<Transform>(bossMinionSpawn.spawnPoints);
            int numSpawnPoints = Random.Range(3, 5);

            for(int i=0; i < numSpawnPoints; i++)
            {
                int randomIndex = Random.Range(0, spawnPoints.Count);
                Instantiate(bossMinionSpawn.skeletonPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            }
            // ���̵� ���� �׽�Ʈ �غ����ϱ� ��.
            yield return new WaitForSeconds(8f);
        }
    }

    private void StartSpawnMinionsCoroutine()
    {
        spawnMinionsCoroutine = StartCoroutine(SpawnMinionsBySeconds());
    }

    private void StopSpawnMinionsCoroutine()
    {
        if (spawnMinionsCoroutine != null)
        {
            StopCoroutine(spawnMinionsCoroutine);
            spawnMinionsCoroutine = null;
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

        ActivateBossMovement();
    }

    public void EnableCollider()
    {
        boxCollider.enabled = true;
    }

    public void DisableCollider()
    {
        boxCollider.enabled = false;
    }

    public void StopBossMovement()
    {
        navMeshAgent.isStopped = true;
    }

    public void ActivateBossMovement()
    {
        navMeshAgent.isStopped = false;
    }

    public int GetBossAtk()
    {
        return bossAtk;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    private void Die()
    {
        // ���� ��� ����
        StopBossMovement();
        UIManager.Instance.BossUI.SetActive(false);
        UIManager.Instance.AfterBossDeadText.SetActive(true);
        // OnDeath �̺�Ʈ ȣ��
        Debug.Log("������ ����߽��ϴ�.");
        if (isBossDead) return;
        isBossDead = true;

        StartCoroutine(DestroyAfterAnimation());
        Ondeath?.Invoke();
    }

    private IEnumerator DestroyAfterAnimation()
    {
        animator.SetBool("isDead", true);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        navMeshAgent.speed = 0;
        //healthBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(8f);

        Destroy(gameObject);
        UIManager.Instance.AfterBossDeadText.SetActive(false);
        SceneManagers.Instance.LoadTitleScene();
    }
}
