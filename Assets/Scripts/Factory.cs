using Managers;
using Singleton;
using UnityEngine;

public class Factory : MonoGenericSingleton<Factory>
{
    // public static readonly List<Gem> gemsInScene = new List<Gem>();
    public void SpawnTile(GameObject tileBgPrefab, Vector2 pos, BoardManager board)
    {
        GameObject bgTile = Instantiate(tileBgPrefab, pos, Quaternion.identity);
        bgTile.transform.parent = board.transform;
        bgTile.name = "BG Tile (" + pos.x + "," + pos.y + ")";
    }
    
    public void SpawnGem(Vector2Int pos, Gem gemToSpawn, BoardManager board)   
    {
        if (Random.Range(0f, 100f) < board.bombChance ) //GameManager.Instance.bombChance)
        {
            gemToSpawn = board.bomb;  //GameManager.Instance.bomb;
        }
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y + board.height, 0), Quaternion.identity);
        // gemsInScene.Add(gem);
        
        gem.transform.parent = board.transform;
        gem.name = "Gem (" + pos.x + "," + pos.y + ")";
        board.boardGrid[pos.x, pos.y] = gem;    // Storing it in 2D array so that i can access it.

        gem.SetupGem(pos, board);
    }
}
        
