using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class activationPlaceholder : MonoBehaviour
{
    public GameObject swainUlt;

    public PlayerInput pitufin;
    bool isButtonActive;
    // Start is called before the first frame update
    void Awake()
    {
        pitufin = new PlayerInput();
        pitufin.pitufin.RAbility.performed += OnCast;
        pitufin.pitufin.RAbility.canceled += OnCast;
    }

    // Update is called once per frame
    void Update()
    {
       if(isButtonActive)
       {
        swainUlt.SetActive(true);
        Debug.Log("pene");
       }
       else
       {
        swainUlt.SetActive(false);
       }
    }

    void OnCast(InputAction.CallbackContext context)
    {
        isButtonActive = context.ReadValueAsButton();
        Debug.Log($"Input detectado: {isButtonActive}, Fase:{context.phase}");
    }

    void OnEnable()
    {
        pitufin.pitufin.Enable();
    }

    void OnDisable()
    {
        pitufin.pitufin.Disable();
    }
}
