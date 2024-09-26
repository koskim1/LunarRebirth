using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttributesManager : AttributesManager
{
    public event System.Action Ondeath;
    private EnemyAnimation enemyAnimation;
    public HealthBar healthBar;
    public GameObject FloatingTextPrefab;
    public RoomBehaviour currentRoom;
    public GameObject SoulPrefab;

    private EnemyAI enemyAI;

    private float damageReduction = 0.5f;

    // Start is called before the first frame update
    protected override void Start()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();        
        enemyAI = GetComponent<EnemyAI>();

        base.Start();
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
    }

    // Damage�κ�
    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        EnemyAI enemyAI = GetComponent<EnemyAI>();
        if(enemyAI != null && enemyAI.enemyType == EnemyAI.EnemyType.Warrior && enemyAI.isShielding)
        {
            damage = Mathf.RoundToInt(damage * damageReduction);
        }

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

        // MLP�ҿ� ���� ( ������� ���� )
        SpawnMLPSoul();

        currentRoom.OnEnemyKilled(gameObject);
        Ondeath?.Invoke();
        isDead = true;

        if (GameObject.FindWithTag("Player") != null)
        {
            PlayerAttributesManager player = GameObject.FindWithTag("Player").GetComponent<PlayerAttributesManager>();
            if (player != null)
            {
                player.GainXP(_xp); // ����ġ ���� ȣ��
                //player.AddMLP(enemyAI.mlpValue);
            }
        }
    }    

    private void SpawnMLPSoul()
    {
        if (SoulPrefab != null)
        {
            GameObject soulPrefab = Instantiate(SoulPrefab, new Vector3(transform.position.x, 0.9f, transform.position.z), Quaternion.identity);
            SoulToMLP soulToMLP = soulPrefab.GetComponent<SoulToMLP>();
            if(soulToMLP != null)
            {
                soulToMLP.mlpValue = enemyAI.mlpValue;
            }
        }
    }
}
