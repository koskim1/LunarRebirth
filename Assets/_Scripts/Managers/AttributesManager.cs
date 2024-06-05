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

    // Health���� �Լ�
    //public HealthBar healthBar;
    //private PlayerAnimation playerAnimation;
    //private EnemyAnimation enemyAnimation;

    public int maxHealth = 100;
    [SerializeField] protected int _health;
    [SerializeField] protected int _attack = 20;

    //// XP���� �Լ�
    //public int currentLevel = 1;
    //public int currentXP = 0;
    //public int xpToNextLevel = 100;

    //// UI ���� ����
    //public Slider xpSlider;
    //public TextMeshProUGUI levelText;

    protected virtual void Start()
    {
        // enemyAnimation = GetComponent<EnemyAnimation>();
        _health = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);

        // UI �ʱ�ȭ
        //UpdateXPUI();
    }

    protected virtual void TakeDamage(int damage)
    {
        _health -= damage;

        //healthBar.SetHealth(_health);
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

    protected virtual void Die()
    {
        //if (enemyAnimation != null)
        //{
        //    enemyAnimation.Dead();
        //}
        //else
        //{
        //    Debug.LogError("EnemyAnimation component not found");
        //    Destroy(gameObject);
        //}

        Destroy(gameObject);
    }

    
}
