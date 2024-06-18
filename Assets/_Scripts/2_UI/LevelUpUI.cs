using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUpUI : MonoBehaviour
{
    /*
      TODO
    1. ����ī�� �߰��ؼ� Ȯ�������� ������ �����
    2. ����ī�忡 Fireball ����� �߰� ( �ɷ� �߰��ϱ� )
    3. Level (��) Generator �����. ������ó��
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
            Debug.LogError("PlayerAttributesManager�� ã�� �� �����ϴ�.");
        }

        // ������ ��ϵ� ��� ������ ����
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

        // ���߿� �������� optionIndex������ �����ؾ���.
        switch (optionIndex)
        {
            case 1:
                Debug.Log("1��° �ɼ� ����");
                playerAttributesManager.IncreaseStat("strength", 5);
                Time.timeScale = 1;
                break;
            case 2:
                Debug.Log("2��° �ɼ� ����");
                playerAttributesManager.IncreaseStat("health", 15);
                Time.timeScale = 1;
                break;
            case 3:
                Debug.Log("3��° �ɼ� ����");
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
