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

    [Header("Gem Properties")]
    [Tooltip("The transition speed in which gems move from one place to another.")] 
    public float gemTransitionSpeed;
    public Gem bomb;
    
    [Tooltip("The number of spawning chances of bomb.")] 
    public float bombChance = 2f;
    
    [Tooltip("Add different different gems here which you wants to have in the game.")]
    public Gem[] gems;
    public Gem[,] boardGrid;
     
    [HideInInspector] public BoardState currenState = BoardState.Move;

    
    #endregion

    private void Start()
    {
        boardGrid = new Gem[width, height];
        SetupBoard();
    }

    private void SetupBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Factory.Instance.SpawnTile(tileBgPrefab, new Vector2Int(i, j), this);

                int gemToUse = GetUniqueGem(i, j);

                Factory.Instance.SpawnGem(new Vector2Int(i,j), gems[gemToUse], this);
            }
        }
    }

    private int GetUniqueGem(int x, int y)
    {
        int gemToUse = Random.Range(0, gems.Length);
        
        int iterations = 0; // By this will Make sure we will not end up in a crashing of game due to excessive iterations.
        
        while (IsGemAlreadyExist(new Vector2Int(x, y), gems[gemToUse]) && iterations < 100)
        {
            gemToUse = Random.Range(0, gems.Length);
            iterations++;
        }
        return gemToUse;
    }

    // Ensures that there's no match exist in the beginning of game.
    public bool IsGemAlreadyExist(Vector2Int posToCheck, Gem gemToCheck)    
    {
        if (posToCheck.x > 1)
        {
            if (boardGrid[posToCheck.x - 1, posToCheck.y].type == gemToCheck.type && boardGrid[posToCheck.x - 2, posToCheck.y].type == gemToCheck.type)
            {
                return true;
            }
        }
        
        if (posToCheck.y > 1)
        {
            if (boardGrid[posToCheck.x, posToCheck.y - 1].type == gemToCheck.type && boardGrid[posToCheck.x, posToCheck.y-2].type == gemToCheck.type)
            {
                return true;
            }
        }

        return false;
    }
}
