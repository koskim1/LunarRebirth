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
    public AudioClip[] playerTakeDamageSoundClip;

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
    //public int deathCount = 0;    
    public int currentMLP = 0;
    public int previousMLP = 0;
    private int displayedMLP = 0;
    private bool isHealthRecoveryActive = false;
    public bool hasLifeSteal = false;
    public int lifeStealAmount = 0;
    private int healthRecoveryAmount = 0;
    private List<ShopItem> purchasedItems = new List<ShopItem>();   // 구매한 아이템 목록
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
        currentLevel = 1;
        currentXP = 0;
        xpToNextLevel = 10;
        //deathCount = 0;
        currentMLP = 0;
        previousMLP = 0;
        displayedMLP = 0;
        healthRecoveryAmount = 0;
        isHealthRecoveryActive = false;
        hasLifeSteal = false;
        lifeStealAmount = 0;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(_health);
        _sword.transform.localScale = new Vector3(1, 1, 1);

        if(_fireballController != null)
        {
            _fireballController.canShootFireball = false;
        }
        if (_playerController != null)
        {
            _playerController._canSlowDash = false;
        }
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetInt("MaxHealth", maxHealth);
        PlayerPrefs.SetInt("CurrentHealth", _health);
        PlayerPrefs.SetInt("Attack", _attack);
        PlayerPrefs.SetFloat("Defense", defense);
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.SetInt("CurrentXP", currentXP);
        PlayerPrefs.SetInt("XpToNextLevel", xpToNextLevel);
        //PlayerPrefs.SetInt("PlayerDeathCount", deathCount);
        PlayerPrefs.SetInt("PreviousMLP", previousMLP);
        PlayerPrefs.SetInt("CurrentMLP", currentMLP);
        PlayerPrefs.SetInt("HealthRecoveryAmount", healthRecoveryAmount);        
        PlayerPrefs.SetInt("LifeStealAmount", lifeStealAmount);

        PlayerPrefs.SetInt("HasLifeSteal", hasLifeSteal ? 1 : 0);
        PlayerPrefs.SetInt("CanShootFireball", _fireballController.canShootFireball ? 1 : 0);
        PlayerPrefs.SetInt("CanSlowDash", _playerController._canSlowDash ? 1 : 0);

        PlayerPrefs.Save();

        Debug.Log("플레이어의 데이터가 저장되었습니다.");
    }

    public void LoadPlayerData()
    {
        // 만약 저장된 데이터가 없다면
        if (!PlayerPrefs.HasKey("MaxHealth"))
        {
            Debug.Log("저장된 플레이어 데이터가 없습니다.");
            return;
        }

        maxHealth = PlayerPrefs.GetInt("MaxHealth");
        _health = PlayerPrefs.GetInt("CurrentHealth");
        _attack = PlayerPrefs.GetInt("Attack");
        defense = PlayerPrefs.GetFloat("Defense");
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentXP = PlayerPrefs.GetInt("CurrentXP");
        //deathCount = PlayerPrefs.GetInt("PlayerDeathCount");
        xpToNextLevel = PlayerPrefs.GetInt("XpToNextLevel");
        previousMLP = PlayerPrefs.GetInt("PreviousMLP");
        currentMLP = PlayerPrefs.GetInt("CurrentMLP");
        healthRecoveryAmount = PlayerPrefs.GetInt("HealthRecoveryAmount");        
        lifeStealAmount = PlayerPrefs.GetInt("LifeStealAmount");

        hasLifeSteal = PlayerPrefs.GetInt("HasLifeSteal") == 1 ? true : false;
        _fireballController.canShootFireball = PlayerPrefs.GetInt("CanShootFireball") == 1 ? true : false;
        _playerController._canSlowDash = PlayerPrefs.GetInt("CanSlowDash") == 1 ? true : false;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(_health);
        
        UpdateXPUI();
        UpdateMLPUI(previousMLP,currentMLP);

        Debug.Log("플레이어 데이터를 불러왔습니다.");
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
            Debug.Log("CardCollection이 설정되지 않았습니다.");
        }
        cardGenerator = new CardGenerator(cardCollection);

        LoadPlayerData();
        PlayerPrefs.Save();
    }

    public void RestoreHealth()
    {
        _health = maxHealth;
        healthBar.SetHealth(_health);
        PlayerPrefs.SetInt("CurrentHealth", _health);
    }

    public void AddMLP(int amount)
    {        
        previousMLP = currentMLP;
        currentMLP += amount;
        UpdateMLPUI(previousMLP, currentMLP);
        Debug.Log($"현재 MLP는 {currentMLP} 입니다");
        SavePlayerData();
    }

    public void UpdateMLPUI(int fromValue, int toValue)
    {        
        // 혹여나 너무 빨리잡아서 꼬이면 안되니 예외처리  
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

    // Damage부분
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(Mathf.RoundToInt(damage * defense));
        animator.SetTrigger("getHit");
        AudioManager.Instance.PlayRandomSoundFXClip(playerTakeDamageSoundClip, transform, 0.5f);
        healthBar.SetHealth(_health);
    }

    protected override void Die()
    {
        StartCoroutine(animatorOnOff());
        UIManager.Instance.BossUI.SetActive(false);
        _playerAnimation.Dead();
        GameManager.Instance.TogglePlayerMovement(false);
        SavePlayerData();
    }

    // XP부분
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
        AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.levelUpSFX, transform, .5f);
        // 레벨업 로직
        Debug.Log("레벨 업!");
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

        // 카드 생성
        ScriptableCard card1 = cardGenerator.GenerateCard();
        ScriptableCard card2 = cardGenerator.GenerateCard();
        ScriptableCard card3 = cardGenerator.GenerateCard();

        // LevelUpUI.cs에 카드 전달
        //levelUpUI.ShowLevelUpOptions(card1, card2, card3);

        // 레벨업 UI 표시
        if (levelUpUI != null)
        {
            Debug.Log("레벨업UI");
            levelUpUI.ShowLevelUpOptions(card1, card2, card3);
        }

        SavePlayerData();
    }

    public void ApplyCardEffect(ScriptableCard card)
    {
        // 여기서 SO의 이름을 기반으로 IncreaseStat에 이름 전달해주고 이를 기반으로
        // 스탯을 올려주는 시스템으로 개발 중.. 스탯들 더 추가해야함!!
        switch (card.cardKey)
        {
            #region Common등급
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
                hasLifeSteal = true;
                break;
            #endregion

            #region Rare등급
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
                hasLifeSteal = true;
                break;
            #endregion
            
            #region Epic등급
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
                hasLifeSteal = true;
                break;
            case "HealthX2":
                maxHealth *= 2;
                _health = maxHealth;
                healthBar.SetMaxHealth(maxHealth);
                healthBar.SetHealth(maxHealth);
                break;
            case "StrengthX2":
                _attack *= 2;
                break;
            #endregion

            #region Legendary등급
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
                hasLifeSteal = true;
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
            Debug.Log($"체력이 {healthRecoveryAmount}만큼 회복되었습니다. 현재 체력 : {_health}/{maxHealth}");
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
