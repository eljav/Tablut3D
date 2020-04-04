using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rey : Token
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool [9,9];
        Token t;
        int i;

        // arriba
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 9)
                break;
            t = BoardManager.Instance.Tokens[CurrentX, i];
            if (t == null)
                r[CurrentX, i] = true;
            else
            {
                if (t.isWhite != isWhite)
                    r[CurrentX, i] = true;
                break;
            }
        }

        // abajo
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
                break;
            t = BoardManager.Instance.Tokens[CurrentX, i];
            if (t == null)
                r[CurrentX, i] = true;
            else
            {
                if (t.isWhite != isWhite)
                    r[CurrentX, i] = true;
                break;
            }
        }
        
        // izquierda
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
                break;
            t = BoardManager.Instance.Tokens[i, CurrentY];
            if (t == null)
                r[i, CurrentY] = true;
            else
            {
                if (t.isWhite != isWhite)
                    r[i, CurrentY] = true;
                break;
            }
        }

        // derecha
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 9)
                break;
            t = BoardManager.Instance.Tokens[i, CurrentY];
            if (t == null)
                r[i, CurrentY] = true;
            else
            {
                if (!t.isWhite)
                    r[i, CurrentY] = true;
                break;
            }
        }
        return r;
    }
}
