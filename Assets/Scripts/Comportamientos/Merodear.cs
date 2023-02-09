/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Merodear : ComportamientoAgente
    {
        [SerializeField] float merodSpeed = 5;

        [SerializeField]
        float tiempoMaximo = 2.0f;

        [SerializeField]
        float tiempoMinimo = 1.0f;

        float t = 3.0f;
        float actualT = 2.0f;

        Direccion lastDir = new Direccion();

        float getRandomTimeAmount()
        {
            return (float)Random.Range(tiempoMinimo * 100, tiempoMaximo * 100) / 100;
        }

        void changeDir()
        {
            lastDir.lineal.x = Random.Range(-100, 100);
            lastDir.lineal.z = Random.Range(-100, 100);
            lastDir.lineal.Normalize();
            lastDir.lineal *= merodSpeed;
            Invoke(nameof(changeDir), getRandomTimeAmount());
        }

        public override Direccion GetDireccion(){

            if (lastDir.lineal == Vector3.zero)
            {
                changeDir();
            }

            return lastDir;
        }

    }
}
