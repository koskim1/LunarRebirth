using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject BossUI;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else if (Instance != null)
        {
            Destroy(gameObject);
        }

        if(BossUI != null)
        {
            BossUI.SetActive(false);
        }
    }

    public void BossCinematicEnd()
    {
        BossUI.SetActive(true);
    }
}
