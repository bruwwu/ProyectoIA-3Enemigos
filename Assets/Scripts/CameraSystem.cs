using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;

    public bool Zona1;
    public bool Zona2;
    public bool Zona3;
    public bool Zona4;
    public bool Zona5;

    void Start()
    {
        currentView = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Zona1)
        {
            currentView = views[0];
        }
        else if(Zona2)
        {
            currentView = views[1];
        }
        else if(Zona3)
        {
            currentView = views[2];
        }
        else if(Zona4)
        {
            currentView = views[3];
        }
        else if(Zona5)
        {
            currentView = views[4];
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Zona 1":
                Zona1 = true;
                break;
            case "Zona 2":
                Zona1 = false;
                Zona2 = true;
                break;
            case "Zona 3":
                Zona2 = false;
                Zona3 = true;
                break;
            case "Zona 4":
                Zona3 = false;
                Zona4 = true;
                break;
            case "Zona 5":
                Zona4 = false;
                Zona5 = true;
                break;
        }
    }
}
