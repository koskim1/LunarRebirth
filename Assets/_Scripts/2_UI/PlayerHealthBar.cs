using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI playerHpText;

    private PlayerAttributesManager playerAttributesManager;

    private void Awake()
    {
        playerAttributesManager = FindAnyObjectByType<PlayerAttributesManager>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        // �����̴��� �ּ� 0 �ִ� 100 �����س���
        slider.value = health;
        playerHpText.text = $"{playerAttributesManager._health} / {playerAttributesManager.maxHealth}";
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    
}
