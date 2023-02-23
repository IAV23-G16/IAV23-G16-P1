/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de HUIR a otro agente
    /// </summary>
    public class Huir : Persecucion
    {
        GameObject[] ratas;

        [SerializeField] protected GameObject ratasEmpty;
        int ratasCerca;

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion d = new Direccion();
            ratasCerca = 0;

            ratas = new GameObject[ratasEmpty.transform.childCount];

            for (int i = 0; i < ratasEmpty.transform.childCount; i++)
            {
                ratas[i] = ratasEmpty.transform.GetChild(i).gameObject;
            }

            foreach (GameObject rata in ratas)
            {
                Vector3 dir = transform.position - (rata.transform.position + rata.GetComponent<Rigidbody>().velocity * predictCoef);
                float dist = dir.magnitude;
                float speedDiff;

                if (dist < rObjetivo)
                {
                    speedDiff = maxSpeed - lastDir.lineal.magnitude;
                    ratasCerca++;
                }
                else if (dist > rRalentizado)
                {
                    speedDiff = 0;
                }
                else
                {
                    ratasCerca++;
                    float targetSpeed = maxSpeed * rRalentizado / dist;

                    speedDiff = targetSpeed - lastDir.lineal.magnitude;

                    d.lineal = dir.normalized * speedDiff;

                    if (d.lineal.magnitude > maxAccel)
                    {
                        speedDiff = maxSpeed - lastDir.lineal.magnitude;
                    }
                }

                d.lineal += dir.normalized * speedDiff;
                lastDir.lineal = d.lineal;
                d.lineal /= timeToAccel;
                

                if (d.lineal.magnitude > maxAccel)
                {
                    d.lineal = d.lineal.normalized * maxAccel;
                }

                // Debug.Log("Ratas: " + ratasCerca + " " + d.lineal);

            }

            if (ratasCerca > 1) {
                return d;
            }
            else return new Direccion();
        }
    }
}
