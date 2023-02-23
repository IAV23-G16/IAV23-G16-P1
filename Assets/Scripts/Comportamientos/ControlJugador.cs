/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UIElements;

    /// <summary>
    /// El comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class ControlJugador: ComportamientoAgente
    {
        [SerializeField] Camera mainCam;
        [SerializeField] float distanceWalkCloseLimit;
        [SerializeField] LayerMask layerMask;
        Vector3 targetPos;

        private void Start()
        {
            targetPos = transform.position;
        }

        /// <summary>
        /// Obtiene la direcci�n
        /// </summary>
        /// <returns></returns>

        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();

            if (Input.GetButton("WalkToPoint"))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return direccion;

                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, 100, layerMask);

                targetPos = hit.point;
            }

            ////Direccion actual
            direccion.lineal.x = targetPos.x - transform.position.x;
            direccion.lineal.z = targetPos.z - transform.position.z;

            if (Mathf.Abs(direccion.lineal.x) < distanceWalkCloseLimit && Mathf.Abs(direccion.lineal.z) < distanceWalkCloseLimit)
            {
                direccion.lineal.x = 0;
                direccion.lineal.z = 0;
            }

            //direccion.lineal.x = Input.GetAxis("Horizontal");
            //direccion.lineal.z = Input.GetAxis("Vertical");

            //Resto de c�lculo de movimiento
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            // Podr�amos meter una rotaci�n autom�tica en la direcci�n del movimiento, si quisi�ramos

            return direccion;
        }
    }
}