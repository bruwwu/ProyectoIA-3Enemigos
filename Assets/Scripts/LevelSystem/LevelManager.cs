using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject puerta;  // Referencia a la puerta que se debe desactivar

    void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy") || other.CompareTag("juanitoTorreta") || other.CompareTag("bruwuFem"))
    {
        Debug.Log("Un objeto con tag válido (Enemy, juanitoTorreta o BruwuFem) entró en el área: " + other.tag);
    }
}


    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("juanitoTorreta") || other.CompareTag("bruwuFem"))
        {
            Debug.Log("Enemigo salió del área o fue destruido.");
            puerta.SetActive(false);  // Desactivar la puerta cuando el enemigo salga
        }
    }
}
