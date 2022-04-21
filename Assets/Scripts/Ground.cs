using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float alturaSuelo;
    public float sueloDerecha;
    public float pantallaDerecha;
    public float pantallaIzquierda;

    public bool generoSuelo = false;

    public float ID;

    BoxCollider2D collider;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        collider = GetComponent<BoxCollider2D>();
        alturaSuelo = transform.position.y + (collider.size.y / 2);
        pantallaDerecha = (Camera.main.orthographicSize * Screen.width / Screen.height);
        pantallaIzquierda = -(pantallaDerecha);

        if (ID == 1)
            generoSuelo = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //El suelo se mueve a la velocidad del jugador / 2
        Vector2 pos = transform.position;
        pos.x -= (player.velocidad.x / 2) * Time.fixedDeltaTime;

        //Se calcula el borde derecho de la plataforma
        sueloDerecha = transform.position.x + (collider.size.x / 2);

        //Si el borde drecho del suelo se sale de la pantalla se destruye
        if (sueloDerecha < pantallaIzquierda)
        {
            Destroy(gameObject);
            return;
        }

        if (!generoSuelo)
        {
            //Genera una plataforma una vez su borde derecho entra en pantalla
            if (sueloDerecha < pantallaDerecha)
            {
                generoSuelo = true;
                generarSuelo();
            }
        }

        transform.position = pos;
    }

    void generarSuelo()
    {
        //Se genera una instancia de la plataforma
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float h1 = player.velSalto * player.tiempoMaxSalto;                     //Calculamos la altura máxima de salto vertical
        float t = player.velSalto / -player.gravedad;                           //Calculamos el tiempo que está el personaje en el aire
        float h2 = player.velSalto * t + (0.5f * (-player.gravedad * (t * t))); //Calculamos la altura máxima del arco que hace el personaje al saltar
        float alturaMaxSalto = h1 + h2;                                         //Altura máxima total
        float maxY = alturaMaxSalto * 0.7f;
        maxY += alturaSuelo;                                                    //Altura máxima a la que se puede generar una plataforma
        float minY = (-Camera.main.orthographicSize) + 1f;                      //Altura mínima a la que se puede generar una plataforma
        float actualY = Random.Range(minY, maxY) - goCollider.size.y / 2;       //Generamos una altura aleatoria dentro del rango

        pos.y = actualY - goCollider.size.y / 2;
        
        if(pos.y > 1.3f)
        {
            pos.y = 1.3f;
        }

        if(pos.y < -6.5f)
        {
            pos.y = -6.5f;
        }

        float t1 = t + player.tiempoMaxSalto;                                   //Calculamos el tiempo que está el personaje en el aire en un salto horizontal
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravedad);    //Calculamos el tiempo que tarda en hacer el arco de salto
        float totalTime = t1 + t2;                                              //Tiempo total que el personaje está en el aire
        float maxX = totalTime * player.velocidad.x;                    
        maxX *= 0.5f;
        maxX += sueloDerecha;                                                   //Distancia máxima a la que se genera una plataforma
        float minX = pantallaDerecha;                                           //Distancia mínima a la que se genera una plataforma
        float actualX = Random.Range(minX, maxX);                               //Generamos una distancia aleatoria dentro del rango


        pos.x = actualX + goCollider.size.x / 2;
        go.transform.position = pos;

        Ground goSuelo = go.GetComponent<Ground>();
        goSuelo.alturaSuelo = go.transform.position.y + (goCollider.size.y / 2);
    }
}
