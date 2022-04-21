using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BckGrndScroller : MonoBehaviour
{
    [SerializeField] Transform First, Second;
    [SerializeField] float scrollSpeed = 1f, horizontalDifference = 18.74f, minimunWidth = -10;
    public float profundidad = 1;

    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Calcula la velocidad de scroll del fondo en base a la velocidad del personaje y la profundidad del mismo
        scrollSpeed = player.velocidad.x / profundidad;

        First.position = new Vector2(First.position.x - Time.deltaTime * scrollSpeed, First.position.y);
        Second.position = new Vector2(Second.position.x - Time.deltaTime * scrollSpeed, Second.position.y);
        Repeat();
    }

    void Repeat()
    {
        //El fondo se traslada a la derecha de la pantalla cuando llega a la posición X
        if (First.localPosition.x < minimunWidth)
        {
            First.position = new Vector2(Second.position.x + horizontalDifference, Second.position.y);
        }

        if (Second.localPosition.x < minimunWidth)
        {
            Second.position = new Vector2(First.position.x + horizontalDifference, First.position.y);
        }
    }
}
