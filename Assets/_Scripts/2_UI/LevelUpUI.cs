using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUpUI : MonoBehaviour
{
    /*
      TODO
    1. 레어카드 추가해서 확률적으로 나오게 만들기
    2. 레어카드에 Fireball 만든거 추가 ( 능력 추가하기 )
    3. Level (방) Generator 만들기. 아이작처럼
    */

    public Button option1;
    public Button option2;
    public Button option3;

    private PlayerAttributesManager playerAttributesManager;
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

    public void ShowLevelUpOptions()
    {
        gameObject.SetActive(true);
        transform.DOScale(targetScale, .8f).OnComplete(()=> Time.timeScale = 0).SetEase(Ease.OutBounce);
        
    }

    public void SelectOption(int optionIndex)
    {

        // 나중에 랜덤으로 optionIndex나오게 설정해야함.
        switch (optionIndex)
        {
            case 1:
                Debug.Log("1번째 옵션 선택");
                playerAttributesManager.IncreaseStat("strength", 5);
                Time.timeScale = 1;
                break;
            case 2:
                Debug.Log("2번째 옵션 선택");
                playerAttributesManager.IncreaseStat("health", 15);
                Time.timeScale = 1;
                break;
            case 3:
                Debug.Log("3번째 옵션 선택");
                Time.timeScale = 1;
                break;
        }
        transform.DOScale(endScale, 0.3f).OnComplete(() => {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        );
    }
}
