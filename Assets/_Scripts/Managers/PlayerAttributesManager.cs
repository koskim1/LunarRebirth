using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class PlayerAttributesManager : AttributesManager
{
    public static PlayerAttributesManager Instance;

    public PlayerHealthBar healthBar;
    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public LevelUpUI levelUpUI;
    public TextMeshProUGUI mlpText; // MLP

    private PlayerAnimation _playerAnimation;
    private PlayerController _playerController;
    private FireballController _fireballController;    
    private CardGenerator cardGenerator;
    private CardCollection cardCollection;
    private SceneManagers sceneManagers;
    private Animator animator;
    private UIManager uiManager;
    private Sword _sword;

    public float defense = 1f;
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int deathCount = 0;    
    public int currentMLP = 0;
    public int previousMLP = 0;
    private int displayedMLP = 0;
    private bool isHealthRecoveryActive = false;
    public bool hasLifeSteal = false;
    public int lifeStealAmount = 0;
    private int healthRecoveryAmount = 0;
    private List<ShopItem> purchasedItems = new List<ShopItem>();   // ������ ������ ���
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeComponents();
        }
        else if (Instance != null)
        {           
            Destroy(gameObject);
        }
        

    }
    private void InitializeComponents()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerController = GetComponent<PlayerController>();
        _fireballController = GetComponent<FireballController>();
        _sword = GetComponentInChildren<Sword>();
        animator = GetComponent<Animator>();
        cardCollection = FindAnyObjectByType<CardCollection>();
        sceneManagers = FindAnyObjectByType<SceneManagers>();
        uiManager = FindAnyObjectByType<UIManager>();
    }

    public void ResetPlayerAttribute()
    {
        base.maxHealth = 100;
        base._health = 100;
        base._attack = 20;
        base._xp = 20;
        defense = 1f;
        currentLevel = 0;
        currentXP = 0;
        xpToNextLevel = 10;
        deathCount = 0;
        currentMLP = 0;
        previousMLP = 0;
        displayedMLP = 0;
        healthRecoveryAmount = 0;
        isHealthRecoveryActive = false;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(_health);
        _sword.transform.localScale = new Vector3(1, 1, 1);
    }

    // Start is called before the first frame update
    protected override void Start()
    {                
        base.Start();        

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
        UpdateXPUI();
        UpdateMLPUI(previousMLP, currentMLP);

        if (cardCollection == null)
        {
            Debug.Log("CardCollection�� �������� �ʾҽ��ϴ�.");
        }
        cardGenerator = new CardGenerator(cardCollection);
    }

    public void AddMLP(int amount)
    {        
        previousMLP = currentMLP;
        currentMLP += amount;
        UpdateMLPUI(previousMLP, currentMLP);
        Debug.Log($"���� MLP�� {currentMLP} �Դϴ�");
    }

    public void UpdateMLPUI(int fromValue, int toValue)
    {        
        // Ȥ���� �ʹ� ������Ƽ� ���̸� �ȵǴ� ����ó��  
        DOTween.Kill("MLPCountUp");

        /*
        DOTween.To(getter, setter, to(end value), float duration) 
        */
        DOTween.To(() => fromValue, x =>
        {
            displayedMLP = x;
            mlpText.text = $"{displayedMLP}";
        }, toValue, 0.5f)
            .SetEase(Ease.OutQuad).SetId("MLPCountUp").SetUpdate(true);
    }
    private IEnumerator animatorOnOff()
    {
        animator.enabled = false;
        yield return new WaitForSeconds(0f);
        animator.enabled = true;
    }

    // Damage�κ�
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(Mathf.RoundToInt(damage * defense));
        healthBar.SetHealth(_health);
    }

    protected override void Die()
    {
        StartCoroutine(animatorOnOff());
        UIManager.Instance.BossUI.SetActive(false);
        _playerAnimation.Dead();
    }

    // XP�κ�
    public override void GainXP(int xp)
    {
        currentXP += xp;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
            UpdateXPUI();
        }
        UpdateXPUI();
    }

    public void LevelUp()
    {
        // ������ ����
        Debug.Log("���� ��!");
        currentLevel++;
        currentXP -= xpToNextLevel;
        if(currentLevel <= 10)
        {
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.4f);
        }
        else
        {
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);
        }
        
        _health = maxHealth;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(_health);

        // ī�� ����
        ScriptableCard card1 = cardGenerator.GenerateCard();
        ScriptableCard card2 = cardGenerator.GenerateCard();
        ScriptableCard card3 = cardGenerator.GenerateCard();

        // LevelUpUI.cs�� ī�� ����
        //levelUpUI.ShowLevelUpOptions(card1, card2, card3);

        // ������ UI ǥ��
        if (levelUpUI != null)
        {
            Debug.Log("������UI");
            levelUpUI.ShowLevelUpOptions(card1, card2, card3);
        }
    }

    public void ApplyCardEffect(ScriptableCard card)
    {
        // ���⼭ SO�� �̸��� ������� IncreaseStat�� �̸� �������ְ� �̸� �������
        // ������ �÷��ִ� �ý������� ���� ��.. ���ȵ� �� �߰��ؾ���!!
        switch (card.CardName)
        {
            #region Common���
            case "Health I":
                IncreaseStat("health", 20);
                break;
            case "Strength I":
                IncreaseStat("attack", 5);
                break;
            case "Range":
                Vector3 currentScale = _sword.transform.localScale;
                currentScale.y += 0.1f;
                _sword.transform.localScale = currentScale;
                break;
            case "Defense I":
                defense -= 0.03f;
                break;
            case "Recovery I":
                healthRecoveryAmount += 3;
                if (!isHealthRecoveryActive)
                { StartCoroutine(HealthRecovery()); }                
                break;
            case "Speed I":
                _playerController.AddSpeed(0.5f);
                break;
            case "LifeSteal I":
                lifeStealAmount += 2;
                break;
            #endregion

            #region Rare���
            case "Fireball":
                _fireballController.EnableFireball();
                break;
            case "SlowDash":
                _playerController.CanSlowDash();
                break;
            case "Health II":
                IncreaseStat("health", 50);
                break;
            case "Strength II":
                IncreaseStat("attack", 10);
                break;
            case "Defense II":
                defense -= 0.05f;
                break;
            case "Recovery II":
                healthRecoveryAmount += 10;
                if (!isHealthRecoveryActive)
                { StartCoroutine(HealthRecovery()); }
                break;
            case "LifeSteal II":
                lifeStealAmount += 4;
                break;
            #endregion

            // �׳� Epic�̶� ��������� �ϴ� ���Ȼ�Ƣ�⸸ ���ִ°ɷ�,,
            #region Epic���
            case "Health III":
                IncreaseStat("health", 100);
                break;
            case "Strength III":
                IncreaseStat("attack", 30);
                break;
            case "Defense III":
                defense -= 0.08f;
                break;
            case "Recovery III":
                healthRecoveryAmount += 20;
                if (!isHealthRecoveryActive)
                { StartCoroutine(HealthRecovery()); }
                break;
            case "LifeSteal III":
                lifeStealAmount += 8;
                break;
            #endregion

            #region Legendary���
            case "Health IV":
                IncreaseStat("health", 200);
                break;
            case "Strength IV":
                IncreaseStat("attack", 50);
                break;
            case "Defense IV":
                defense -= 0.1f;
                break;
            case "Recovery IV":
                healthRecoveryAmount += 40;
                if (!isHealthRecoveryActive)
                { StartCoroutine(HealthRecovery()); }
                break;
            case "LifeSteal IV":
                lifeStealAmount += 15;
                break;
                #endregion

        }
    }

    public void IncreaseStat(string statName, int amount)
    {
        Debug.Log($"IncreaseStat called with statName: {statName}, amount: {amount}");

        switch (statName)
        {
            case "attack":
                _attack += amount;
                break;
            case "health":
                maxHealth += amount;
                _health = maxHealth;
                healthBar.SetMaxHealth(maxHealth);
                healthBar.SetHealth(maxHealth);
                break;
        }
    }

    private IEnumerator HealthRecovery()
    {
        isHealthRecoveryActive = true;

        while (isHealthRecoveryActive)
        {
            _health = Mathf.Min(_health + healthRecoveryAmount, maxHealth);
            healthBar.SetHealth(_health);
            Debug.Log($"ü���� {healthRecoveryAmount}��ŭ ȸ���Ǿ����ϴ�. ���� ü�� : {_health}/{maxHealth}");
            yield return new WaitForSeconds(10f);
        }

    }

    public void UpdateXPUI()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXP;
        }

        if (levelText != null)
        {
            levelText.text = $"Level : {currentLevel} ( {currentXP} / {xpToNextLevel} )";
        }
    }

    public void AddPurchasedItem(ShopItem item)
    {
        if (!purchasedItems.Contains(item))
        {
            purchasedItems.Add(item);
        }
    }

    public void ResetPurchasedItem(ShopItem item)
    {
        if (!purchasedItems.Contains(item))
        {
            purchasedItems.Clear();
        }
    }

    public bool IsItemPurchased(ShopItem item)
    {
        return purchasedItems.Contains(item);
    }

    public void IncreaseAttackSpeed(float amount)
    {
        _playerAnimation.IncreaseAttackSpeed(amount);
    }

    public void HitAndRecovery(int amount)
    {
        _health = Mathf.Min(_health + amount, maxHealth);
        healthBar.SetHealth(_health);
    }
}
