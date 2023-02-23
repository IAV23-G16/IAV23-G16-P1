using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de PERSEGUIR a otro agente, con predicci�n
    /// </summary>
    public class Persecucion : Llegada
    {
        [SerializeField] protected float predictCoef;

        /// <summary>
        /// Obtiene la direcci�n
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            // Obtener direcci�n de aqu� al objetivo
            Vector3 dir = objetivo.transform.position + objetivo.GetComponent<Rigidbody>().velocity * predictCoef - transform.position;
            float dist = dir.magnitude;
            Direccion d = new Direccion();
            float speedDiff;

            // Si estamos dentro del radio objetivo, frenamos
            if (dist < rObjetivo)
            {
                speedDiff = -lastDir.lineal.magnitude;
            }

            // Si estamos fuera del radio de ralentizado, vamos a velocidad m�xima
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

                // Sin pasarnos de la aceleraci�n m�xima
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
