using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerAttributesManager : AttributesManager
{
    public static PlayerAttributesManager Instance;

    public HealthBar healthBar;
    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public LevelUpUI levelUpUI;

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

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);            
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerController = GetComponent<PlayerController>();
        _fireballController = GetComponent<FireballController>();
        animator = GetComponent<Animator>();
        cardCollection = FindAnyObjectByType<CardCollection>();
        sceneManagers = FindAnyObjectByType<SceneManagers>();
        uiManager = FindAnyObjectByType<UIManager>();
    }
    // Start is called before the first frame update
    protected override void Start()
    {                
        base.Start();        

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
        UpdateXPUI();

        if (cardCollection == null)
        {
            Debug.Log("CardCollection�� �������� �ʾҽ��ϴ�.");
        }
        cardGenerator = new CardGenerator(cardCollection);
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
        base.TakeDamage(damage);
        healthBar.SetHealth(_health);
    }

    protected override void Die()
    {
        StartCoroutine(animatorOnOff());
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
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
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
            case "strength":
                _attack += amount;
                break;
            case "health":
                maxHealth += amount;
                _health = maxHealth;
                healthBar.SetMaxHealth(maxHealth);
                healthBar.SetHealth(maxHealth);
                break;
            case "intelligence":
                // ���÷� ���� �߰�
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

    
}
