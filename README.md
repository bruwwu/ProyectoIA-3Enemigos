# Parcial 2, Proyect: Navigation Mesh
Equipo: Jhon Zorrilla y Bruno Amezcua (y)

# Enemigo NavMeshEscapistEnemy y Obstáculo Móvil

## Descripción del Enemigo NavMeshEscapistEnemy

Se ha implementado una nueva clase de enemigo llamada **NavMeshEscapistEnemy** que utiliza **NavMesh** para su movimiento y no emplea **Steering Behaviors**. El comportamiento de este enemigo se describe de la siguiente manera:

### Comportamiento del Enemigo:

#### A) Disparo al Jugador:
- El enemigo siempre dispara hacia la posición actual del jugador en el momento de disparar.

#### B) Movimiento:
- El enemigo tiene un movimiento "ligero", lo que significa que acelera rápidamente, pero no alcanza grandes velocidades. Se recomienda jugar con los valores de configuración de **NavMeshAgent** para obtener el movimiento deseado.

#### C) Estados del Enemigo:
El enemigo tiene dos estados que determinan su comportamiento: **Cansado** y **Activo**.

##### Estado Cansado:
1. Al entrar en el estado de cansancio:
    - El enemigo deja de moverse y no tiene una posición como destino.
    - Se marca como **Cansado** y se inicia un temporizador de activación con duración de **X segundos**.
2. Cuando termina el temporizador, el enemigo pasa al **Estado Activo**.
3. Mientras está cansado, el enemigo solo puede disparar al jugador.
4. **Representación visual**: Se debe representar visualmente que el enemigo está cansado, para que el jugador lo note claramente.
5. **Extra**: Se puede hacer que el enemigo dispare más lentamente o con menos precisión mientras está cansado.

##### Estado Activo:
1. Al entrar en el **Estado Activo**:
    - El enemigo asigna la posición del jugador como destino para su **NavMeshAgent** y se acerca al jugador.
    - Si el enemigo tiene **línea de visión directa** con el jugador, debe dejar de moverse. Para esto, se utilizará un **Raycast**.
2. **Área de detección**:
    - El enemigo tiene un área de detección con un radio **X**.
    - Si el jugador está dentro del rango mientras el enemigo está cansado, no sucede nada.
    - Si el enemigo no está cansado, verá el siguiente comportamiento:
      - El enemigo intenta alejarse del jugador (flee).
      - Calcula un punto en la dirección opuesta a la del jugador para asignarlo como destino de su **NavMeshAgent**.
      - Si este punto no es válido, el enemigo buscará una posición en la dirección del jugador.
3. Una vez asignado el destino del punto de **flee**, comienza el **temporizador de cansancio**.
4. Cuando termina el temporizador, el enemigo pasa de nuevo al **Estado Cansado**.

5. Si el jugador no ha estado en línea de visión directa durante **X segundos**, el enemigo vuelve al **Estado Activo**.

#### Notas:
- Utiliza **Coroutines** en Unity para manejar los temporizadores y acciones relacionadas con el tiempo.

## Implementación de Obstáculo Móvil (Puntos Extra)

Se ha implementado un **obstáculo móvil** que interactúa con el **NavMesh**. El comportamiento de este obstáculo es el siguiente:

### Comportamiento del Obstáculo:

#### A) Obstáculo Móvil:
- Se crea una clase para representar obstáculos que se mueven en el escenario.
- Estos obstáculos deben ser considerados dentro del **NavMeshSurface** para que el **NavMeshAgent** pueda reaccionar ante ellos.

#### B) Movimiento del Obstáculo:
- El obstáculo se mueve de **X forma** cada **X cantidad de tiempo**. Ejemplos:
    - El obstáculo rota **90 grados cada 2 segundos**.
    - El obstáculo se mueve **5 unidades de distancia cada 6 segundos**.

#### C) Actualización del NavMesh:
- Cuando el obstáculo se mueve, el **NavMeshSurface** debe actualizarse automáticamente para reflejar los cambios en el escenario.
- Los actores en el camino del obstáculo serán desplazados (pueden ser afectados por las físicas de Unity o dañados/muertos si es necesario).

#### D) Animación/Advertencia:
- El obstáculo debe mostrar algún tipo de **efecto visual** o **animación** para que el jugador pueda anticipar el movimiento del obstáculo.
- Ejemplos de efectos:
    - **Polvo levantándose** cerca del obstáculo.
    - **Turbinas o cohetes** encendidos en la dirección en la que se moverá el obstáculo.
    - **Flechas brillando** o alguna **advertencia visual** en la dirección de movimiento.

#### E) Debug/Gizmos:
- Se debe agregar un **debug/gizmo** para mostrar cómo se mueve el obstáculo y cómo se actualiza el **NavMesh**.

## Requisitos Técnicos:

- **NavMesh 2D**: Para los desarrolladores que están implementando el juego en 2D, se recomienda usar [NavMeshPlus](https://github.com/h8man/NavMeshPlus) para trabajar con NavMesh en un entorno 2D.

---

### Implementación en Unity:

1. **Crear la clase NavMeshEscapistEnemy**:
    - Asegúrate de que el movimiento del enemigo se base completamente en **NavMeshAgent**.
    - Implementa los comportamientos de los estados **Cansado** y **Activo**, y utiliza **Raycasts** para verificar la línea de visión del jugador.
    - Utiliza **Coroutines** para gestionar los temporizadores de activación.

2. **Crear la clase de Obstáculo Móvil**:
    - Asegúrate de que los obstáculos interactúan con el **NavMeshSurface** y actualizan el **NavMesh** cuando se mueven.
    - Implementa efectos visuales o animaciones para hacer que los obstáculos sean anticipados por el jugador.

---
