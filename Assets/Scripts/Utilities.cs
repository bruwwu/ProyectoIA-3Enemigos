using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities{
    public class Utility{

        public static Vector3 visionCone(float angle, float distance)
        {
            // Mantiene la misma lógica, solamente que ahora es una clase static (esto por estar en Utilities).
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * distance;
        }

        public static void DrawVisionCone(Vector3 origin, float angle, float distance, bool isDetected, Transform agentTransform)
        {
            if (angle <= 0f) return;

            float halfFovAngle = angle / 2;

            Vector3 p1, p2;

            p1 = agentTransform.TransformDirection(visionCone(halfFovAngle, distance));
            p2 = agentTransform.TransformDirection(visionCone(-halfFovAngle, distance));

            Gizmos.color = isDetected ? Color.green : Color.red;
            Gizmos.DrawLine(origin, origin + p1);
            Gizmos.DrawLine(origin, origin + p2);
        }

       public static bool inInsideCone(float inConeAngle, float inConeDistance, Vector3 inTargetPos, Vector3 coneOrigin, Vector3 coneDirection)
        {
            // Calcular el vector dirección hacia el objetivo desde el origen del cono
            Vector3 targetDirection = inTargetPos - coneOrigin;

            // Calcular el ángulo entre la dirección del cono y la dirección hacia el objetivo
            float angleToTarget = Vector3.Angle(targetDirection.normalized, coneDirection.normalized);

            // Verificar si el ángulo está dentro del rango del cono
            if (angleToTarget < inConeAngle * 0.5f)
            {
                // Verificar si el objetivo está dentro de la distancia máxima del cono
                if (targetDirection.magnitude < inConeDistance)
                {
                    return true; // El objetivo está dentro del cono
                }
            }

            return false; // El objetivo no está dentro del cono
        }

    }
}
