
using UnityEngine;
using Random = UnityEngine.Random;


public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public float gemTransitionSpeed;

    public GameObject tileBgPrefab;
    
    public Gem[] gems;
    public Gem[,] allGems;

    public MatchFinder matchFind;

    void Start()
    {
        allGems = new Gem[width, height];
        Setup();
    }

    private void Update()
    {
        matchFind.FindAllGemMatches();
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
                while (MatchesAt(new Vector2Int(i,j), gems[gemToUse]) && iterations < 100)  
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
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = "Gem (" + pos.x + "," + pos.y + ")";
        allGems[pos.x, pos.y] = gem;    // Storing it in 2D array so that i can access it.

        gem.SetupGem(pos, this);
    }

    bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)    // Ensures that there's no match exist in the beginning of game.
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
}
