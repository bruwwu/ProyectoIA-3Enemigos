using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectNavMesh : MonoBehaviour
{
    public Transform startPoint;  
    public Transform endPoint;    
    public float lerpSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // Lerp entre startPoint y endPoint basado en el tiempo
        float lerpValue = Mathf.PingPong(Time.time * lerpSpeed, 1); // Alterna entre 0 y 1 basado en el tiempo
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerpValue);
    }
}