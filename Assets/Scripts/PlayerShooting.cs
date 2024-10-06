using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject BalaPrefabInicio;
    public GameObject Mira;
    public float BalaVelocidad;

    private bool block;

    void Start()
    {
        block = false;
    }

    void Update()
    {
        if(!block)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject Balatemporal = Instantiate(Mira, BalaPrefabInicio.transform.position, BalaPrefabInicio.transform.rotation) as GameObject;
                Rigidbody rb = Balatemporal.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * BalaVelocidad);

                Destroy(Balatemporal, 1f);
                StartCoroutine(WaitFor());
            } 
        }
        
    }

    IEnumerator WaitFor()
    {
        block = true;
        yield return new WaitForSeconds(0.8f);
        block = false;
    }

}