using System;
using System.Collections;
using Enums;
using UnityEngine;

public class Gem : MonoBehaviour
{
    #region Variables

    [HideInInspector] public Vector2Int posIndex;
    [Tooltip("Select type of the Gem.")] 
    public GemType type;
    
    [Tooltip("Add particle effect which you wants to play when this type of gem get destroyed.")] 
    public GameObject destroyEffect;
    
    [HideInInspector] public BoardManager boardManager;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    private bool b_MousePressed;
    private float swipeAngle;

    private Gem neighborGem;

    public bool b_IsMatched;
    private Vector2Int previousPos;

    public int bombBlastRadius = 1;

    #endregion
    
    void Update()
    {
        CheckSwipe();
    }

    private void CheckSwipe()
    {
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position =Vector2.Lerp(transform.position, posIndex,
                                                    boardManager.gemTransitionSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            boardManager.allGems[posIndex.x, posIndex.y] = this;
        }

        if (b_MousePressed && Input.GetMouseButtonUp(0))
        {
            b_MousePressed = false;
            if (boardManager.currenState == BoardState.Move)
            {
                finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateSwipeAngle();
            }
        }
    }

    private void OnMouseDown()
    {
        if (boardManager.currenState ==  BoardState.Move)
        {
            if (Camera.main != null) firstTouchPosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            b_MousePressed = true;
        }
    }
    
    public void SetupGem(Vector2Int pos, BoardManager theBoardManager)
    {
        posIndex = pos;
        boardManager = theBoardManager;
    }
    
    private void CalculateSwipeAngle()
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
        previousPos = posIndex;
        // Right swipe
        if (swipeAngle is > -45 and < 45 && posIndex.x < boardManager.width - 1)
        {
            neighborGem = boardManager.allGems[posIndex.x + 1, posIndex.y];
            neighborGem.posIndex.x--;
            posIndex.x++;

        }
        // Up swipe
        else if (swipeAngle is > 45 and <= 135 && posIndex.y < boardManager.height - 1)
        {
            neighborGem = boardManager.allGems[posIndex.x, posIndex.y + 1];
            neighborGem.posIndex.y--;
            posIndex.y++;

        }
        // Down swipe
        else if (swipeAngle is >= -135 and < -45 && posIndex.y > 0)
        {
            neighborGem = boardManager.allGems[posIndex.x, posIndex.y - 1];
            neighborGem.posIndex.y++;
            posIndex.y--;
        }
        // left swipe
        else if (swipeAngle > 135 || swipeAngle <= -135 && posIndex.x > 0)
        {
            neighborGem = boardManager.allGems[posIndex.x - 1, posIndex.y];
            neighborGem.posIndex.x++;
            posIndex.x--;
        }
        // Ensuring if the gems are on correct position.
        boardManager.allGems[posIndex.x, posIndex.y] = this;
        boardManager.allGems[neighborGem.posIndex.x, neighborGem.posIndex.y] = neighborGem;

        StartCoroutine(CheckMove());
    }

    private IEnumerator CheckMove()     // Check if there's a match with move if not then return back to it's position.
    {
        boardManager.currenState = BoardState.Wait;
        
        yield return new WaitForSeconds(.5f);
        boardManager.matchFind.FindAllGemMatches();

        if (neighborGem != null)
        {
            if (!b_IsMatched && !neighborGem.b_IsMatched)
            {
                neighborGem.posIndex = posIndex;
                posIndex = previousPos;
                
                boardManager.allGems[posIndex.x, posIndex.y] = this;
                boardManager.allGems[neighborGem.posIndex.x, neighborGem.posIndex.y] = neighborGem;

                yield return new WaitForSeconds(.5f);
                boardManager.currenState = BoardState.Move;
            }
            else
            {
                boardManager.DestroyMatches();
            }
        }
    }
}
