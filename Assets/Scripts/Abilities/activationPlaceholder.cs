using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class activationPlaceholder : MonoBehaviour
{
    [Header("EnemyTemporizer")]
    public BossFSM bossFSM;
    public CloudDMG cloudDmg;
    public TMP_Text finalBattleTempo;
    [Header("Componentes/UI")]
    //Inmune Timpe
    public TMP_Text inmuneTimer;
    public TMP_Text inmuneText;
    //PlayerInput
    public PlayerInput pitufin; 
    //Q
    public Image qImagen;
    public TMP_Text contadorQ;
    public TMP_Text duracionQ;
    //R
    public Image RImage;
    public TMP_Text contadorR;
    public TMP_Text duracionR;
    //E
    public Image EImage;
    public TMP_Text contadorE;
    public TMP_Text duracionE;

    [Header("SwainE")]
    public GameObject eSlash;
    public float ECooldown = 8f;
    [SerializeField] private bool EActive = true;
    public float EDuration = 2f;
    public GameObject eSlashOrigin;
    public float slashVelocity = 5f;


    [Header("Ultimate")]
    public GameObject swainUlt;
    public float RCooldown = 15f;
    [SerializeField] private bool RActive = true;
    public float RDuration = 5f;

    [Header("SwainQ")]
    public GameObject rayoPrefab;
    public float qCooldown = 5f;
    public float qDuracion = .5f;
    [SerializeField] private bool qDisponible = true;

    void Awake()
    {
        pitufin = new PlayerInput();

        // Configuración de Input para la Ultimate
        pitufin.pitufin.RAbility.performed += OnRCast;
        pitufin.pitufin.RAbility.canceled += OnRCast;
        // Configuración de Input para la Q
        pitufin.pitufin.QAbility.performed += OnQCast;
        //Configuracion Input E
        pitufin.pitufin.EAbility.performed += OnECast;
    }

    void Start()
    {
        StartCoroutine(temporizadorFinal());
    }

    void Update()
    {
    }
    //BLOQUE DE HABILIDAD R
    void RActivation()
    {
        if (!RActive) return;
        RActive = false;
        swainUlt.SetActive(true);
        RActive = true; // Activa la habilidad
        RImage.color = new Color(1, 1, 1, 0.5f);

        StartCoroutine(RIniciarDuracion());
    }

    //Temporizador de la batalla final, gg papu :v
    IEnumerator temporizadorFinal()
    {
        float tiempoRestante = bossFSM.wipeTemporizer;

        while (tiempoRestante > 0)
        {
            finalBattleTempo.text = Mathf.CeilToInt(tiempoRestante).ToString();
            tiempoRestante--;
            yield return new WaitForSeconds(1f);
        }
        finalBattleTempo.text = "gg papuh :v";
    }
    //webos
    public IEnumerator InmuneTime()
{
    float tiempoRestante = cloudDmg.duration;

    // Mostrar el texto inicial de inmunidad
    inmuneText.text = "Immunity: ";
    inmuneTimer.text = tiempoRestante.ToString();

    while (tiempoRestante > 0)
    {
        // Actualizar el texto con el tiempo restante
        inmuneTimer.text = Mathf.CeilToInt(tiempoRestante).ToString();
        tiempoRestante--;

        yield return new WaitForSeconds(1f);
    }

    // Limpiar el texto una vez que termine la inmunidad
    inmuneText.text = "";
    inmuneTimer.text = "";
}



    IEnumerator RIniciarDuracion()
    {
        float timepoRestante = RDuration;

        while (timepoRestante > 0)
        {
            duracionR.text = Mathf.CeilToInt(timepoRestante).ToString();
            timepoRestante--;
            yield return new WaitForSeconds(1f);
        }
        RActive = false;
        duracionR.text = "";
        swainUlt.SetActive(false);
        StartCoroutine(RIniciarCooldown());
    }

    IEnumerator RIniciarCooldown()
    {
        float tiempoRestante = RCooldown;

        while (tiempoRestante > 0)
        {
            contadorR.text = Mathf.CeilToInt(tiempoRestante).ToString();
            yield return new WaitForSeconds(1f);
            tiempoRestante--;
        }
        RActive = true;
        RImage.color = Color.white;
        contadorR.text = "";
    }
      void OnRCast(InputAction.CallbackContext context)
    {
        // Actualizar el estado de activación de la ultimate
        if (RActive && context.ReadValueAsButton())
        {
            RActivation();
        }
        else
        {
            Debug.Log("No sirvo we");
        }
        Debug.Log($"Ultimate activada: {RActive}, Fase:{context.phase}");
    }


    //BLOQUE DE HABILIDAD Q
    void QActivation()
    {
        if (!qDisponible) return; // Evitar activar Q si no está disponible

        qDisponible = false;

        // Instanciar rayos en los puntos designados
        rayoPrefab.SetActive(true);

        // Actualizar la interfaz visual del cooldown
        
        StartCoroutine(QIniciarDuracion());
    }

    IEnumerator QIniciarCooldown()
    {
        float tiempoRestante = qCooldown;
        rayoPrefab.SetActive(false);

        while (tiempoRestante > 0)
        {
            contadorQ.text = Mathf.CeilToInt(tiempoRestante).ToString();
            yield return new WaitForSeconds(1f);
            tiempoRestante--;
        }
        qDisponible = true;
        qImagen.color = Color.white;
        contadorQ.text = "";
    }
     IEnumerator QIniciarDuracion()
    {
        float timepoRestante = qDuracion;

        while (timepoRestante > 0)
        {
            duracionQ.text = timepoRestante.ToString("F2");
            yield return null;
            timepoRestante -= Time.deltaTime;
        }
        qImagen.color = new Color(1, 1, 1, 0.5f);
        qDisponible = false;
        duracionQ.text = "";
        rayoPrefab.SetActive(false);
        StartCoroutine(QIniciarCooldown());
    }

    void OnQCast(InputAction.CallbackContext context)
    {
        // Activar Q solo si está disponible
        if (qDisponible && context.ReadValueAsButton())
        {
            QActivation();
            Debug.Log("Habilidad Q activada.");
        }
        else
        {
            Debug.Log("Habilidad Q no disponible.");
        }
    }

    //BLOQUE HABILIDADES E

    void EActivate()
    {
        if (!EActive) return; // Salir si no está disponible
        EActive = false;      // Deshabilitar la habilidad durante su uso

        EImage.color = new Color(1, 1, 1, 0.5f); // Indicar visualmente que está en cooldown

        StartCoroutine(EIniciarHabilidad());
    }

    IEnumerator EIniciarHabilidad()
    {
        float tiempoRestante = EDuration;

        while (tiempoRestante > 0)
        {
            Quaternion adjustedRotation = eSlashOrigin.transform.rotation * quaternion.Euler(-90, 0, 0);
            // Instanciar el slash desde el origen
            GameObject newSlash = Instantiate(eSlash, eSlashOrigin.transform.position, adjustedRotation);
            Rigidbody rb = newSlash.GetComponent<Rigidbody>();
            
            // Aplicar fuerza al slash
            rb.AddForce(eSlashOrigin.transform.forward * slashVelocity, ForceMode.Impulse);

            Destroy(newSlash, 0.4f); // Destruir después de 0.4 segundos

            yield return new WaitForSeconds(0.5f); // Intervalo entre slashes
            tiempoRestante -= 0.5f;
        }

        // Finalizar la habilidad
        EImage.color = new Color(1, 1, 1, 0.5f);
        duracionE.text = ""; // Limpiar texto de duración
        StartCoroutine(EIniciarCooldown());
    }

    IEnumerator EIniciarCooldown()
    {
        float tiempoRestante = ECooldown;

        while (tiempoRestante > 0)
        {
            contadorE.text = Mathf.CeilToInt(tiempoRestante).ToString(); // Mostrar cooldown
            yield return new WaitForSeconds(1f);
            tiempoRestante--;
        }

        // Reiniciar la habilidad
        EActive = true;
        EImage.color = Color.white;
        contadorE.text = ""; // Limpiar el contador
    }

    void OnECast(InputAction.CallbackContext context)
    {
        if (EActive && context.ReadValueAsButton())
        {
            EActivate();
        }
        else
        {
            Debug.Log("La habilidad E no está disponible.");
        }
        Debug.Log($"Habilidad E activada: {EActive}, Fase: {context.phase}");
    }

    //Mamadams para activar el pitufin (Input Action)

    void OnEnable()
    {
        pitufin.pitufin.Enable();
    }

    void OnDisable()
    {
        pitufin.pitufin.Disable();
    }
}