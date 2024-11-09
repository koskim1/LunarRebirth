using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI bossName;

    private Boss boss;
    // Start is called before the first frame update
    void Awake()
    {
        boss = FindAnyObjectByType<Boss>();
        bossName.text = "���� - ��";
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
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
