using System.Collections;
using DialogueEditor;
using UnityEngine;

public class SingleDialogue : MonoBehaviour
{
    [SerializeField] private NPCConversation SingleConvo;
    [SerializeField] private PlayerSystem playerSystem;  // Referencia al nuevo sistema de movimiento
    public Camera playerCamera;
    private Transform originalParent;
    private Vector3 originalPosition;

    private void Start()
    {
        GameObject playerDAM = GameObject.Find("PlayerDAM");
        playerSystem = playerDAM.GetComponent<PlayerSystem>(); // Obtener referencia al PlayerSystem
        originalParent = playerCamera.transform.parent;
        originalPosition = playerCamera.transform.localPosition;
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

        // Desuscríbete del evento para evitar múltiples suscripciones
        ConversationManager.OnConversationEnded -= HandleConversationEnded;
    }

    private void DisablePlayerMovement()
    {
        // Deshabilitar el control de CharacterController durante el diálogo
        playerSystem.enabled = false;
    }

    private void EnablePlayerMovement()
    {
        // Habilitar el control de CharacterController al finalizar el diálogo
        playerSystem.enabled = true;
    }
}
