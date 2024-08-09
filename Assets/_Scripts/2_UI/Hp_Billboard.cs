using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp_Billboard : MonoBehaviour
{
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
        
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
