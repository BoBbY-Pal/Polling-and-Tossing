using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public MatchFinder matchFinder;
   
    [HideInInspector] 
    public BoardManager board;

    [HideInInspector] 
    public BoardState currenState = BoardState.Move;
    
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
    
    public BoardManager GetBoardRef()
    {
        return board;
    }
    
    private void DestroyMatchesAt(Vector2Int pos)
    {
        if (board.boardGrid[pos.x, pos.y] != null )
        {
            if (board.boardGrid[pos.x, pos.y].b_IsMatched)
            {
                switch (board.boardGrid[pos.x, pos.y].type)
                {
                    case GemType.Bomb:
                        SoundManager.Instance.Play(SoundTypes.BombExplode);  
                        break;
                    
                    case GemType.Stone:
                        SoundManager.Instance.Play(SoundTypes.StoneBreak);
                        break;
                    
                    default:
                        SoundManager.Instance.Play(SoundTypes.GemBreak);
                        break;
                }
                Instantiate(board.boardGrid[pos.x, pos.y].destroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);
                Destroy(board.boardGrid[pos.x, pos.y].gameObject);
                board.boardGrid[pos.x, pos.y] = null;
            }
        }
    }
    
    public void DestroyMatches()
    {
        foreach (var gem in matchFinder.currentMatches.Where(gem => gem != null))
        {
            // SoundManager.Instance.AddScore(gem);
            DestroyMatchesAt(gem.posIndex);
        }

        StartCoroutine(RowFallDown());
    }

    private IEnumerator RowFallDown()
    {
        yield return new WaitForSeconds(.2f);
        int emptySlotCounter = 0;
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.boardGrid[i, j] == null)
                {
                    emptySlotCounter++;
                }
                else if (emptySlotCounter > 0)
                {
                    board.boardGrid[i, j].posIndex.y -= emptySlotCounter;
                    board.boardGrid[i, j - emptySlotCounter] = board.boardGrid[i, j];
                    board.boardGrid[i, j] = null;
                }
            }

            emptySlotCounter = 0;
        }
        
        StartCoroutine(FillBoard());
    }

    private IEnumerator FillBoard()
    {
        yield return new WaitForSeconds(.5f);
        RefillBoard();
        
        yield return new WaitForSeconds(.5f);
        matchFinder.FindAllGemMatches();
        
        if (matchFinder.currentMatches.Count > 0)     // Destroying new matches after refilling.
        {
            ScoreManager.Instance.scoreMultiplierCount++;
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        else
        {
            ScoreManager.Instance.scoreMultiplierCount = 0;
            yield return new WaitForSeconds(.5f);
            currenState = BoardState.Move;
        }
    }

    private void RefillBoard()
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.boardGrid[i, j] != null) continue;
                
                int gemToUse = Random.Range(0, board.gems.Length);
                Factory.Instance.SpawnGem(new Vector2Int(i,j), board.gems[ gemToUse], board);
            }
        }
    }

    public void ShuffleBoard()
    {
        if (currenState != BoardState.Wait)
        {
            currenState = BoardState.Wait;
            List<Gem> gemsFromBoard = new List<Gem>();
            
            for (int x = 0; x < board.width; x++)
            {
                for (int y = 0; y < board.height; y++)
                {
                    gemsFromBoard.Add(board.boardGrid[x, y]);
                    board.boardGrid[x, y] = null;
                }
            }
            
            for (int x = 0; x < board.width; x++)
            {
                for (int y = 0; y < board.height; y++)
                {
                    int gemToUse = Random.Range(0, gemsFromBoard.Count);

                    int iterations = 0; 
                    while (board.IsGemAlreadyExist(new Vector2Int(x,y), gemsFromBoard[gemToUse]) && iterations < 100 && gemsFromBoard.Count > 1)  
                    {
                        gemToUse = Random.Range(0, gemsFromBoard.Count);
                        iterations++;
                    }

                    gemsFromBoard[gemToUse].SetupGem( new Vector2Int(x,y), board);
                    board.boardGrid[x,y] = gemsFromBoard[gemToUse];
                    gemsFromBoard.RemoveAt(gemToUse);
                }
            }
            StartCoroutine(FillBoard());
        }
    }

   
}