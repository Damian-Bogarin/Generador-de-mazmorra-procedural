using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class ControllerRandom : MonoBehaviour
    {
        [SerializeField] int maxStep = 3;
        [SerializeField] int maxX = 4;
        [SerializeField] int maxY = 4;
        [SerializeField] int emptyCells = 0;    

        [SerializeField] float period = 0.5f;


        MemTurtle turtle;
        MemMaze maze;
        TileMap tileMap;


        void Start()
        {
            Application.targetFrameRate = 30;
            tileMap = GameObject.Find("TileMap").GetComponent<TileMap>();
            turtle = new MemTurtle();
            turtle.SetClamp(maxX, maxY);

            turtle.forwardDelegate = (pos, lastPost, turn, invTurn) =>
            {
                maze.Add(pos, lastPost, turn, invTurn);
            };

            maze = new MemMaze();
            maze.iteratorDelegate = (x, y, value) =>
            {
                tileMap.AddTile(x, y, value & 0x0F, (value >> 4) & 0x0F); // 10101110 = value & 0x0F filtra 4 ->1110, y  (value >> 4) & 0x0F filtra 1010
            };

            StartCoroutine("WalkCoroutine");

        }


        IEnumerator WalkCoroutine()
        {

            while (true)
            {
                Walk();
                yield return new WaitForSeconds(period);
            }
        }

        void Restart()
        {
            maze.Clear();
            maze.Fill(maxX, maxY);
            maze.AddColor(turtle.Pos, 1); // 1 representa el color rojo!


        }

        void UpdateTileMap()
        {
            tileMap.ClearMesh();
            maze.IterateRect();
            tileMap.UpdateMesh();
        }

        void Walk()
        {
            Restart();
            while(maze.GetEmptyCount() > emptyCells)
            {
                // puede que repita giro: ?? 
                turtle.TurnTo(UnityEngine.Random.Range(0, 4)); //si da 0, no gira y sigue derecho - dos pasos adelante, tipo dos arriba, dos abajo, dos derecha
                // No repetir giro:
                //turtle.AddTurn(UnityEngine.Random.Range(1, 4)); // no puede ser 0 osea ir dos pasos iguales, pero si para atras.
                //Girar siempre:
                //turtle.AddTurn(1 + 2 * UnityEngine.Random.Range(1, 3)); no da 0 ni 3 el randome, Siempre da 3 o 5, lo cual hace que gire siempre.

                turtle.Forward(UnityEngine.Random.Range(1, maxStep + 1)); // Avanza entre 1 a maxStep, se le suma 1 porque random no da el tope.

            }

            maze.AddColor(turtle.Pos, 2);
            UpdateTileMap();
        }
    }
}
