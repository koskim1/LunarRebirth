using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private SceneManagers sceneManagers;
    
    // Start is called before the first frame update
    void Start()
    {
        sceneManagers = FindAnyObjectByType<SceneManagers>();
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sceneManagers.LoadDungeon();
        }
    }
}
