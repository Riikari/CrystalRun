using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float gravedad;
    public Vector2 velocidad;
    
    public float velMaxima = 100;
    public float maxAceleracion = 10;
    public float aceleracion = 10;

    public float distancia = 0;

    public float velSalto = 20;
    public float alturaBase = -3;
    public bool isGrounded;

    public bool isMantSalto = false;
    public float tiempoMaxSalto = 0.2f;
    public float tiempoMaxSaltoMax = 0.2f;
    public float tempSalto = 0.0f;

    public float thresholdSalto = 1;

    public bool isDead = false;

    public LayerMask layerMaskSuelo;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        float distSuelo = Mathf.Abs(pos.y - alturaBase);

        if (isGrounded || distSuelo <= thresholdSalto)
        {
            if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began))
            {
                isGrounded = false;
                velocidad.y = velSalto;
                isMantSalto = true;
                tempSalto = 0.0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isMantSalto = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        if (isDead)
        {
            return;
        }

        if (pos.y < -20)
        {
            isDead = true;
        }

        //Si el jugador no está tocando el suelo
        if (!isGrounded)
        {
            //Si está manteniendo el salto
            if (isMantSalto)
            {
                //Se le permite hacer al jugador un salto más largo en función del tiempo que mantenga el salto
                //y la velocidad del jugador
                tempSalto += Time.fixedDeltaTime;
                if (tempSalto >= tiempoMaxSalto)
                {
                    isMantSalto = false;
                }
            }
            //Se le aplica la velocidad del salto
            pos.y += velocidad.y * Time.fixedDeltaTime;
            
            //En cuanto el jugador deja de mantener el salto, o se le acaba el tiempo del mismo
            //la gravedad empieza a afectar al personaje
            if (!isMantSalto)
            {
                velocidad.y += gravedad * Time.fixedDeltaTime;
            }

            //Hacemos raycasting durante la caída para detectar las plataformas
            Vector2 origenRayo = new Vector2(pos.x , pos.y - 0.8f);
            Vector2 dirRayo = Vector2.up;                                       //Se hace hacia arriba ya que la velocidad que lleva es negativa
            float distRayo = velocidad.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(origenRayo, dirRayo, distRayo, layerMaskSuelo);

            if (hit2D.collider != null)
            {
                //Si detecta un objeto de tipo suelo
                Ground ground = hit2D.collider.GetComponent<Ground>();
                
                if (ground != null)
                {
                    //Si la posición desde que la detecta es mayor a la altura del suelo
                    if (pos.y >= ground.alturaSuelo)
                    {
                        //Se detiene la caída y se actualiza la altura del suelo a la que está el personaje
                        velocidad.y = 0.0f;

                        alturaBase = ground.alturaSuelo + 0.9f;
                        pos.y = alturaBase;

                        isGrounded = true;                                     //El personaje pasa a estar en el suelo
                    }
                }

            }

            //Hacemos raycasting hacia adelante para detectar colisiones con el lateral de las plataformas
            Vector2 origenPared = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(origenPared, Vector2.right, velocidad.x * Time.fixedDeltaTime, layerMaskSuelo);

            if (wallHit.collider != null)
            {
                //Si detecta colisión con un objeto de tipo Ground la velocidad del personaje pasa a 0
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.alturaSuelo)
                    {
                        velocidad.x = 0f;
                    }
                }
            }
            Debug.DrawRay(origenPared, Vector2.right * velocidad.x * Time.fixedDeltaTime, Color.red);
        }

        //Calculamos la distancia recorrida en base a la velocidad del jugador y el tiempo de juego
        distancia += velocidad.x * Time.fixedDeltaTime;

        //Si está en contacto con el suelo
        if (isGrounded)
        {
            //Calculamos un ratio de velocidad al que va a acelerar el personaje
            float ratioVel = velocidad.x / velMaxima;
            aceleracion = maxAceleracion * (1 - ratioVel);
            
            //Calculamos el tiempo que puede mantener el salto el jugador en función al ratio
            tiempoMaxSalto = tiempoMaxSaltoMax * ratioVel;

            velocidad.x += aceleracion * Time.fixedDeltaTime;
            if (velocidad.x >= velMaxima)
            {
                velocidad.x = velMaxima;
            }

            //Hacemos raycasting para detectar si el jugador deja de estar en contacto con una plataforma sin haber saltado
            Vector2 origenRayo = new Vector2(pos.x - 0.65f, pos.y-0.8f);
            Vector2 dirRayo = Vector2.down;
            float distRayo = 0.3f;
            RaycastHit2D hit2D = Physics2D.Raycast(origenRayo, dirRayo, distRayo, layerMaskSuelo);

            //Si no detecta ningún objeto el personaje deja de estar en el suelo
            if (hit2D.collider == null)
            {
                //Debug.Log("HE ENTRADO");
                isGrounded = false;
            }
            /*else
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();

                if (ground == null)
                {
                    Debug.Log("HE ENTRADO");
                    isGrounded = true;
                }
                else
                {
                    isGrounded = true;
                }
            }*/
            Debug.DrawRay(origenRayo, dirRayo * distRayo, Color.yellow);
        }


        transform.position = pos;
    }
}
