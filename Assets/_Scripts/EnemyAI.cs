using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyAttributesManager enemyAttributesManager;
    private Animator animator;

    public Transform Player;
    public LayerMask whatIsGround, whatIsPlayer;

    public enum EnemyType
    {
        Mage,
        Minion,
        Warrior,
        Rogue,
    }

    public EnemyType enemyType;

    [Header("Mage Attack Settings")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 20f;

    [Header("Patroling")]
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    [SerializeField]
    private float timeBetweenAttacks = 1.1f;
    bool alreadyAttacked;

    [Header("States")]
    //States
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Warrior Sheild Setting")]
    public bool isShielding = false;

    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAttributesManager = GetComponent<EnemyAttributesManager>();
        animator = GetComponent<Animator>();

        Player = GameObject.Find("Player").transform;

        switch (enemyType)
        {
            case EnemyType.Mage:
                sightRange = 10f;
                attackRange = 10f;
                timeBetweenAttacks = 1.8f;
                break;
            case EnemyType.Minion:
                sightRange = 7;
                attackRange = 1.5f;
                timeBetweenAttacks = 1.6f;
                break;
            case EnemyType.Rogue:
                sightRange = 7;
                attackRange = 1.5f;
                navMeshAgent.speed = 6f;
                timeBetweenAttacks = 1.167f;
                break;
            case EnemyType.Warrior:
                sightRange = 9f;
                attackRange = 1.4f;
                timeBetweenAttacks = 1.067f;
                break;
        }
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (enemyAttributesManager.isDead) return;
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
        navMeshAgent.SetDestination(Player.position);
        navMeshAgent.speed = 6f;

        if(enemyType == EnemyType.Warrior)
        {
            animator.SetBool("isShielding", true);
            isShielding = true;
        }
    }
    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        navMeshAgent.SetDestination(transform.position);

        transform.LookAt(Player);

        if (!alreadyAttacked)
        {
            // 적 유형에 따라 공격방식 다르게
            switch (enemyType)
            {
                case EnemyType.Mage:
                    PerformMageAttack();
                    break;
                case EnemyType.Minion:
                    PerformMinionAttack();
                    break;
                case EnemyType.Rogue:
                    PerformRogueAttack();
                    break;
                case EnemyType.Warrior:
                    PerformWarriorAttack();
                    break;
            }



            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    private void PerformMageAttack()
    {
        animator.SetBool("canSpell", true);

        // 쿨다운 밸런스 조절좀 해야할듯
        Invoke(nameof(ShootFireball), 1.2f);
    }

    private void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        Vector3 direction = (Player.position - firePoint.position).normalized;
        fireball.GetComponent<Rigidbody>().velocity = direction * fireballSpeed;

        animator.SetBool("canSpell", false);
    }

    private void PerformMinionAttack()
    {
        animator.SetBool("canAttack", true);        
    }

    private void ResetAttack()
    {
        animator.SetBool("canAttack", false);
        animator.SetBool("canAxeAttack", false);
        animator.SetBool("canRogueAttack", false);
        alreadyAttacked = false;
    }

    private void PerformRogueAttack()
    {
        animator.SetBool("canRogueAttack", true);
    }

    private void PerformWarriorAttack()
    {
        isShielding = false;
        animator.SetBool("canAxeAttack", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
