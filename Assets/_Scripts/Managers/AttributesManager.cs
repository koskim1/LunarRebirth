using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class AttributesManager : MonoBehaviour
{
    // TODO
    // 죽으면 바로 파괴되지않고 애니메이션 실행 후 사망하는 연출.
    // 위에거 했고 이제 죽으면 콜라이더 꺼줘야할듯,, fadeout기능 넣어주면 최고  
    // 대화시스템 관리
    // XP 관리 & Level관리
    // UI 연결
    // 맵 디자인


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
            // 적이 죽어도 계속때리면 xp가 애니메이션 끝날때까지 얻어지던거 수정
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
