using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class UIController : MonoBehaviour
{
    Player player;
    Text textoDistancia;
    Text textoDistanciaFinal;
    Text recordDistancia;
    Text nombre;
    public int APIID;

    GameObject resultados;
    GameObject titulo;
    GameObject login;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        textoDistancia = GameObject.Find("Distancia").GetComponent<Text>();

        textoDistanciaFinal = GameObject.Find("DistanciaTotal").GetComponent<Text>();
        recordDistancia = GameObject.Find("Record").GetComponent<Text>();
        resultados = GameObject.Find("Resultados");
        resultados.SetActive(false);        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Se actualiza el texto en la pantalla con la distancia recorrida por el jugador
        int distancia = Mathf.FloorToInt(player.distancia);
        textoDistancia.text = distancia + " M";

        //Cuando el jugador muere se muestra la pantalla con los resultados
        if (player.isDead)
        {
            //Obtenemos el último record registrado
            int record = PlayerPrefs.GetInt("Record");

            resultados.SetActive(true);
            textoDistanciaFinal.text = distancia + " M";

            //En caso de que la distancia recorrida sea mayor que la del record registrado, se actualiza
            if (distancia > record)
            {
                PlayerPrefs.SetInt("Record", distancia);
                //Conecta con la base de datos y actualiza el último valor almacenado en ella
                SubmitScore(distancia);
            }

            recordDistancia.text = record + " M";
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu2");
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //Función que conecta con la base de datos y actualiza el valor de distancia recorrida
    public void SubmitScore(int dist)
    {
        LootLockerSDKManager.StartSession("Player", (response) =>
        {
            if (response.success)
            {
                string ID = PlayerPrefs.GetString("ID");
                LootLockerSDKManager.SubmitScore(ID, dist, APIID, (response) =>
                {
                    if (response.success)
                    {

                    }
                    else
                    {

                    }
                });
            }
            else
            {

            }
        });
    }

}
