using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public GameObject square;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTile(int x, int y, int valor, int color)
    {
        
        // Calcula la posición donde deseas instanciar el prefab
        Vector3 position = new Vector3(x, y, 0); // Aquí asumo que quieres instanciar en un plano XY en la coordenada z = 0

        // Instancia el prefab en la posición calculada
        GameObject newSquare = Instantiate(square, position, Quaternion.identity);
        if(color == 1)
        {
            
            newSquare.GetComponent<SpriteRenderer>().material.color =  Color.red;
        }
        if(color == 2)
        {
            newSquare.GetComponent<SpriteRenderer>().material.color = Color.green;
        }
        if (color == 3)
        {
            newSquare.GetComponent<SpriteRenderer>().material.color = Color.blue;
        }
        if(color == 4)
        {
            newSquare.GetComponent<SpriteRenderer>().material.color = Color.magenta;
        }
        if (valor == 0)
        {
            newSquare.GetComponent<SpriteRenderer>().material.color = Color.grey;
        }

       newSquare.GetComponent<RoomDoors>().OpenDoors(valor);
    }

    public void ClearMesh()
    {
        // Deberia eliminar toda la senda
        Debug.Log("Me han llamado Clear!");
    }

    public void UpdateMesh()
    {
        // ni idea
        Debug.Log("Me han llamado lakaa!");
    }
}
