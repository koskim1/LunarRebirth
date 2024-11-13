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

    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int deathCount = 0;    
    public int currentMLP = 0;
    public int previousMLP = 0;
    private int displayedMLP = 0;

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
        currentLevel = 0;
        currentXP = 0;
        xpToNextLevel = 10;
        deathCount = 0;
        currentMLP = 0;
        previousMLP = 0;
        displayedMLP = 0;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(_health);
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
    }

    public void AddMLP(int amount)
    {        
        previousMLP = currentMLP;
        currentMLP += amount;
        UpdateMLPUI(previousMLP, currentMLP);
        Debug.Log($"현재 MLP는 {currentMLP} 입니다");
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
        base.TakeDamage(damage);
        healthBar.SetHealth(_health);
    }

    protected override void Die()
    {
        StartCoroutine(animatorOnOff());
        UIManager.Instance.BossUI.SetActive(false);
        _playerAnimation.Dead();
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
        // 레벨업 로직
        Debug.Log("레벨 업!");
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
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
    }

    public void ApplyCardEffect(ScriptableCard card)
    {
        // 여기서 SO의 이름을 기반으로 IncreaseStat에 이름 전달해주고 이를 기반으로
        // 스탯을 올려주는 시스템으로 개발 중.. 스탯들 더 추가해야함!!
        switch (card.CardName)
        {
            case "Health":
                IncreaseStat("health", 15);
                break;
            case "Fireball":
                _fireballController.EnableFireball();
                break;
            case "SlowDash":
                _playerController.CanSlowDash();
                break;

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
            case "intelligence":
                // 예시로 지능 추가
                break;
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

    public bool IsItemPurchased(ShopItem item)
    {
        return purchasedItems.Contains(item);
    }
}
