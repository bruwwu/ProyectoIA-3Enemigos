using System.Collections;
using DialogueEditor;
using UnityEngine;

public class SingleDialogue : MonoBehaviour
{
    [SerializeField] private NPCConversation SingleConvo;
    [SerializeField] private PlayerSystem playerSystem;  // Referencia al nuevo sistema de movimiento
    [SerializeField] public GameObject puertaInicio;

    private Vector3 lastMoveDirection;

    private void Start()
    {
        GameObject playerDAM = GameObject.Find("PlayerDAM");
        playerSystem = playerDAM.GetComponent<PlayerSystem>(); // Obtener referencia al PlayerSystem
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Deshabilitar el movimiento del jugador
                DisablePlayerMovement();

                // Inicia la conversación
                ConversationManager.Instance.StartConversation(SingleConvo);

                // Suscríbete al evento de fin de conversación
                ConversationManager.OnConversationEnded += HandleConversationEnded;
            }
        }
    }

    private void HandleConversationEnded()
    {
        // Habilitar el movimiento del jugador
        EnablePlayerMovement();
        puertaInicio.SetActive(false);

        // Desuscríbete del evento para evitar múltiples suscripciones
        ConversationManager.OnConversationEnded -= HandleConversationEnded;
    }

    private void DisablePlayerMovement()
    {
        // Guardar la última dirección de movimiento antes de deshabilitar
        lastMoveDirection = playerSystem.transform.forward;

        // Deshabilitar el control de CharacterController durante el diálogo
        playerSystem.enabled = false;

        // Detener el CharacterController completamente
        CharacterController controller = playerSystem.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.Move(Vector3.zero); // Forzar a que no tenga velocidad
        }
    }

    private void EnablePlayerMovement()
    {
        // Habilitar el control de CharacterController al finalizar el diálogo
        playerSystem.enabled = true;

        // Restaurar la dirección de movimiento previa
        playerSystem.transform.forward = lastMoveDirection;
    }
}
