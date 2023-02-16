using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de SEGUIR a otro agente
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
            Vector3 dir = objetivo.transform.position + objetivo.GetComponent<Rigidbody>().velocity * predictCoef - transform.position;
            float dist = dir.magnitude;
            Direccion d = new Direccion();
            float speedDiff;

            if (dist < rObjetivo)
            {
                speedDiff = -lastDir.lineal.magnitude;
            }
            else if (dist > rRalentizado)
            {
                speedDiff = maxSpeed - lastDir.lineal.magnitude;
            }
            else
            {
                float targetSpeed = maxSpeed * dist / rRalentizado;

                speedDiff = targetSpeed - lastDir.lineal.magnitude;

                d.lineal = dir.normalized * speedDiff;

                if (d.lineal.magnitude > maxAccel)
                {
                    speedDiff = maxSpeed - lastDir.lineal.magnitude;
                }
            }

            d.lineal = dir.normalized * speedDiff;
            lastDir.lineal = d.lineal;
            d.lineal /= timeToAccel;
            Debug.Log(d.lineal);

            return d;

        }


    }
}
