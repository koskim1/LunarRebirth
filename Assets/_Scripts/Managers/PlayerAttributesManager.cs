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

    private PlayerAnimation playerAnimation;

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

        playerAnimation = GetComponent<PlayerAnimation>();
    }

    // Damage�κ�
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        healthBar.SetHealth(_health);
    }

    protected override void Die()
    {
        playerAnimation.Dead();
    }

    // XP�κ�
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
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        _health = maxHealth;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(_health);

        // ������ UI ǥ��
        if(levelUpUI != null)
        {
            Debug.Log("������UI");
            levelUpUI.ShowLevelUpOptions();
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
