using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;
using UnityEngine;

public delegate void VoidFunc3Int(int x, int y, int value);

namespace Assets.Scripts
{
    public class MemMaze
    {
        Dictionary<(int, int), int> maze;

        int maxX = 0;
        int maxY = 0;

        public Dictionary<(int, int), int> Maze { get { return maze; } }

        public VoidFunc3Int iteratorDelegate;


        public MemMaze()
        {
            maze = new Dictionary<(int, int), int>();
            iteratorDelegate = (x, y, v) => { };

        }

        public void Clear() { 
            maze.Clear();
        }

        public int GetValueAt(int x, int y)
        {
            return maze[(x,y)] ;
        }

        public int GetValueAt(float x, float y)
        {
            return maze[((int)x, (int)y)];
        }

        public int GetValueAt(Vector3 v)
        {
            return maze[((int)v.x, (int)v.y)];
        }

        public int SetValueAt(int x,int y, int value)
        {
            return maze[(x, y)] = value;

        }
        public int SetValueAt(float x, float y, int value)
        {
            return maze[((int)x, (int)y)] = value;

        }
        public int SetValueAt(Vector3 v, int value)
        {
            return maze[((int)v.x, (int)v.y)] = value;
        }

        public int CombineValueAt(int x, int y, int value)
        {
            return maze[(x,y)] |= value;
        }
        public int CombineValueAt(float x, float y, int value)
        {
            return maze[((int)x,(int)y)] |= value;
        }
        public int CombineValueAt(Vector3 v, int value)
        {
            return maze[((int)v.x, (int)v.y)] |= value;
        }

        public void Fill(int maxX, int maxY, int value = 0)
        {
            this.maxX = maxX;
            this.maxY = maxY;

            for(int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    maze[(x, y)] = value;
                }
            }
            // mark walls and shift 4 bits
            // top 1, right 2, bottom 4, left 8

            for(int y = -1; y < maxY +1; y++)
            {
                //llenamos de rooms completas a los bordes del Y -> | R |.
                maze[(-1, y)] = 15; 
                maze[(maxX, y)] = 15;
            }

            for(int x = -1; x < maxX; x++)
            {
                //llenamos de rooms completas a los bordes del X ->  techo R suelo.
                maze[(x, maxY)] = 15;
                maze[(x, -1)] = 15;
            }

        }

        public void Add( Vector3 pos, Vector3 lastPos, int turn, int invTurn)
        {
            CombineValueAt(pos, 1 << invTurn);
            CombineValueAt(lastPos, 1 << turn);
        }

        public void AddColor(Vector3 pos, int colorID)
        {
            int value = GetValueAt(pos) & 0x0F; // obtengo los primeros 4 valores, del 0 al 15, osea 101001110 -> 0000001110
            SetValueAt(pos, (colorID << 4 | value)); // le agrego datos a posicion. Si el id color es 0110, el valor queda color-> 0110+1110 <-pos
        }
        
        public void Iterate()//Recorre el dictionario en el orden almacenado
        {
            foreach(KeyValuePair<(int, int), int> m in maze)
            {
                iteratorDelegate(m.Key.Item1, m.Key.Item2, m.Value);
            }
        }

        public void IterateRect() // Recorre las casillas de forma rectangular, pero ignora el borde externo
        {
            for(int y = 0; y < maxY; y++)
            {
                for(int x = 0; x < maxX; x++)
                {
                    iteratorDelegate(x, y, maze[(x, y)]);
                }
            }
        }

        public int GetEmptyCount() { 
        
            int count = 0;
                
                foreach(KeyValuePair<(int,int),int> m in maze)
            {
                if((m.Value & 0x0F) == 0) { count++; } // obtiene ultimos 4 valores, si es 0 se suma +1 a count, si 1110000 -> se toma 0000, esta vacio!
            }

            return count;
        }

        public bool isFull()
        {
            return GetEmptyCount() == 0; // si la matriz esta llena, entonces la funcion de arriba devuelve 0
        }

        public int GetNeighborsAt(int x, int y)
        {
            int result = 0;
            result |= maze[(x, y + 1 )].bit(); // celda de arriba ocupada, da 0001, si esta vacia sera 0
            result |= maze[(x + 1 , y  )].bit() << 1; // celda derecha
            result |= maze[(x , y - 1)].bit() << 2; // celda abajo
            result |= maze[(x - 1, y)].bit() << 3; // celda izquierda

            return result; // Ejm, si celda top y bottom, ocupadas: 0101. Si celda top right ocupadas: 0011
        }

        public int GetNeighborsAt(float x, float y)
        {
            return GetNeighborsAt((int)x, (int)y);
        }
        public int GetNeighborsAt(Vector3 v)
        {
            return GetNeighborsAt((int)v.x, (int)v.y);
        }


    }
}
