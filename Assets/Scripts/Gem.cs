using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{   
    
    // [HideInInspector]
    public Vector2Int posIndex;
    // [HideInInspector]
    public Board board;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    private bool b_MousePressed;
    private float swipeAngle = 0;

    private Gem neighborGem;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (b_MousePressed && Input.GetMouseButtonUp(0))
        {
            b_MousePressed = false;
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    private void OnMouseDown()
    {
        if (Camera.main != null) firstTouchPosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        b_MousePressed = true;
    }
    
    public void SetupGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }
    
    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
                                 finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / MathF.PI;
        Debug.Log(swipeAngle);

        if (Vector3.Distance(firstTouchPosition, finalTouchPosition ) > 0.5f)
        {
            MovePieces();
        }
    }

    private void MovePieces()
    {   // Right
        if (swipeAngle > -45 && swipeAngle < 45 && posIndex.x < board.width - 1)
        {
            neighborGem = board.allGems[posIndex.x + 1, posIndex.y];
            neighborGem.posIndex.x--;
            posIndex.x++;

        }
        // Up
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1)
        {
            neighborGem = board.allGems[posIndex.x, posIndex.y + 1];
            neighborGem.posIndex.y--;
            posIndex.y++;

        }
        // Down
        else if (swipeAngle > -135 && swipeAngle < -45 && posIndex.y > 0)
        {
            neighborGem = board.allGems[posIndex.x, posIndex.y - 1];
            neighborGem.posIndex.y++;
            posIndex.y--;
        }
        // left
        else if (swipeAngle > 135 || swipeAngle <= -135 && posIndex.x > 0)
        {
            neighborGem = board.allGems[posIndex.x - 1, posIndex.y];
            neighborGem.posIndex.x++;
            posIndex.x--;

        }
    }
}
