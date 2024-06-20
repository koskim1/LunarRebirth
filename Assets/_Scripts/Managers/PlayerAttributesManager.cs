using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerAttributesManager : AttributesManager
{
    public HealthBar healthBar;
    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public LevelUpUI levelUpUI;

    private PlayerAnimation _playerAnimation;
    private CardGenerator cardGenerator;
    private CardCollection cardCollection;

    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    // Start is called before the first frame update
    protected override void Start()
    {                
        base.Start();
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
        UpdateXPUI();

        _playerAnimation = GetComponent<PlayerAnimation>();
        cardCollection = FindAnyObjectByType<CardCollection>();
        if (cardCollection == null)
        {
            Debug.Log("CardCollection이 설정되지 않았습니다.");
        }
        cardGenerator = new CardGenerator(cardCollection);

    }

    // Damage부분
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        healthBar.SetHealth(_health);
    }

    protected override void Die()
    {
        _playerAnimation.Dead();
    }

    // XP부분
    protected override void GainXP(int xp)
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

    public void IncreaseStat(string statName, int amount)
    {
        Debug.Log($"IncreaseStat called with statName: {statName}, amount: {amount}");

        switch (statName)
        {
            case "strength":
                _attack += amount;
                break;
            case "health":
                _health += amount;
                maxHealth = _health;
                healthBar.SetMaxHealth(maxHealth);
                healthBar.SetHealth(_health);
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
}
