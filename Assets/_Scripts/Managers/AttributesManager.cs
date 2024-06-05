using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class AttributesManager : MonoBehaviour
{
    // TODO
    // ������ �ٷ� �ı������ʰ� �ִϸ��̼� ���� �� ����ϴ� ����.
    // ������ �߰� ���� ������ �ݶ��̴� ������ҵ�,, fadeout��� �־��ָ� �ְ�  
    // ��ȭ�ý��� ����
    // XP ���� & Level����
    // UI ����
    // �� ������


    public int maxHealth = 100;
    [SerializeField] protected int _health;
    [SerializeField] protected int _attack = 20;
    [SerializeField] protected int _xp = 20;
    protected bool isDead = false;

    protected virtual void Start()
    {
        _health = maxHealth;
    }

    protected virtual void TakeDamage(int damage)
    {
        if(isDead) return;

        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    public void DealDamage(GameObject target)
    {
        var enemy = target.GetComponent<EnemyAttributesManager>();
        if(enemy != null)
        {
            enemy.TakeDamage(_attack);
            // ���� �׾ ��Ӷ����� xp�� �ִϸ��̼� ���������� ��������� ����
            if(enemy._health <= 0 && !enemy.isDead)
            {
                GainXP(enemy._xp);
                enemy.isDead = true;
            }
        }

        var player = target.GetComponent<PlayerAttributesManager>();
        if(player != null)
        {
            player.TakeDamage(_attack);
        }
    }

    protected virtual void GainXP(int xp)
    {
        // Override in PlayerAttributesManager to handle gaining XP
    }

    protected virtual void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    
}
