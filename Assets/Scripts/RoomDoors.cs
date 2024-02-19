using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoors : MonoBehaviour
{
    public GameObject[] doors;
    // Start is called before the first frame update
    public string bi;
    public int valor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoors(int value)
    {
        valor = value;
        // Convertir el entero a su representación binaria de 4 bits
        string binario = Convert.ToString(value, 2).PadLeft(4, '0');
        bi = binario;
        // Recorrer cada dígito binario
        for (int i = 0; i < 4; i++)
        {
            // Obtener el dígito binario en la posición actual
            char digito = binario[i];

            // Obtener el estado activo o inactivo según el dígito binario
            bool activo = (digito == '1');

            // Establecer el estado activo o inactivo del objeto de la puerta correspondiente en el array 'doors'
            doors[i].SetActive(activo);

        }
    }
}
