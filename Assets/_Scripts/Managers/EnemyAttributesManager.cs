using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttributesManager : AttributesManager, ILockOnTarget
{
    public event System.Action Ondeath;
    private EnemyAnimation enemyAnimation;
    public HealthBar healthBar;
    public GameObject FloatingTextPrefab;
    public RoomBehaviour currentRoom;
    public GameObject SoulPrefab;

    private EnemyAI enemyAI;

    public bool isBossGateKeeper = false;
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

    // Damage부분
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

    public Transform GetTransform()
    {
        return this.transform;
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

        // MLP소울 생성 ( 상점기능 위해 )
        SpawnMLPSoul();

        if(currentRoom != null)
        {
            currentRoom.OnEnemyKilled(gameObject);
        }

        if (isBossGateKeeper)
        {
            StartCoroutine(WaitForGoingBossRoomText());
        }

        Ondeath?.Invoke();
        isDead = true;

        if (GameObject.FindWithTag("Player") != null)
        {
            PlayerAttributesManager player = GameObject.FindWithTag("Player").GetComponent<PlayerAttributesManager>();
            if (player != null)
            {
                player.GainXP(_xp); // 경험치 증가 호출
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

    public IEnumerator WaitForGoingBossRoomText()
    {
        UIManager.Instance.GoingToBossRoomText.SetActive(true);
        yield return new WaitForSeconds(8f);
        BossGateKeeperDead();
    }

    public void BossGateKeeperDead()
    {
        SceneManagers.Instance.LoadBossRoom();
        UIManager.Instance.GoingToBossRoomText.SetActive(false);
    }

    public bool IsBoss { get { return false; } }
}
