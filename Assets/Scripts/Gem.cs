using System;
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
    private float swipeAngle;

    private Gem neighborGem;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.gemTransitionSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.allGems[posIndex.x, posIndex.y] = this;
        }
        
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
    {
        // Right swipe
        if (swipeAngle is > -45 and < 45 && posIndex.x < board.width - 1)
        {
            neighborGem = board.allGems[posIndex.x + 1, posIndex.y];
            neighborGem.posIndex.x--;
            posIndex.x++;

        }
        // Up swipe
        else if (swipeAngle is > 45 and <= 135 && posIndex.y < board.height - 1)
        {
            neighborGem = board.allGems[posIndex.x, posIndex.y + 1];
            neighborGem.posIndex.y--;
            posIndex.y++;

        }
        // Down swipe
        else if (swipeAngle is >= -135 and < -45 && posIndex.y > 0)
        {
            neighborGem = board.allGems[posIndex.x, posIndex.y - 1];
            neighborGem.posIndex.y++;
            posIndex.y--;
        }
        // left swipe
        else if (swipeAngle > 135 || swipeAngle <= -135 && posIndex.x > 0)
        {
            neighborGem = board.allGems[posIndex.x - 1, posIndex.y];
            neighborGem.posIndex.x++;
            posIndex.x--;
        }

        board.allGems[posIndex.x, posIndex.y] = this;
        board.allGems[neighborGem.posIndex.x, neighborGem.posIndex.y] = neighborGem;
    }
}
