/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UCM.IAV.Movimiento
{

    public class GestorJuego : MonoBehaviour
    {
        public static GestorJuego instance = null;

        [SerializeField]
        GameObject scenario = null;

        [SerializeField]
        GameObject rataPrefab = null;

        // textos UI
        [SerializeField]
        Text fRText;   
        [SerializeField]
        Text ratText;

        private GameObject rataGO = null;
        private int frameRate = 60;

        // Variables de timer de framerate
        int m_frameCounter = 0;
        float m_timeCounter = 0.0f;
        float m_lastFramerate = 0.0f;
        float m_refreshTime = 0.5f;

        private int numRats;

        private bool cameraPerspective = true;
        private void Awake()
        {
            //Cosa que viene en los apuntes para que el gestor del juego no se destruya entre escenas
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            rataGO = GameObject.Find("Ratas");
            Application.targetFrameRate = frameRate;
            numRats = rataGO.transform.childCount;
            ratText.text = numRats.ToString();
        }

        // Sustituir este viejo m�todo por una soluci�n m�s actual...
        private void OnLevelWasLoaded(int level)
        {
            rataGO = GameObject.Find("Ratas");
            ratText = GameObject.Find("NumRats").GetComponent<Text>();
            fRText = GameObject.Find("Framerate").GetComponent<Text>();
            numRats = rataGO.transform.childCount;
            ratText.text = numRats.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            // Timer para mostrar el frameRate a intervalos
            if (m_timeCounter < m_refreshTime)
            {
                m_timeCounter += Time.deltaTime;
                m_frameCounter++;
            }
            else
            {
                m_lastFramerate = (float)m_frameCounter / m_timeCounter;
                m_frameCounter = 0;
                m_timeCounter = 0.0f;
            }

            // Texto con el framerate y 2 decimales
            fRText.text = (((int)(m_lastFramerate * 100 + .5) / 100.0)).ToString();

            //Input
            if (Input.GetKeyDown(KeyCode.R))
                Restart();
            if (Input.GetKeyDown(KeyCode.T))
                HideScenario();
            if (Input.GetKeyDown(KeyCode.O))
                SpawnRata();
            if (Input.GetKeyDown(KeyCode.P))
                DespawnRata();
            if (Input.GetKeyDown(KeyCode.F))
                ChangeFrameRate();
            if (Input.GetKeyDown(KeyCode.N))
                ChangeCameraView();

        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void HideScenario()
        {
            if (scenario == null)
                return;

            if (scenario.activeSelf)
                scenario.SetActive(false);
            else
                scenario.SetActive(true);
        }

        public void SpawnRata()
        {
            if (rataPrefab == null || rataGO == null)
                return;

            GameObject rata = Instantiate(rataPrefab, rataGO.transform);
            rata.GetComponent<Separacion>().targEmpty = rataGO;
            rata.GetComponent<Merodear>().enabled = true;
            rata.GetComponent<Separacion>().enabled = false;
            rata.GetComponent<Llegada>().enabled = false;

            numRats++;
            ratText.text = numRats.ToString();
        }

        public void DespawnRata()
        {
            if (rataGO == null || rataGO.transform.childCount < 1)
                return;

            Destroy(rataGO.transform.GetChild(0).gameObject);

            numRats--;
            ratText.text = numRats.ToString();
        }

        private void ChangeFrameRate()
        {
            if (frameRate == 30)
            {
                frameRate = 60;
                Application.targetFrameRate = 60;
            }
            else
            {
                frameRate = 30;
                Application.targetFrameRate = 30;
            }
        }

        private void ChangeCameraView()
        {
            if (cameraPerspective){
                Camera.main.GetComponent<SeguimientoCamara>().offset = new Vector3(0, 15, -2);
                cameraPerspective = false;
            }
            else{
                Camera.main.GetComponent<SeguimientoCamara>().offset = new Vector3(0, 7, -10);
                cameraPerspective = true;
            }
        }
    }
}