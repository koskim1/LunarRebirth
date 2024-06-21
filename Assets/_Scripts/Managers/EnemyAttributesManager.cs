using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributesManager : AttributesManager
{
    /*
     TODO.
    1. NavMeshAgent로 AI설정해주기.
    2. State설정해주기.
     - 1 사정거리안에 Player없을 때 방황하기
     - 2 사정거리안에 오면 Player에게 다가가기
     - 3 공격사정거리 안에 오면 Player 공격하기
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

    // Damage부분
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
