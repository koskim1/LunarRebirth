using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    /// <summary>
    /// "���� Ƚ��"�� ����
    /// </summary>
    public int deathCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ���� ����
        }
        else
        {
            Destroy(gameObject);
        }

        LoadDeathCount();
    }

    /// <summary>
    /// deathCount�� ���̺�
    /// </summary>
    public void SaveDeathCount()
    {
        PlayerPrefs.SetInt("DeathCount", deathCount);
        PlayerPrefs.Save();
        Debug.Log($"DataManager: ���� Ƚ��({deathCount})�� �����߽��ϴ�.");
    }

    /// <summary>
    /// deathCount�� �ε�
    /// </summary>
    public void LoadDeathCount()
    {
        if (!PlayerPrefs.HasKey("DeathCount"))
        {
            Debug.Log("DataManager: ����� DeathCount�� �����ϴ�. 0���� �ʱ�ȭ.");
            PlayerPrefs.SetInt("DeathCount", 0);
        }
        else
        {
            deathCount = PlayerPrefs.GetInt("DeathCount");
            Debug.Log($"DataManager: DeathCount={deathCount} �ε� �Ϸ�.");
        }
    }

    /// <summary>
    /// DeathCount�� 1 �̻��̸� �̾��ϱ� ��ư Ȱ��ȭ �Ǵܿ� ���
    /// </summary>
    public bool CanContinueGame()
    {
        return deathCount > 0;
    }
}
