using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void VoidFuncVoid();
public delegate void VoidFuncInt(int turn);
public delegate void VoidFunc4Param(Vector3 pos, Vector3 lastPost, int turn, int invTurn);

public class MemTurtle 
{
    Vector3 pos = Vector3.zero;
    Vector3 lastPost = Vector3.zero;

    int turn = 0;
    int invTurn = 0;

    Vector3[] dirs =
    {
        Vector3.up,
        Vector3.right,
        Vector3.down,
        Vector3.left
    };

    float clampX = 0;
    float clampY = 0;

    public Vector3 Pos { get { return pos; } }
    public Vector3 LastPost { get { return lastPost; } }
    public Vector3 Dir { get { return dirs[turn]; } }

    public int Turn { get { return turn; } }
    public int InvTurn { get { return invTurn; } }

    public VoidFunc4Param forwardDelegate;
    public VoidFunc4Param backwardDelegate;
    public VoidFuncInt turnDelegate;

    public MemTurtle(float clampX = 3, float clampY = 3)
    {

        forwardDelegate = (x, y, z, w) => { };
        backwardDelegate = (x, y, z, w) => { };
        turnDelegate = (i) => {};

        invTurn = (turn + 2) & 0x03;

        this.clampX = clampX;
        this.clampX = clampY;
    }

    public void Forward() {
        lastPost = pos;
        pos += dirs[turn];

        //Restringir la "matriz" de la tortuga:

        pos.x = Mathf.Clamp(pos.x, 0, clampX - 1);
        pos.y = Mathf.Clamp(pos.y, 0,clampY - 1);

        if(lastPost != pos) // la posicion de la tortu cambio
        {
            forwardDelegate(pos, lastPost, turn, invTurn);
        }
    }

    public void Backward() {
        lastPost = pos;
        pos += dirs[invTurn];

        //Restringir la "matriz" de la tortuga:
        pos.x = Mathf.Clamp(pos.x, 0, clampX - 1);
        pos.y = Mathf.Clamp(pos.y, 0, clampY - 1);

        if (lastPost != pos) { 
            backwardDelegate(pos, lastPost, turn, invTurn); 
        }
    }


    public void Forward(int fwd) {
        
        for (int i = 1; i <= fwd; i++)
        {
            Forward();
        }
    }


    public void TurnTo(int newTurn)
    {
        turn = newTurn;
        turn &= 0x03;
        invTurn = (turn + 2) & 0x03;
        turnDelegate(turn);
    }

    public void AddTurn(int newTurn) { 
    
        turn += newTurn; // agrega el giro
        turn &= 0x03;
        invTurn = (turn + 2) & 0x03;
        turnDelegate(turn);
    }

    public void SetPos(float x, float y)
    {
        pos.x = x;
        pos.y = y;
        lastPost = pos;

        TurnTo(0);

    }

    public void SetClamp(float clampX, float clampY) { // revisar si son float :D

        this.clampX = clampX;
        this.clampY = clampY;


    }
}
