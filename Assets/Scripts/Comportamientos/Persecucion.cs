using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de PERSEGUIR a otro agente, con predicción
    /// </summary>
    public class Persecucion : Llegada
    {
        [SerializeField] protected float predictCoef;

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            // Obtener dirección de aquí al objetivo
            Vector3 dir = objetivo.transform.position + objetivo.GetComponent<Rigidbody>().velocity * predictCoef - transform.position;
            float dist = dir.magnitude;
            Direccion d = new Direccion();
            float speedDiff;

            // Si estamos dentro del radio objetivo, frenamos
            if (dist < rObjetivo)
            {
                speedDiff = -lastDir.lineal.magnitude;
            }

            // Si estamos fuera del radio de ralentizado, vamos a velocidad máxima
            else if (dist > rRalentizado)
            {
                speedDiff = maxSpeed - lastDir.lineal.magnitude;
            }

            // Si estamos entre ambos radios, la velocidad objetivo es proporcional a la distancia hasta el objetivo
            else
            {
                float targetSpeed = maxSpeed * dist / rRalentizado;

                speedDiff = targetSpeed - lastDir.lineal.magnitude;

                d.lineal = dir.normalized * speedDiff;

                // Sin pasarnos de la aceleración máxima
                if (d.lineal.magnitude > maxAccel)
                {
                    speedDiff = maxSpeed - lastDir.lineal.magnitude;
                }
            }

            d.lineal = dir.normalized * speedDiff;
            lastDir.lineal = d.lineal;
            d.lineal /= timeToAccel;

            return d;

        }


    }
}
