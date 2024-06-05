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

    // Damage부분
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        healthBar.SetHealth(_health);
    }

    protected override void Die()
    {
        playerAnimation.Dead();
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
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        maxHealth += 10;
        _health = maxHealth;
        _attack += 5;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(_health);
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
