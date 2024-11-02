# Investigación por parte del alumno: Navigation Mesh
Equipo: Jhon Zorrilla y Bruno Amezcua (y)

Este proyecto implementa un escenario básico con un agente de navegación y un agente con cono de visión en Unity. A través de un `Navigation Mesh` y scripts personalizados, los agentes interactúan en el entorno y responden a la detección del jugador.

## Contenido

- [Requisitos del Escenario](#requisitos-del-escenario)
- [Configuración de Navigation Mesh y Navigation Agent](#configuración-de-navigation-mesh-y-navigation-agent)
- [Implementación del Agente de Movimiento](#implementación-del-agente-de-movimiento)
- [Implementación del Agente con Cono de Visión](#implementación-del-agente-con-cono-de-visión)
- [Interacciones y Eventos Especiales](#interacciones-y-eventos-especiales)
- [Recursos](#recursos)

## Requisitos del Escenario

1. **Escenario Simple**:
    - Crea un escenario en Unity que incluya:
      - Una inclinación o pendiente.
      - Un puente que permita pasar tanto por encima como por debajo.
      - Al menos dos superficies elevadas con separación suficiente para saltar entre ellas.
    - **Ejemplo**: [Escenario de salto y navegación](https://youtu.be/atCOd4o7tG4?si=WS6QOQQ-9PIW-iDI&t=515) (ver círculos y arcos de referencia en el video).

## Configuración de Navigation Mesh y Navigation Agent

1. **Crear Navigation Mesh**:
    - Define un `Navigation Mesh` en Unity que abarque todo el escenario.
    - Asegúrate de incluir áreas de navegación en los puentes, inclinaciones y plataformas elevadas para que los agentes puedan moverse correctamente.

2. **Agregar Navigation Agent**:
    - Configura un agente de navegación (`NavMeshAgent`) en el objeto del agente.
    - Este agente utilizará el `Navigation Mesh` para desplazarse automáticamente hacia los puntos objetivo.

## Implementación del Agente de Movimiento

1. **Script de Movimiento para el Agente**:
    - Crea un script que permita al agente de navegación moverse hacia un punto específico en el escenario.
    - **Ejemplo**: [Implementación de movimiento en el agente](https://youtu.be/atCOd4o7tG4?si=QjfJ_GzzxKZ_Uo_6&t=470).

## Implementación del Agente con Cono de Visión

1. **Script para el Cono de Visión**:
    - Crea un segundo script que implemente un agente con un cono de visión.
    - Puedes reutilizar la función de cono de visión de tu primer parcial para detectar objetos en el rango de visión.

2. **Detección y Persecución**:
    - Cuando el agente con cono de visión detecte al `Player` o al agente de navegación en su rango, debe:
      - **Perseguir al objetivo** durante 3 segundos.
      - **Detenerse** en la última posición cuando termine el tiempo.

3. **Regreso a Posición Inicial (Opcional)**:
    - Al finalizar los 3 segundos de persecución, el agente con cono de visión puede regresar a su posición inicial de patrullaje.
    - Esta función opcional agrega realismo al comportamiento de patrullaje y detección.

## Interacciones y Eventos Especiales

1. **Destrucción o Daño al Contacto**:
    - Si el agente del cono de visión toca al otro agente, debe aplicarle daño o destruirlo.

2. **Reaparición del Agente Destruido**:
    - Implementa un botón o tecla que permita reaparecer al agente destruido en el escenario para reiniciar la interacción.
