using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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

    // Damage부분
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
                player.GainXP(_xp); // 경험치 증가 호출
            }
        }
    }
}
