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
        [SerializeField] float maxSpeed;

        // El radio para llegar al objetivo
        public float rObjetivo;

        // El radio en el que se empieza a ralentizarse
        public float rRalentizado;

        public float fRalentizado;

        public int distancia = 7;

        // El tiempo en el que conseguir la aceleracion objetivo
        float timeToTarget = 0.1f;


        

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>

        public override Direccion GetDireccion()
        {
            Vector3 dir = objetivo.transform.position - transform.position;
            float dist = dir.magnitude;
            Direccion d = new Direccion();

            if (dist < rObjetivo)
            {
                return d;
            }

            if (dist > rRalentizado)
            {
                d.lineal = dir.normalized * maxSpeed;
                return d;
            }

            float slowSpeed = maxSpeed * dist / rRalentizado;
            d.lineal = dir.normalized * slowSpeed;
            return d;

        }


    }
}
