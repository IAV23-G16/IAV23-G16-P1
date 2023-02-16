/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Separacion : ComportamientoAgente
    {
        /// <summary>
        /// Separa al agente
        /// </summary>
        /// <returns></returns>

        // Entidades potenciales de las que huir
        public GameObject targEmpty;

        // Umbral en el que se activa
        [SerializeField]
        float umbral;

        // Coeficiente de reducci�n de la fuerza de repulsi�n
        [SerializeField]
        float decayCoefficient;

        private GameObject[] targets;

        public override Direccion GetDireccion()
        {
            Direccion dir = new Direccion();
            targets = new GameObject[targEmpty.transform.childCount];

            targEmpty = transform.parent.gameObject;


            for (int i = 0; i < targEmpty.transform.childCount; i++)
            {
                targets[i] = targEmpty.transform.GetChild(i).gameObject;
            }

            foreach (GameObject target in targets)
            {
                if (gameObject == target)   continue;

                Vector3 dist = target.transform.position - transform.position;
                if (dist.magnitude < umbral)
                {
                    float repulsionStrength = decayCoefficient / dist.sqrMagnitude;
                    dir.lineal += repulsionStrength * -dist.normalized;
                }
            }

            return dir;
        }
    }
}