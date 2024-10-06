using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    [Header("Velocidad")]
    public float MoveSpeed;
    private CharacterController characterController;
    public float rotationSensitivity = 210f;
    private Vector3 rotationInput = Vector3.zero; 

    public CameraSystem cameraSystem;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
        
        characterController.Move(move * Time.deltaTime * MoveSpeed);

        rotationInput.x = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
        rotationInput.y = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * rotationInput.x);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            PlayerTakeDmg(20);
            Debug.Log(GameManager.gameManager.playerHealth.Health);
        }

        switch (other.gameObject.tag)
        {
            case "Zona 1":
                cameraSystem.Zona1 = true;
                break;
            case "Zona 2":
                cameraSystem.Zona1 = false;
                cameraSystem.Zona2 = true;
                break;
            case "Zona 3":
                cameraSystem.Zona2 = false;
                cameraSystem.Zona3 = true;
                break;
            case "Zona 4":
                cameraSystem.Zona3 = false;
                cameraSystem.Zona4 = true;
                break;
            case "Zona 5":
                cameraSystem.Zona4 = false;
                cameraSystem.Zona5 = true;
                break;
        }
    }

    private void PlayerTakeDmg(int dmg)
    {
        GameManager.gameManager.playerHealth.DmgUnit(dmg, GameManager.gameManager.UI, GameManager.gameManager.Scene);
        Debug.Log("Awaaaa soy el personaje principal y me hicieron daño unu");
    }
}