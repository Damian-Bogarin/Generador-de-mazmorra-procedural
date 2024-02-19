using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class ControllerTracker : MonoBehaviour
    {
       // [SerializeField] int maxStep = 3;
        [SerializeField] int maxX = 4;
        [SerializeField] int maxY = 4;
        [SerializeField] int emptyCells = 0;

        [SerializeField] float period = 0.5f;


        MemTurtle turtle;
        MemMaze maze;
        TileMap tileMap;

        Stack<int> stack; // guarda las direcciones, para saber la ruta directa!
        int lastStackCount = 0;
        int blockRoom = 0;

        void Start()
        {
            Application.targetFrameRate = 30;
            stack = new Stack<int>();
            tileMap = GameObject.Find("TileMap").GetComponent<TileMap>();

            maze = new MemMaze();
            maze.iteratorDelegate = (x, y, value) =>
            {
                tileMap.AddTile(x, y, value & 0x0F, (value >> 4) & 0x0F);
            };

            turtle = new MemTurtle();
            turtle.SetClamp(maxX, maxY);
            turtle.forwardDelegate = (pos, lastPos, turn, invTurn) =>
            {
                maze.Add(pos, lastPos, turn, invTurn);
            };
            StartCoroutine("Tracker");

        }

        IEnumerator Tracker()
        {
            while (true) // no tiene final, lo hara hasta la muerte.
            {
                Track();
                yield return new WaitForSeconds(period);
            }

        }

        void Restart()
        {
            stack.Clear();
            maze.Clear();
            maze.Fill(maxX, maxY);
            maze.AddColor(turtle.Pos, 1);
        }

        void UpdateMazeView()
        {
            tileMap.ClearMesh();
            maze.IterateRect();
            tileMap.UpdateMesh();
        }

        void Track()
        {
            Restart(); // limpia todo, el maze
            while (maze.GetEmptyCount() > emptyCells) // esta es la funcion importante, aca se va add datos al maze y la tortuga se mueve
            {
                int neighbor = maze.GetNeighborsAt(turtle.Pos); // devolve los vecinos disponibles.
                if(neighbor < 15) // si neigbor es menor a 15, hay disponibles!
                {
                    if(blockRoom != 0)
                    {
                        maze.AddColor(turtle.Pos,4);
                        blockRoom--;
                    }
                    GoToNeighbor(neighbor); // ve a la celda disponible.
                }
                if(neighbor == 15 && stack.Count == lastStackCount) // si no hay disponibles, osea 1111  y count == lastStackCount entonces:
                {
                    lastStackCount = 0;
                    blockRoom++; // suma 1 para saber que debe existir un bloqueo.
                    maze.AddColor(turtle.Pos, 3);

                }
                if(neighbor == 15 && stack.Count > 0) // dar pasos hacia atras hasta encontrar una ruta nueva.
                {
                    turtle.TurnTo(stack.Pop());
                    turtle.Backward();
                }
            }

            maze.AddColor(turtle.Pos, 2);
            //GoalPath();
            UpdateMazeView();

        }

        void GoToNeighbor(int neighbor)
        {
            int[] ns = { 0,0,0,0};
            int count = 0;
            for(int i=0; i < 4; i++)
            {
                if(((neighbor >> i) & 0x01) == 0)
                {
                    ns[count] = i;
                    count++;
                }
            }

            int turn = ns[UnityEngine.Random.Range(0, count)];
            stack.Push(turn);
            lastStackCount = stack.Count;
            turtle.TurnTo(turn);
            turtle.Forward(); // 
         
        }

        void GoalPath()
        {
            while(stack.Count > 1)
            {
                turtle.TurnTo(stack.Pop());
                turtle.Backward();
                maze.AddColor(turtle.Pos, 4);
            }
        }
    }
}
