using System;
using System.Collections;
using Enums;
using Managers;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    private float swipeAngle;

    private bool b_MousePressed;
    public bool b_IsMatched;

    private Gem neighborGem;
    private Vector2Int previousPos;
    
    public int bombBlastRadius = 1;

    public int scoreValue = 10;
    
    #endregion
    
    void Update()
    {
        CheckSwipe();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.currenState ==  BoardState.Move && GameManager.Instance.roundManager.roundTime > 0 )
        {
            if (Camera.main != null) firstTouchPosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            b_MousePressed = true;
        }
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
            boardManager.boardGrid[posIndex.x, posIndex.y] = this;
        }

        if (b_MousePressed && Input.GetMouseButtonUp(0))
        {
            b_MousePressed = false;
            if (GameManager.Instance.currenState == BoardState.Move && GameManager.Instance.roundManager.roundTime > 0)
            {
                if (Camera.main != null) finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateSwipeAngle();
            }
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

        switch (swipeAngle)
        {
            // Right swipe
            case > -45 and < 45 when posIndex.x < boardManager.width - 1:
                neighborGem = boardManager.boardGrid[posIndex.x + 1, posIndex.y];
                neighborGem.posIndex.x--;
                posIndex.x++;
                break;
            // Up swipe
            case > 45 and <= 135 when posIndex.y < boardManager.height - 1:
                neighborGem = boardManager.boardGrid[posIndex.x, posIndex.y + 1];
                neighborGem.posIndex.y--;
                posIndex.y++;
                break;
            // Down swipe
            case >= -135 and < -45 when posIndex.y > 0:
                neighborGem = boardManager.boardGrid[posIndex.x, posIndex.y - 1];
                neighborGem.posIndex.y++;
                posIndex.y--;
                break;
            // left swipe
            case > 135:
            case <= -135 when posIndex.x > 0:
                neighborGem = boardManager.boardGrid[posIndex.x - 1, posIndex.y];
                neighborGem.posIndex.x++;
                posIndex.x--;
                break;
        }
        // Ensuring gems are on correct position.
        boardManager.boardGrid[posIndex.x, posIndex.y] = this;
        boardManager.boardGrid[neighborGem.posIndex.x, neighborGem.posIndex.y] = neighborGem;

        StartCoroutine(CheckMove());
    }

    private IEnumerator CheckMove()     // Check if there's a match with move if not then return back to it's position.
    {
        GameManager.Instance.currenState = BoardState.Wait;
        
        yield return new WaitForSeconds(.5f);
        GameManager.Instance.matchFinder.FindAllGemMatches();

        if (neighborGem != null)
        {
            if (!b_IsMatched && !neighborGem.b_IsMatched)
            {
                neighborGem.posIndex = posIndex;
                posIndex = previousPos;
                
                boardManager.boardGrid[posIndex.x, posIndex.y] = this;
                boardManager.boardGrid[neighborGem.posIndex.x, neighborGem.posIndex.y] = neighborGem;

                yield return new WaitForSeconds(.5f);
                GameManager.Instance.currenState = BoardState.Move;
            }
            else
            {
                GameManager.Instance.DestroyMatches();
            }
        }
    }
}
