using System.Collections;
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
    public TMP_Text inmuneTimer;
    public TMP_Text inmuneText;
    public PlayerInput pitufin;

    // Q
    public Image qImagen;
    public TMP_Text contadorQ;
    public TMP_Text duracionQ;

    // R
    public Image RImage;
    public TMP_Text contadorR;
    public TMP_Text duracionR;

    // E
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

    // 🛡️ Invencibilidad
    public static bool isInvincible = false;

    void Awake()
    {
        pitufin = new PlayerInput();

        pitufin.pitufin.RAbility.performed += OnRCast;
        pitufin.pitufin.RAbility.canceled += OnRCast;
        pitufin.pitufin.QAbility.performed += OnQCast;
        pitufin.pitufin.EAbility.performed += OnECast;
    }

    void Start()
    {
        if (finalBattleTempo == null)
        {
            var obj = GameObject.Find("FinalBattleText");
            finalBattleTempo = obj != null ? obj.GetComponent<TMP_Text>() : null;
            if (finalBattleTempo == null) Debug.LogWarning("No se encontró 'finalBattleTempo'.");
        }

        if (bossFSM == null)
        {
            bossFSM = FindObjectOfType<BossFSM>();
            if (bossFSM == null) Debug.LogWarning("No se encontró 'bossFSM'.");
        }

        StartCoroutine(temporizadorFinal());
    }

    void Update()
    {
        // Invencibilidad toggle con tecla I
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            isInvincible = !isInvincible;
            Debug.Log("Invencibilidad " + (isInvincible ? "ACTIVADA" : "DESACTIVADA"));
        }
    }

    void RActivation()
    {
        if (!RActive) return;

        RActive = false;
        swainUlt.SetActive(true);
        RImage.color = new Color(1, 1, 1, 0.5f);

        StartCoroutine(RIniciarDuracion());
    }

    IEnumerator temporizadorFinal()
    {
        float tiempoRestante = bossFSM != null ? bossFSM.wipeTemporizer : 10f;

        while (tiempoRestante > 0)
        {
            if (finalBattleTempo != null)
            {
                finalBattleTempo.text = Mathf.CeilToInt(tiempoRestante).ToString();
            }
            tiempoRestante--;
            yield return new WaitForSeconds(1f);
        }

        if (finalBattleTempo != null)
        {
            finalBattleTempo.text = "gg papuh :v";
        }
        else
        {
            Debug.Log("Fin de batalla: gg papuh :v");
        }
    }

    public IEnumerator InmuneTime()
    {
        float tiempoRestante = cloudDmg.duration;

        inmuneText.text = "Immunity: ";
        inmuneTimer.text = tiempoRestante.ToString();

        while (tiempoRestante > 0)
        {
            inmuneTimer.text = Mathf.CeilToInt(tiempoRestante).ToString();
            tiempoRestante--;

            yield return new WaitForSeconds(1f);
        }

        inmuneText.text = "";
        inmuneTimer.text = "";
    }

    IEnumerator RIniciarDuracion()
    {
        float tiempoRestante = RDuration;

        while (tiempoRestante > 0)
        {
            duracionR.text = Mathf.CeilToInt(tiempoRestante).ToString();
            tiempoRestante--;
            yield return new WaitForSeconds(1f);
        }

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

    void QActivation()
    {
        if (!qDisponible) return;

        qDisponible = false;
        rayoPrefab.SetActive(true);
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
        float tiempoRestante = qDuracion;

        while (tiempoRestante > 0)
        {
            duracionQ.text = tiempoRestante.ToString("F2");
            yield return null;
            tiempoRestante -= Time.deltaTime;
        }

        qImagen.color = new Color(1, 1, 1, 0.5f);
        qDisponible = false;
        duracionQ.text = "";
        rayoPrefab.SetActive(false);
        StartCoroutine(QIniciarCooldown());
    }

    void OnQCast(InputAction.CallbackContext context)
    {
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

    void EActivate()
    {
        if (!EActive) return;

        EActive = false;
        EImage.color = new Color(1, 1, 1, 0.5f);
        StartCoroutine(EIniciarHabilidad());
    }

    IEnumerator EIniciarHabilidad()
    {
        float tiempoRestante = EDuration;

        while (tiempoRestante > 0)
        {
            Quaternion adjustedRotation = eSlashOrigin.transform.rotation * quaternion.Euler(-90, 0, 0);
            GameObject newSlash = Instantiate(eSlash, eSlashOrigin.transform.position, adjustedRotation);
            Rigidbody rb = newSlash.GetComponent<Rigidbody>();
            rb.AddForce(eSlashOrigin.transform.forward * slashVelocity, ForceMode.Impulse);

            Destroy(newSlash, 0.4f);
            yield return new WaitForSeconds(0.5f);
            tiempoRestante -= 0.5f;
        }

        EImage.color = new Color(1, 1, 1, 0.5f);
        duracionE.text = "";
        StartCoroutine(EIniciarCooldown());
    }

    IEnumerator EIniciarCooldown()
    {
        float tiempoRestante = ECooldown;

        while (tiempoRestante > 0)
        {
            contadorE.text = Mathf.CeilToInt(tiempoRestante).ToString();
            yield return new WaitForSeconds(1f);
            tiempoRestante--;
        }

        EActive = true;
        EImage.color = Color.white;
        contadorE.text = "";
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

    void OnEnable()
    {
        pitufin.pitufin.Enable();
    }

    void OnDisable()
    {
        pitufin.pitufin.Disable();
    }
}
