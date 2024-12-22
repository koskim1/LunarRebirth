using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainStoryIntro : MonoBehaviour
{
    private void OnEnable()
    {
        if (PlayerAttributesManager.Instance != null)
        {
            PlayerAttributesManager.Instance.gameObject.SetActive(true);
            PlayerAttributesManager.Instance.gameObject.transform.position = new Vector3(0, 0f, -10f);
            //여기서 그냥 플레이어 체력이랑 레벨
            /*
                public int currentLevel = 1;
                public int currentXP = 0;
                public int xpToNextLevel = 100;
                public int deathCount = 0; 이놈들 초기화해버리자
            */
            PlayerAttributesManager.Instance.currentLevel = 1;
            PlayerAttributesManager.Instance.currentXP = 0;
            PlayerAttributesManager.Instance.xpToNextLevel = 10;
            //PlayerAttributesManager.Instance.deathCount = 0;
            DataManager.Instance.deathCount = 0;
            if (CameraFollow.Instance != null && UIManager.Instance != null)
            {
                CameraFollow.Instance.gameObject.SetActive(true);
                UIManager.Instance.gameObject.SetActive(true);
            }

        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.soundFXObject.gameObject.SetActive(true);
            AudioManager.Instance.BGM.gameObject.SetActive(true);
        }

        LoadingSceneController.LoadScene("MainRoom");

        //SceneManager.LoadScene("MainRoom");
    }

}
