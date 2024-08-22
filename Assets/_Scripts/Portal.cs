using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public static Portal Instance;

    private SceneManagers sceneManagers;

    private void Awake()
    {
        sceneManagers = GameObject.Find("SceneManager").GetComponent<SceneManagers>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sceneManagers.LoadDungeon();
        }
    }
}
