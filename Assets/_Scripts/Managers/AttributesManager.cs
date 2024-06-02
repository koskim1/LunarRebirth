using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    // TODO
    // ������ �ٷ� �ı������ʰ� �ִϸ��̼� ���� �� ����ϴ� ����.
    // ������ �߰� ���� ������ �ݶ��̴� ������ҵ�,, fadeout��� �־��ָ� �ְ�  
    // ��ȭ�ý��� ����
    // XP ���� & Level����
    // UI ����
    // �� ������

    public HealthBar healthBar;
    private PlayerAnimation playerAnimation;
    private EnemyAnimation enemyAnimation;

    public int maxHealth = 100;
    [SerializeField] int _health;
    [SerializeField] int _attack = 20;
    //[SerializeField] int _xp = 20;


    private void Start()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();


        _health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        healthBar.SetHealth(_health);
        if (_health <= 0)
        {
            Die();
        }
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AttributesManager>();
        if (atm != null)
        {
            atm.TakeDamage(_attack);
        }
    }

    private void Die()
    {
        if (enemyAnimation != null)
        {
            enemyAnimation.Dead();
        }
        else
        {
            Debug.LogError("EnemyAnimation component not found");
            Destroy(gameObject);
        }
    }
}
