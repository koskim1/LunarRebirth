using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCinemachineSingleton : MonoBehaviour
{
    public static EnemyCinemachineSingleton Instance;

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
}
