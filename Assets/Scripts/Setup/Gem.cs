using System;
using System.Collections;
using Enums;
using Managers;
using UnityEngine;

public class Gem : MonoBehaviour
{
    #region VARIABLES & REFRENCES

    [HideInInspector] public Vector2Int posIndex;
    [Tooltip("Select type of the Gem.")] 
    public GemType type;
    
    [Tooltip("Add particle effect which you wants to play when this type of gem get destroyed.")] 
    public GameObject destroyEffect;
    
    [HideInInspector] public BoardManager boardManager;
    
    private Vector2 m_firstTouchPosition;
    private Vector2 m_finalTouchPosition;
    private float m_swipeAngle;

    private bool b_MousePressed;
    public bool b_IsMatched;

    private Gem m_neighborGem;
    private Vector2Int m_previousPos;

    public int scoreValue = 10;
    
    #endregion

    public static event Action<Gem> GemDestroyed;

    private void OnDisable()
    {
        GemDestroyed?.Invoke(this);
    }

    void Update()
    {
        CheckSwipe();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.currenState != BoardState.Move || !(UIManager.Instance.roundTime > 0)) return;
        if (Camera.main != null) m_firstTouchPosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        b_MousePressed = true;
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

        if (!b_MousePressed || !Input.GetMouseButtonUp(0)) return;
        b_MousePressed = false;
        if (GameManager.Instance.currenState != BoardState.Move || !(UIManager.Instance.roundTime > 0)) return;
        if (Camera.main != null) m_finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateSwipeAngle();
    }

    public void SetupGem(Vector2Int pos, BoardManager theBoardManager)
    {
        posIndex = pos;
        boardManager = theBoardManager;
    }
    
    private void CalculateSwipeAngle()
    {
        m_swipeAngle = Mathf.Atan2(m_finalTouchPosition.y - m_firstTouchPosition.y,
                                 m_finalTouchPosition.x - m_firstTouchPosition.x);
        m_swipeAngle = m_swipeAngle * 180 / MathF.PI;
        GameLogManager.CustomLog(m_swipeAngle);

        if (Vector3.Distance(m_firstTouchPosition, m_finalTouchPosition ) > 0.5f)
        {
            MovePieces();
        }
    }
    
    private void MovePieces()
    {
        m_previousPos = posIndex;

        switch (m_swipeAngle)
        {
            // Right swipe
            case > -45 and < 45 when posIndex.x < boardManager.width - 1:
                m_neighborGem = boardManager.boardGrid[posIndex.x + 1, posIndex.y];
                m_neighborGem.posIndex.x--;
                posIndex.x++;
                break;
            // Up swipe
            case > 45 and <= 135 when posIndex.y < boardManager.height - 1:
                m_neighborGem = boardManager.boardGrid[posIndex.x, posIndex.y + 1];
                m_neighborGem.posIndex.y--;
                posIndex.y++;
                break;
            // Down swipe
            case >= -135 and < -45 when posIndex.y > 0:
                m_neighborGem = boardManager.boardGrid[posIndex.x, posIndex.y - 1];
                m_neighborGem.posIndex.y++;
                posIndex.y--;
                break;
            // left swipe
            case > 135:
            case <= -135 when posIndex.x > 0:
                m_neighborGem = boardManager.boardGrid[posIndex.x - 1, posIndex.y];
                m_neighborGem.posIndex.x++;
                posIndex.x--;
                break;
        }
        // Ensuring gems are on correct position.
        boardManager.boardGrid[posIndex.x, posIndex.y] = this;
        boardManager.boardGrid[m_neighborGem.posIndex.x, m_neighborGem.posIndex.y] = m_neighborGem;

        StartCoroutine(CheckMove());
    }

    private IEnumerator CheckMove()     // Check if there's a match with move if not then return back to it's position.
    {
        GameManager.Instance.currenState = BoardState.Wait;
        
        yield return new WaitForSeconds(.5f);
        GameManager.Instance.matchFinder.FindAllGemMatches();

        if (m_neighborGem != null)
        {
            if (!b_IsMatched && !m_neighborGem.b_IsMatched)
            {
                m_neighborGem.posIndex = posIndex;
                posIndex = m_previousPos;
                
                boardManager.boardGrid[posIndex.x, posIndex.y] = this;
                boardManager.boardGrid[m_neighborGem.posIndex.x, m_neighborGem.posIndex.y] = m_neighborGem;

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
