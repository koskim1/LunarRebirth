using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;

    private Camera mainCamera;
    private CinemachineBrain cinemachineBrain;

    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            mainCamera = Camera.main;
            cinemachineBrain = mainCamera.GetComponentInChildren<CinemachineBrain>();
        }
        else if(Instance != null)
        {
            Destroy(gameObject);    
        }

        if(target == null)
        {
            FindPlayer();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 targetPosition = target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("CameraFollow : Player not Found");
        }
    }
}
