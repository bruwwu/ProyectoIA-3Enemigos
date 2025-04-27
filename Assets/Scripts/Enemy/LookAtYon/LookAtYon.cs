    using System.Collections;
    using UnityEngine;
    using Utilities;
    using DebugManager;
    using System.Collections.Generic;

    public class LookAtYon : MonoBehaviour
    {
        [Header("VisionCone")]
        public Transform juanitoTorreta;
        public Transform player;
        public float coneAngle;
        public float coneDistance;
        private bool isDetected = false;
        private bool isRotating = false;  // Para controlar la activación de la rotación

        [Header("Corrutina")]
        public float idleRotationSpeed = 30f;  // Velocidad de la rotación idle
        public float maxRotationAngle = 90f;   // Ángulo máximo de rotación
        private float currentRotationAngle = 0f;
        private int rotationDirection = 1; 
        [Header("Shooting")]
        public GameObject Mira;
        public GameObject BalaPrefabInicio;
        public float BalaVelocidad;

        private bool block;
        public static float difficultyWeight = 0.1f;
        // 1 = rotación positiva, -1 = rotación negativa

        [Header("IA Diff")]
        public Renderer enemyRenderer;
        [SerializeField] private float dificultad;
        public Difficulty difficultyMode;
        public float HP;
        public enum Difficulty
        {
            //Le chaqueteamos unos enums para poder seleccionar la dificultad de los enemigos
            modoDiablo, //Dificultad alta
            HPModifier, //Modificador de HP
            darkwarrior777, //Dificultad media no tan cabrona
            elRotador //Le aumenta el rango y velocidad de bala al enemigo, al chile está bien perro este
        }
    
        void Start()
        {
            enemyRenderer = GetComponent<Renderer>();
            if(enemyRenderer == null)
            {
                Debug.LogError("No se encontró Renderer en el enemigo");
            }
        
            // Se guardan los valores base de los parámetros, ya que los necestiamos para modificarlos de manera local en cada difficultyMode
            float baseIdleRotationSpeed = idleRotationSpeed;
            float baseMaxRotationAngle = maxRotationAngle;
            float baseBalaVelocidad = BalaVelocidad;
            float baseConeDistance = coneDistance;
            float baseHP = GameManager.gameManager.juanitoTorreta.Health;
        
            // Cálculo de dificultad y modificaciones específicas para cada modo
            switch(difficultyMode) //Hola un switch para seleccionar la dificultad
            {
                case Difficulty.modoDiablo: 
                /* 
                Se modifican todos los valores (menos coneDistance) y el escalado de la dificultad es el mas perro aquí, 
                este enemigo es el que tiene los mayores escalados y es raro ver uno de color amarillo*/
                    dificultad = GameManager.gameManager.juanitoTorreta.Health * Random.Range(0.8f, 2.0f) +
                                 baseIdleRotationSpeed * Random.Range(0.5f, 1.0f) +
                                 baseMaxRotationAngle * Random.Range(0.5f, 1.0f) +
                                 baseBalaVelocidad * Random.Range(0.5f, 1.0f); 
                    // Solo modificamos los parámetros de rotación y disparo
                    idleRotationSpeed = baseIdleRotationSpeed * (1 + dificultad / 800f);
                    maxRotationAngle = baseMaxRotationAngle * (1 + dificultad / 800f);
                    BalaVelocidad = baseBalaVelocidad * (1 + dificultad / 800f);
                    HP = baseHP + (1 + dificultad / 800f);

                    //No modificado
                    coneDistance = baseConeDistance;
              
                    break;
                
                case Difficulty.HPModifier:

                /* 
                Este es puro escalado de vida, algo sencillito
                */
                    dificultad = (GameManager.gameManager.juanitoTorreta.Health * Random.Range(0.0f, 1.0f) + 
                                  baseBalaVelocidad * Random.Range(0.0f, 1.0f)) / 2;
                    // Se aplica la modificación principalmente a la HP
                    HP = baseHP * (1 + dificultad / 400f);
                    BalaVelocidad = baseBalaVelocidad * (1 + dificultad / 300f);

                    //No modificado
                    idleRotationSpeed = baseIdleRotationSpeed;
                    maxRotationAngle = baseMaxRotationAngle;
                    coneDistance = baseConeDistance;
                    break;
                
                case Difficulty.darkwarrior777:
                /* 
                    Este es un enemigo con dificultad media, no tan cabrona, no conteine escalado de vida
                */
                    dificultad = (baseIdleRotationSpeed * Random.Range(0.0f, 1.0f) +
                                  baseMaxRotationAngle * Random.Range(0.0f, 1.0f) +
                                  baseBalaVelocidad * Random.Range(0.0f, 1.0f) * 2);
                    // Se aplican modificaciones a casi todos los parámetros, pero con distintos factores
                    idleRotationSpeed = baseIdleRotationSpeed * (1 + dificultad / 600f);
                    maxRotationAngle = baseMaxRotationAngle * (1 + dificultad / 600f);
                    BalaVelocidad = baseBalaVelocidad * (1 + dificultad / 600f);
                    //No modificado
                    coneDistance = baseConeDistance;
                    HP = baseHP;
                    break;
                
                case Difficulty.elRotador:
                /* 
                Borren a este wey porfas, tiene un rango de disparo muy grande, rotación rápida y balas muy veloces
                */
                    dificultad = baseIdleRotationSpeed * Random.Range(2.0f, 5.0f) * 5f + 
                                 coneDistance * Random.Range(0.5f, 1.0f) * 2f + 
                                 BalaVelocidad * Random.Range(0.5f, 1.0f) * 2f;
                    //ola yonesi y niggawarrior
                    idleRotationSpeed = baseIdleRotationSpeed * (1 + dificultad / 800f);
                    BalaVelocidad = baseBalaVelocidad * (1 + dificultad / 200);
                    coneDistance = baseConeDistance * (1 + dificultad / 200);
                    //No modificado
                    maxRotationAngle = baseMaxRotationAngle;
                    HP = baseHP;
                    break;
                
                default:
                    Debug.LogError("Modo de dificultad no reconocido");
                    break;
            }


        // ===========
        // 2. Crear EnemyStats basado en los valores ya modificados
        // ===========
        EnemyStats baseStats = new EnemyStats();
            baseStats.maxHP = HP;
            baseStats.bulletSpeed = BalaVelocidad;
            baseStats.rotationSpeed = idleRotationSpeed;
            baseStats.rotationAngle = maxRotationAngle;
            baseStats.coneDistance = coneDistance;
            baseStats.difficultyValue = DifficultyFunction(baseStats); // Puedes basarla en como midas dificultad

            PcgEnemy baseEnemy = new PcgEnemy();
            baseEnemy.stats = baseStats;

            // ===========
            // 3. Aplicar GreedySearch
            // ===========
            EnemyStats mejorado = GreedySearch(baseEnemy);

            // ===========
            // 4. Actualizar los valores del enemigo con lo que mejoró
            // ===========
            HP = mejorado.maxHP;
            BalaVelocidad = mejorado.bulletSpeed;
            idleRotationSpeed = mejorado.rotationSpeed;
            maxRotationAngle = mejorado.rotationAngle;
            coneDistance = mejorado.coneDistance;
            dificultad = mejorado.difficultyValue;

        Debug.Log($"[LookAtYon] Stats FINALES después de GreedySearch => " +
        $"HP: {HP}, BulletSpeed: {BalaVelocidad}, " +
        $"RotationSpeed: {idleRotationSpeed}, RotationAngle: {maxRotationAngle}, " +
        $"ConeDistance: {coneDistance}, DifficultyValue: {dificultad}");
        
        // ===========
        // 5. Terminar como siempre
        // ===========
        SetEnemyColor(dificultad);
            StartCoroutine(rotateIdle());

            Debug.Log("Stats finales tras switch + GreedySearch: " + mejorado.PrintStats());
        }

        void Update()
        {
            Vector3 juanitoDirection = juanitoTorreta.forward;

            // Verificar si el jugador está dentro del cono de visión
            if (Utilities.Utility.inInsideCone(coneAngle, coneDistance, player.position, juanitoTorreta.position, juanitoDirection))
            {
                if (!isDetected)
                {
                    isDetected = true;
                     // Parar la rotación si se detecta al jugador
                    StartCoroutine(lockIn());      // Iniciar la corutina de fijación de vista (lock-in)
                }
            }
            else
            {
                if (isDetected)
                {
                    isDetected = false;
                    StopCoroutine(lockIn());       // Parar la fijación de vista
                     // Volver a iniciar la rotación
                }
            }
        }

        EnemyStats GreedySearch(PcgEnemy origin)
        {
            EnemyStats currentNode = origin.stats;

            PriorityQueue openList = new PriorityQueue(false); // Menor dificultadValue primero
            openList.Enqueue(currentNode, currentNode.difficultyValue);

            HashSet<EnemyStats> closedList = new HashSet<EnemyStats>();

            int maxGreedySearchIterations = 50;
            float greedySearchTolerance = 0.05f; // Umbral de mejora para seguir buscando

            int currentIteration = 0;

            while (currentIteration < maxGreedySearchIterations && openList.Count > 0)
            {
                currentIteration++;
                currentNode = openList.Dequeue();
                closedList.Add(currentNode);

                // Generar vecinos (pequeñas variaciones)
                GenerateNeighbors(currentNode, openList);

                // Condición de terminación greedy: ¿el mejor vecino no mejora suficiente?
                if (openList.First().Item2 + greedySearchTolerance < currentNode.difficultyValue)
                {
                    break;
                }
            }

            return currentNode;
        }

    void GenerateNeighbors(EnemyStats current, PriorityQueue openList)
    {
        float hpStep = current.maxHP * 0.05f; // 5% de su valor actual
        float speedStep = current.bulletSpeed * 0.05f; // 5%
        float rotationStep = current.rotationSpeed * 0.05f; // 5%
        float angleStep = 5f; // dejamos igual
        float coneStep = 0.2f; // dejamos pequeño

        EnemyStats[] neighbors = new EnemyStats[]
        {
        new EnemyStats(current) { maxHP = Mathf.Clamp(current.maxHP + hpStep, 300, 800) },
        new EnemyStats(current) { bulletSpeed = Mathf.Clamp(current.bulletSpeed + speedStep, 300, 600) },
        new EnemyStats(current) { rotationSpeed = Mathf.Clamp(current.rotationSpeed + rotationStep, 15, 50) },
        new EnemyStats(current) { rotationAngle = Mathf.Clamp(current.rotationAngle + angleStep, 30, 90) },
        new EnemyStats(current) { coneDistance = Mathf.Clamp(current.coneDistance + coneStep, 38, 45) },
        };

        foreach (var neighbor in neighbors)
        {
            neighbor.difficultyValue = DifficultyFunction(neighbor);
            openList.Enqueue(neighbor, neighbor.difficultyValue);
        }
    }

    float DifficultyFunction(EnemyStats stats)
        {
            //Los pesos si se quiere que HP pese más, o balaVelocidad pese más
            return ((stats.maxHP * 0.2f) +
                 (stats.bulletSpeed * 0.1f) +
                 (stats.rotationSpeed * 0.1f) +
                 (stats.rotationAngle * 0.1f) +
                 (stats.coneDistance * 0.05f)) * difficultyWeight;
        }

        public void SetDifficultyWeight(float newWeight)
        {
        difficultyWeight = newWeight; // SIN poner LookAtYon.difficultyWeight
        }

    void OnDrawGizmos()
        {
            if (DebugGizmoManager.VisionCone)
            {
                Utility.DrawVisionCone(juanitoTorreta.position, coneAngle, coneDistance, isDetected, juanitoTorreta);
            }
        }
        void yonBombing()
        {
            if(!block){
                GameObject Balatemporal = Instantiate(Mira, BalaPrefabInicio.transform.position, BalaPrefabInicio.transform.rotation) as GameObject;
                Rigidbody rb = Balatemporal.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * BalaVelocidad);

                Destroy(Balatemporal, 2f);
                StartCoroutine(WaitFor());
            }
        }

        IEnumerator rotateIdle()
        {
            isRotating = true;
            while (!isDetected)
            {
                // Calcular el nuevo ángulo
                currentRotationAngle += idleRotationSpeed * rotationDirection * Time.deltaTime;

                // Si alcanza el ángulo máximo o mínimo, cambiar la dirección de la rotación
                if (Mathf.Abs(currentRotationAngle) >= maxRotationAngle)
                {
                    rotationDirection *= -1;  // Invertir la dirección de la rotación
                }

                // Aplicar la rotación usando RotateAround
                juanitoTorreta.Rotate(0, idleRotationSpeed * rotationDirection * Time.deltaTime, 0);
                yield return null;
            
            }
            isRotating = false;
        }

        IEnumerator lockIn()
        {
            while (isDetected)  // Mientras el jugador esté dentro del rango de visión
            {
                // Hacer que juanitoTorreta mire al jugador
                Vector3 relativePos = player.position - juanitoTorreta.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                juanitoTorreta.rotation = Quaternion.Slerp(juanitoTorreta.rotation, toRotation, Time.deltaTime * 5f);
                yonBombing();
                yield return null; // Continuar cada frame
            }
            yield return new WaitForSeconds(2f);
            yonBombing();
            StartCoroutine(rotateIdle());
        }

        IEnumerator WaitFor()
        {
            block = true;
            yield return new WaitForSeconds(0.5f);
            block = false;
        }


        #region IA Diff
        void SetEnemyColor(float diff)
    {
        if(diff >= 800) 
        {
            enemyRenderer.material.color = Color.red; // Dificultad alta
        } 
        else if(diff >= 500) 
        {
            enemyRenderer.material.color = Color.yellow; // Dificultad media
        } 
        else 
        {
            enemyRenderer.material.color = Color.green; // Dificultad baja
        }
    }
        #endregion
    }
