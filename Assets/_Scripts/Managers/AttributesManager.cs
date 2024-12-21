using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class AttributesManager : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] public int _health;
    [SerializeField] public int _attack = 20;
    [SerializeField] public int _xp = 20;
    public bool isDead = false;

    protected virtual void Start()
    {
        _health = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if(isDead) return;

        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    public void DealDamage(GameObject target)
    {
        var enemy = target.GetComponent<EnemyAttributesManager>();
        if(enemy != null)
        {
            enemy.TakeDamage(_attack);
            // 적이 죽어도 계속때리면 xp가 애니메이션 끝날때까지 얻어지던거 수정
            if(enemy._health <= 0 && !enemy.isDead)
            {
                GainXP(enemy._xp);
                enemy.isDead = true;
            }

            if (!enemy.isDead && PlayerAttributesManager.Instance.hasLifeSteal) 
                PlayerAttributesManager.Instance.HitAndRecovery(PlayerAttributesManager.Instance.lifeStealAmount);
        }

        var player = target.GetComponent<PlayerAttributesManager>();
        if(player != null)
        {
            player.TakeDamage(_attack);
        }
    }

    public virtual void GainXP(int xp)
    {
        // Override in PlayerAttributesManager to handle gaining XP
    }

    protected virtual void Die()
    {
        isDead = true;
    }
}
