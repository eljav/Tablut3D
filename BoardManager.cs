using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance{set;get;}
    private bool[,] allowedMoves{set;get;}

    public Token[,] Tokens{set;get;}
    private Token selectedToken;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    // private Quaternion orientation = Quaternion.Euler(0,180,0); // para rotar las piezas reemplazar Quaternion.identity en SpawnToken()

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> tokenPrefabs;
    private List<GameObject> activeToken;

    public bool IsWhiteTurn = false;

    private void Start()
    {
        Instance = this;
        SpawnAllTokens();
    }

    private void Update()
    {
        UpdateSelection();
        DrawBoard();
        
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedToken == null)
                {
                    SelectToken(selectionX, selectionY);
                }
                else
                {
                    MoveToken(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectToken(int x, int y)
    {
        if (Tokens[x,y] == null)
            return;
        if (Tokens[x,y].isWhite != IsWhiteTurn)
            return;
        allowedMoves = Tokens[x,y].PossibleMove();
        selectedToken = Tokens[x,y];
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveToken(int x, int y)
    {
        if (allowedMoves[x,y])
        {
            // destruir pieza
            Token t = Tokens[x,y];
            if (t != null && t.isWhite != IsWhiteTurn)
            {
                activeToken.Remove(t.gameObject);
                Destroy(t.gameObject);
            }

            Tokens[selectedToken.CurrentX, selectedToken.CurrentY] = null;
            selectedToken.transform.position = GetTileCenter(x,y);
            selectedToken.SetPosition(x,y);
            Tokens[x,y] = selectedToken;
            IsWhiteTurn = !IsWhiteTurn;
        }
        selectedToken = null;
        BoardHighlights.Instance.HideHighlights();
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Plane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }
    
    private void SpawnToken(int index, int x, int y)
    {
        GameObject go = Instantiate(tokenPrefabs[index], GetTileCenter(x,y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        Tokens[x,y] = go.GetComponent<Token>();
        Tokens[x,y].SetPosition(x,y);
        activeToken.Add(go);
    }

    private void SpawnAllTokens()
    {
        activeToken = new List<GameObject>();
        Tokens = new Token[9,9];

        SpawnToken(0,4,4); // rey
        SpawnToken(1,4,6); // blancos
        SpawnToken(1,4,5);
        SpawnToken(1,4,3);
        SpawnToken(1,4,2);
        SpawnToken(1,2,4);
        SpawnToken(1,3,4);
        SpawnToken(1,5,4);
        SpawnToken(1,6,4);
        for (int i = 3; i <= 5; i++)    // negros
        {
            SpawnToken(2,i,0);
            SpawnToken(2,i,8);
            SpawnToken(2,0,i);
            SpawnToken(2,8,i);
        }
        SpawnToken(2,1,4);
        SpawnToken(2,4,1);
        SpawnToken(2,4,7);
        SpawnToken(2,7,4);
        
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void DrawBoard()
    {
        Vector3 widthLine = Vector3.right * 9;
        Vector3 heightLine = Vector3.forward * 9;
        for (int i = 0; i <= 9; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 9; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }
        // Draw the selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1)
            );
            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1)
            );
        }
    }
}
