using System.Collections;

using System.Linq;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour
{
    #region variables
    [Header("Board Properties")]
    
    [Tooltip("The Width of the board.")] 
    public int width;
    
    [Tooltip("The Height of the board.")] 
    public int height;
    
    [Tooltip("The Prefab of background tile.")] 
    public GameObject tileBgPrefab;
    
    [Tooltip("Reference of Gem match finder.")]
    public GemMatchFinder matchFind;
    
    [Header("Gem Properties")]
    
    [Tooltip("The transition speed in which gems move from one place to another.")] 
    public float gemTransitionSpeed;
    public Gem bomb;
    
    [Tooltip("The number of spawning chances of bomb.")] 
    public float bombChance = 2f;
    
    [Tooltip("Add different different gems here which you wants to have in the game.")]
    public Gem[] gems;
    public Gem[,] allGems;
     
    [HideInInspector] public BoardState currenState = BoardState.Move;
    #endregion
    
    
    void Start()
    {
        allGems = new Gem[width, height];
        Setup();
    }

    private void Setup()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 pos = new Vector2(i, j);
                GameObject bgTile = Instantiate(tileBgPrefab, pos, Quaternion.identity);
                bgTile.transform.parent = transform;
                bgTile.name = "BG Tile (" + i + "," + j + ")";
                
                int gemToUse = Random.Range(0, gems.Length);

                int iterations = 0; // By this will Make sure we will not end up in a crashing of game due to excessive iterations.
                while (MatchesInBeginning(new Vector2Int(i,j), gems[gemToUse]) && iterations < 100)  
                {
                    gemToUse = Random.Range(0, gems.Length);
                    iterations++;
                }

                SpawnGem(new Vector2Int(i,j), gems[gemToUse]);
            }
        }
    }

    private void SpawnGem(Vector2Int pos, Gem gemToSpawn)   // using vec2Int becoz we need a whole value.
    {
        if (Random.Range(0f, 100f) < bombChance)
        {
            gemToSpawn = bomb;
        }
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y + height, 0), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = "Gem (" + pos.x + "," + pos.y + ")";
        allGems[pos.x, pos.y] = gem;    // Storing it in 2D array so that i can access it.

        gem.SetupGem(pos, this);
    }

    private bool MatchesInBeginning(Vector2Int posToCheck, Gem gemToCheck)    // Ensures that there's no match exist in the beginning of game.
    {
        if (posToCheck.x > 1)
        {
            if (allGems[posToCheck.x - 1, posToCheck.y].type == gemToCheck.type && allGems[posToCheck.x - 2, posToCheck.y].type == gemToCheck.type)
            {
                return true;
            }
        }
        
        if (posToCheck.y > 1)
        {
            if (allGems[posToCheck.x, posToCheck.y - 1].type == gemToCheck.type && allGems[posToCheck.x, posToCheck.y-2].type == gemToCheck.type)
            {
                return true;
            }
        }

        return false;
    }

    private void DestroyMatchesAt(Vector2Int pos)
    {
        if (allGems[pos.x, pos.y] != null)
        {
            if (allGems[pos.x, pos.y].b_IsMatched)
            {
                Instantiate(allGems[pos.x, pos.y].destroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);
                Destroy(allGems[pos.x, pos.y].gameObject);
                allGems[pos.x, pos.y] = null;
            }
        }
    }

    public void DestroyMatches()
    {
        foreach (var gem in matchFind.currentMatches.Where(gem => gem != null))
        {
            DestroyMatchesAt(gem.posIndex);
        }

        StartCoroutine(RowFallDown());
    }

    private IEnumerator RowFallDown()
    {
        yield return new WaitForSeconds(.2f);
        int emptySlotCounter = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] == null)
                {
                    emptySlotCounter++;
                }
                else if (emptySlotCounter > 0)
                {
                    allGems[i, j].posIndex.y -= emptySlotCounter;
                    allGems[i, j - emptySlotCounter] = allGems[i, j];
                    allGems[i, j] = null;
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
        matchFind.FindAllGemMatches();
        
        if (matchFind.currentMatches.Count > 0)     // Destroying new matches after refilling.
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            currenState = BoardState.Move;
        }
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] != null) continue;
                
                int gemToUse = Random.Range(0, gems.Length);
                SpawnGem(new Vector2Int(i,j), gems[ gemToUse] );
            }
        }
        // CheckMisplacedGems();
    }

    // private void CheckMisplacedGems()
    // {
    //     List<Gem> gemsInScene = new List<Gem>();
    //     gemsInScene.AddRange(FindObjectsOfType<Gem>());
    //     
    //     for (int i = 0; i < width; i++)
    //     {
    //         for (int j = 0; j < height; j++)
    //         {
    //             if (gemsInScene.Contains(allGems[i, j]))
    //             {
    //                 gemsInScene.Remove(allGems[i, j]);
    //             }
    //         }
    //     }
    //
    //     foreach (Gem g in gemsInScene)
    //     {
    //         Destroy(g.gameObject);
    //     }
    // }
}