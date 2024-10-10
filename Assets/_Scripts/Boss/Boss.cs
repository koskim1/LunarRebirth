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

    */

    public event System.Action Ondeath;
    
    [Header("���� �Ӽ�")]
    [SerializeField] private int bossMaxHp = 2000;
    private int currentHp;
    [SerializeField] private int bossAtk = 40;
    [SerializeField] private int bossMoveSpeed = 15;

    [Header("��ȯ����")]
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
        // ���� ��� ����

        // OnDeath �̺�Ʈ ȣ��
        Ondeath?.Invoke();
    }
}
