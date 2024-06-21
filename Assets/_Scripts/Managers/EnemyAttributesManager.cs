using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private EnemyAnimation enemyAnimation;
    public HealthBar healthBar;

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
        healthBar.SetHealth(_health);
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

    }
}
