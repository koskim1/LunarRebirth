using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainStoryIntro : MonoBehaviour
{
    private void OnEnable()
    {
        LoadingSceneController.LoadScene("MainRoom");
    }

}
