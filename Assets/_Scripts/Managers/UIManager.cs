using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject BossUI;
    public GameObject GoingToBossRoomText;
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
        if(GoingToBossRoomText != null)
        {
            GoingToBossRoomText.SetActive(false);
        }
    }
}
