using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject FloatingTextPrefab;

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
            var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
            go.GetComponent<TextMeshPro>().text = damage.ToString();
        }
        
        healthBar.SetHealth(_health);
    }

    private void ShowFloatingText()
    {

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
