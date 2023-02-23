/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de SEGUIR a otro agente
    /// </summary>
    public class Llegada : ComportamientoAgente
    {
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected float maxAccel;

        // El radio para llegar al objetivo
        public float rObjetivo;

        // El radio en el que se empieza a ralentizarse
        public float rRalentizado;

        protected Direccion lastDir = new Direccion();

        public float timeToAccel = 0.03f;


        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            // Se calcula dirección al objetivo
            Vector3 dir = objetivo.transform.position - transform.position;
            float dist = dir.magnitude;
            Direccion d = new Direccion();
            float speedDiff;

            // Si estamos dentro del radio objetivo, frenamos
            if (dist < rObjetivo)
            {
                speedDiff = -lastDir.lineal.magnitude * 0.99f;
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
            Debug.Log(d.lineal);

            return d;

        }


    }
}
