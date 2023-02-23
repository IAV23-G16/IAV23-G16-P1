/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

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

        // Entidades potenciales de las que huir
        public GameObject targEmpty;

        // Umbral en el que se activa
        [SerializeField]
        float umbral;

        // Coeficiente de reducción de la fuerza de repulsión
        [SerializeField]
        float decayCoefficient;

        private GameObject[] targets;

        /// <summary>
        /// Separa al agente
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion dir = new Direccion();
            targets = new GameObject[targEmpty.transform.childCount];

            targEmpty = transform.parent.gameObject;

            // Se añaden todos los hijos del empty de ratas como ratas a evadir
            for (int i = 0; i < targEmpty.transform.childCount; i++)
            {
                targets[i] = targEmpty.transform.GetChild(i).gameObject;
            }

            // Se recorren las ratas
            foreach (GameObject target in targets)
            {
                // Cada rata se ignora a sí misma
                if (gameObject == target)   continue;

                // Si hay una rata cerca, sumamos una aceleración para alejarnos, más fuerte cuanto más cerca está
                Vector3 dist = target.transform.position - transform.position;
                if (dist.magnitude < umbral)
                {
                    float repulsionStrength = decayCoefficient / dist.sqrMagnitude;
                    dir.lineal += repulsionStrength * -dist.normalized;
                }
            }

            // Se devuelve la suma de separarse de todas las demás ratas cercanas
            return dir;
        }
    }
}