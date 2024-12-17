using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerController playerController;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;
    }

    public void TogglePlayerMovement(bool canMove)
    {
        if (playerController != null)
        {
            Debug.Log("플레이어 TogglePlayerMovement");
            playerController.SetCanMove(canMove);
        }
    }

  
    public void ChangeLocale(int index)
    {
        UIManager.Instance.PlayClickSound();
        if (LocalManager.Instance.isChanging)
        {
            return;
        }

        StartCoroutine(ChangeRoutine(index));
    }

    IEnumerator ChangeRoutine(int index)
    {
        LocalManager.Instance.isChanging = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        LocalManager.Instance.isChanging = false;
    }
}
