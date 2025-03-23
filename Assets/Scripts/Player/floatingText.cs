using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingText : MonoBehaviour
{
    public float DestroyTime = 3f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        Destroy(gameObject, DestroyTime);   
    }

    void LateUpdate()
    {
        // Haz que el objeto mire hacia la cámara
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }
}
