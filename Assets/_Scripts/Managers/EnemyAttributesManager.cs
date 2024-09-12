using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttributesManager : AttributesManager
{
    /*
     TODO.
    1. NavMeshAgent�� AI�������ֱ�.
    2. State�������ֱ�.
     - 1 �����Ÿ��ȿ� Player���� �� ��Ȳ�ϱ�
     - 2 �����Ÿ��ȿ� ���� Player���� �ٰ�����
     - 3 ���ݻ����Ÿ� �ȿ� ���� Player �����ϱ�
    */
    public event System.Action Ondeath;
    private EnemyAnimation enemyAnimation;
    public HealthBar healthBar;
    public GameObject FloatingTextPrefab;

    public RoomBehaviour currentRoom;
    

    // Start is called before the first frame update
    protected override void Start()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();        

        base.Start();
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
    }

    // Damage�κ�
    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        base.TakeDamage(damage);

        if (FloatingTextPrefab)
        {
            ShowFloatingText(damage);
        }
        
        healthBar.SetHealth(_health);
    }

    private void ShowFloatingText(int damage)
    {
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    protected override void Die()
    {
        if(enemyAnimation != null)
        {
            enemyAnimation.Dead();
        }
        else
        {
            Destroy(gameObject);
        }

        //currentRoom.OnEnemyKilled(gameObject);

        Ondeath?.Invoke();
        isDead = true;

        if (GameObject.FindWithTag("Player") != null)
        {
            PlayerAttributesManager player = GameObject.FindWithTag("Player").GetComponent<PlayerAttributesManager>();
            if (player != null)
            {
                player.GainXP(_xp); // ����ġ ���� ȣ��
            }
        }
    }
}
