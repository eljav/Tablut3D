using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Token : MonoBehaviour
{
    public int CurrentX{set;get;}
    public int CurrentY{set;get;}
    public bool isWhite;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }
    public virtual bool[,] PossibleMove()
    {
        bool[,] r = new bool[9,9];
        Token t, t2;
        // arriba
        if (CurrentY != 8)
        {
            t = BoardManager.Instance.Tokens[CurrentX, CurrentY + 1];
            if (t == null)
            {
                r[CurrentX, CurrentY + 1] = true;
            }
        }

        // abajo
        if (CurrentY != 0)
        {
            t = BoardManager.Instance.Tokens[CurrentX, CurrentY - 1];
            if (t == null)
            {
                r[CurrentX, CurrentY - 1] = true;
            }
        }
        
        // izquierda
        if (CurrentX != 0)
        {
            t = BoardManager.Instance.Tokens[CurrentX - 1, CurrentY];
            if (t == null)
            {
                r[CurrentX - 1, CurrentY] = true;
            }
        }

        // derecha
        if (CurrentX != 8)
        {
            t = BoardManager.Instance.Tokens[CurrentX + 1, CurrentY];
            if (t == null)
            {
                r[CurrentX + 1, CurrentY] = true;
            }
        }
        return r;
        

    }
}
