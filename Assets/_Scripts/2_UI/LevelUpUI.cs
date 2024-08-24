using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUpUI : MonoBehaviour
{
    /*
      TODO
    1. 페이드 인, 페이드 아웃, 스토리 진행되게.. (초반 인트로)
    페이드 인 ( 검은화면 -> 밝은화면 )
    페이드 아웃 ( 밝은화면 -> 검은화면 )
    . 1 - 검정화면에서 스토리 진행이 먼저 나온다.
    . 2 - 말이 다 끝났으면 페이드 인
    . 3 - npc대화하기.
    . 4 - 문으로 통하면 바로 게임스테이지로 들어가게 설정.

    2. 파이어볼 맞으면 데미지 들어가게 설정.
    3. Level (방) Generator 만들기. 아이작처럼
    */

    public Button option1;
    public Button option2;
    public Button option3;
    public GameObject cardPrefab;

    private PlayerAttributesManager playerAttributesManager;   
    private ScriptableCard card1, card2, card3;

    private Vector3 targetScale = new Vector3(1, 1, 1);
    private Vector3 endScale = new Vector3(0, 0, 0);

    

    // Start is called before the first frame update
    void Start()
    {
        playerAttributesManager = FindObjectOfType<PlayerAttributesManager>();        

        if (playerAttributesManager == null)
        {
            Debug.LogError("PlayerAttributesManager를 찾을 수 없습니다.");
        }

        // 이전에 등록된 모든 리스너 제거
        option1.onClick.RemoveAllListeners();
        option2.onClick.RemoveAllListeners();
        option3.onClick.RemoveAllListeners();

        option1.onClick.AddListener(() => SelectOption(1));
        option2.onClick.AddListener(() => SelectOption(2));
        option3.onClick.AddListener(() => SelectOption(3));

        transform.DOScale(targetScale, 1);
        transform.DOScale(endScale, 1);
        gameObject.SetActive(false);
    }

    public void ShowLevelUpOptions(ScriptableCard card1, ScriptableCard card2, ScriptableCard card3)
    {
        this.card1 = card1;
        this.card2 = card2;
        this.card3 = card3;

        //ClearCardUI();

        // 기존의 카드 UI를 업데이트
        UpdateCardUI(option1, card1);
        UpdateCardUI(option2, card2);
        UpdateCardUI(option3, card3);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        gameObject.SetActive(true);


        transform.DOScale(targetScale, .8f).OnComplete(() => {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }).SetEase(Ease.OutBounce);        
    }
    private void UpdateCardUI(Button button, ScriptableCard cardData)
    {
        CardUI cardUI = button.GetComponent<CardUI>();
        if (cardUI != null)
        {
            cardUI.SetCardData(cardData);
        }
    }

    private void ClearCardUI()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateCardUI(ScriptableCard cardData, Vector3 position, int optionIndex)
    {

        GameObject cardObject = Instantiate(cardPrefab, position, Quaternion.identity, transform);
        if (cardObject == null)
        {
            Debug.Log("cardObject NULL");
        }
        Card cardComponent = cardObject.GetComponent<Card>();
        if(cardComponent != null)
        {
            cardComponent.CardData = cardData;
        }
        Button button = GetComponent<Button>();
        if (button != null)
        {
            //button.onClick.RemoveAllListeners(); // 이전의 리스너 제거
            button.onClick.AddListener(() => SelectOption(optionIndex));
        }
        cardObject.transform.SetParent(transform, false);
    }

    public void SelectOption(int optionIndex)
    {
        ScriptableCard selectedCard = null;

        // 나중에 랜덤으로 optionIndex나오게 설정해야함.
        switch (optionIndex)
        {
            case 1:
                Debug.Log("1번째 옵션 선택");
                selectedCard = card1;
                //playerAttributesManager.IncreaseStat("strength", 5);
                Time.timeScale = 1;
                break;
            case 2:
                Debug.Log("2번째 옵션 선택");
                selectedCard = card2;
                //playerAttributesManager.IncreaseStat("health", 15);
                Time.timeScale = 1;
                break;
            case 3:
                Debug.Log("3번째 옵션 선택");
                selectedCard = card3;
                Time.timeScale = 1;
                break;
        }

        if(selectedCard != null)
        {
            playerAttributesManager.ApplyCardEffect(selectedCard);
        }


        transform.DOScale(endScale, 0.3f).OnComplete(() => {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        );

        Cursor.visible = false;
    }
}
