using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAttack : MonoBehaviour
{
    Animator animator;
    PlayerInput playerInputA;

    [Header("Cooldown Config")]
    public float cooldownTime = 2f;  // Tiempo de espera entre ataques
    private float nextFireTime = 0f;

    [Header("Combo Config")]
    public static int noClicks = 0; // Contador de clicks
    private float lastClickedTime = 0f;  // Tiempo del último clic
    public float maxComboDelay = 1f; // Tiempo máximo entre clicks para combo

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerInputA = new PlayerInput();

        // Configuración del evento de Input
        playerInputA.CharacterControls.Hit.performed += OnHit;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Verificar y reiniciar combo si es necesario
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noClicks = 0;
        }

        // Sincronizar transiciones de animaciones
        if (stateInfo.normalizedTime > 1.0f)
        {
            if (stateInfo.IsName("hit1"))
            {
                animator.SetBool("hit1", false);
                Debug.Log("hit1 finalizado. Bool desactivado.");
            }
            else if (stateInfo.IsName("hit2"))
            {
                animator.SetBool("hit2", false);
                Debug.Log("hit2 finalizado. Bool desactivado.");
            }
            else if (stateInfo.IsName("hit3"))
            {
                animator.SetBool("hit3", false);
                noClicks = 0;
                Debug.Log("hit3 finalizado. Combo reiniciado.");
            }
        }
    }

    void OnHit(InputAction.CallbackContext context)
    {
        if (Time.time < nextFireTime) return; // No permitir ataque si el cooldown no ha terminado

        lastClickedTime = Time.time;
        noClicks++;
        nextFireTime = Time.time + cooldownTime;

        

        // Ejecutar el combo
        HandleCombo();
    }

    void HandleCombo()
    {
        if (noClicks == 1)
        {
            animator.SetBool("hit1", true);
            Debug.Log("Iniciando hit1.");
        }
        else if (noClicks == 2 && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
        {
            //animator.SetBool("hit1", false); //Aprendamos de nuestros errores, estabamos haciendo aqui lo que ya hacemos arriba, se follan entre si 👍
            animator.SetBool("hit2", true);
            Debug.Log("Transición a hit2.");
        }
        else if (noClicks == 3 && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
        {
            //animator.SetBool("hit2", false);
            animator.SetBool("hit3", true);
            Debug.Log("Transición a hit3.");
            /*StartCoroutine(damBOLAS());
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", false); 
            animator.SetBool("hit3", false);*/ 
        }
        else
        {
            
            noClicks = 0; // Reinicia el combo si no se cumplen las condiciones
            Debug.Log("Combo reseteado por secuencia inválida.");
        }
    }

    IEnumerator damBOLAS()
    {
        yield return new WaitForSeconds(2);
    }

    void OnEnable()
    {
        playerInputA.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInputA.CharacterControls.Disable();
    }
}
