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

    // Health관련 함수
    //public HealthBar healthBar;
    //private PlayerAnimation playerAnimation;
    //private EnemyAnimation enemyAnimation;

    public int maxHealth = 100;
    [SerializeField] protected int _health;
    [SerializeField] protected int _attack = 20;

    //// XP관련 함수
    //public int currentLevel = 1;
    //public int currentXP = 0;
    //public int xpToNextLevel = 100;

    //// UI 관련 변수
    //public Slider xpSlider;
    //public TextMeshProUGUI levelText;

    protected virtual void Start()
    {
        // enemyAnimation = GetComponent<EnemyAnimation>();
        _health = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);

        // UI 초기화
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
