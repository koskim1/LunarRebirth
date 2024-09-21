using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    public Camera minimapCam;

    private void Start()
    {
        RenderTexture renderTexture = new RenderTexture(256, 256, 8);

        minimapCam.targetTexture = renderTexture;
        minimapCam.Render();
        minimapCam.targetTexture = null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.y = 0;
        eulerAngles.z = 0;
        transform.eulerAngles = eulerAngles;
    }
}
