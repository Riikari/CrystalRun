using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Player player;
    Text user;
    Text input;
    string ID;

    GameObject titulo;
    GameObject login;
    GameObject ranking;
    

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        titulo = GameObject.Find("Titulo");
        login = GameObject.Find("Login");
        ranking = GameObject.Find("Ranking");
        input = GameObject.Find("NombreSubmit").GetComponent<Text>();
        user = GameObject.Find("User").GetComponent<Text>();

        //PlayerPrefs.SetString("ID", "");
        //PlayerPrefs.SetInt("Record", 0);

        //Obtenemos el Username del jugador, si no existe, se le pide que lo introduzca
        ID = PlayerPrefs.GetString("ID");
        user.text = ID;

        if (ID == "")
        {
            titulo.SetActive(false);
        }
        else
        {
            login.SetActive(false);
        }
        ranking.SetActive(false);
    }

    public void Update()
    {
        if (titulo)
        {
            Input.backButtonLeavesApp = true;
        }
        else
        {
            Input.backButtonLeavesApp = false;
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

    //Función que almacena el username del jugador
    public void Login()
    {
        PlayerPrefs.SetInt("Record", 0);
        //Se impide que el jugador pueda meter un username vacío
        ID = input.text;
        if (ID == "")
        {
            Text pchdr = GameObject.Find("Placeholder").GetComponent<Text>();
            pchdr.text = "Introduce un nombre valido...";
        }
        else
        {
            //Se guarda el usuario del jugador.
            PlayerPrefs.SetString("ID", ID);
            user.text = ID;

            login.SetActive(false);
            titulo.SetActive(true);
        }
        
    }

    public void Ranking()
    {
        titulo.SetActive(false);
        ranking.SetActive(true);
    }

    public void TituloPrin() 
    {
        ranking.SetActive(false);
        titulo.SetActive(true);
    }
}

