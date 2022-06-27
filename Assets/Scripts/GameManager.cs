using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Singleton;
using UnityEngine;

public class GameManager : MonoGenericSingleton<GameManager>
{
    public BoardManager board;
    public RoundManager roundManager;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ShuffleBoard();
        }
    }

    private void DestroyMatchesAt(Vector2Int pos)
    {
        if (board.boardGrid[pos.x, pos.y] != null )
        {
            if (board.boardGrid[pos.x, pos.y].b_IsMatched)
            {
                Instantiate(board.boardGrid[pos.x, pos.y].destroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);
                Destroy(board.boardGrid[pos.x, pos.y].gameObject);
                board.boardGrid[pos.x, pos.y] = null;
            }
        }
    }
    
    public void DestroyMatches()
    {
        foreach (var gem in MatchFinder.Instance.currentMatches.Where(gem => gem != null))
        {
            AddScore(gem);
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
        MatchFinder.Instance.FindAllGemMatches();
        
        if (MatchFinder.Instance.currentMatches.Count > 0)     // Destroying new matches after refilling.
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            board.currenState = BoardState.Move;
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
        // CheckMisplacedGems();
    }

    /*private void CheckMisplacedGems()
    {
        // List<Gem> gemsInScene = new List<Gem>();
        // gemsInScene.AddRange(FindObjectsOfType<Gem>());

        
        
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                foreach (Gem gem in Factory.gemsInScene)
                {
                    if (gem == board.boardGrid[i, j])
                    {
                        Factory.gemsInScene.Remove(board.boardGrid[i, j]);
                        // Destroy(gem.gameObject);
                    }
                }
                
                // if (gemsInScene.Contains(board.boardGrid[i, j]))
                // {
                //     gemsInScene.Remove(board.boardGrid[i, j]);
                // }
            }
        }
        
        foreach (Gem g in Factory.gemsInScene)
        {
            Destroy(g.gameObject);
        }
    }*/
    
    private void ShuffleBoard()
    {
        if (board.currenState != BoardState.Wait)
        {
            board.currenState = BoardState.Wait;
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

    private void AddScore(Gem gemToCheck)
    {
        roundManager.currentScore += gemToCheck.scoreValue;
    }
}