using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUpUI : MonoBehaviour
{
    public Button option1;
    public Button option2;
    public Button option3;

    private PlayerAttributesManager playerAttributesManager;

    // Start is called before the first frame update
    void Start()
    {
        playerAttributesManager = FindObjectOfType<PlayerAttributesManager>();

        if (playerAttributesManager == null)
        {
            Debug.LogError("PlayerAttributesManager를 찾을 수 없습니다.");
        }

        option1.onClick.AddListener(() => SelectOption(1));
        option2.onClick.AddListener(() => SelectOption(2));
        option3.onClick.AddListener(() => SelectOption(3));

        gameObject.SetActive(false);
    }

    public void ShowLevelUpOptions()
    {
        gameObject.SetActive(true);
    }

    public void SelectOption(int optionIndex)
    {
        // 나중에 랜덤으로 optionIndex나오게 설정해야함.
        switch (optionIndex)
        {
            case 1:
                playerAttributesManager.IncreaseStat("strength", 5);
                break;
            case 2:
                playerAttributesManager.IncreaseStat("agility", 5);
                break;
            case 3:
                Ability newAbility = new Ability("Fireball", "Throws a fireball", new Effect());
                playerAttributesManager.AddAbility(newAbility);
                break;
        }
        gameObject.SetActive(false);
    }
}
