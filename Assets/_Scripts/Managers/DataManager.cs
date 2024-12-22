using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    /// <summary>
    /// "죽은 횟수"만 관리
    /// </summary>
    public int deathCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 유지
        }
        else
        {
            Destroy(gameObject);
        }

        LoadDeathCount();
    }

    /// <summary>
    /// deathCount만 세이브
    /// </summary>
    public void SaveDeathCount()
    {
        PlayerPrefs.SetInt("DeathCount", deathCount);
        PlayerPrefs.Save();
        Debug.Log($"DataManager: 죽은 횟수({deathCount})를 저장했습니다.");
    }

    /// <summary>
    /// deathCount만 로드
    /// </summary>
    public void LoadDeathCount()
    {
        if (!PlayerPrefs.HasKey("DeathCount"))
        {
            Debug.Log("DataManager: 저장된 DeathCount가 없습니다. 0으로 초기화.");
            PlayerPrefs.SetInt("DeathCount", 0);
        }
        else
        {
            deathCount = PlayerPrefs.GetInt("DeathCount");
            Debug.Log($"DataManager: DeathCount={deathCount} 로드 완료.");
        }
    }

    /// <summary>
    /// DeathCount가 1 이상이면 이어하기 버튼 활성화 판단에 사용
    /// </summary>
    public bool CanContinueGame()
    {
        return deathCount > 0;
    }
}
