using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Board : MonoBehaviour
{
    public int width;
    public int height;

    public GameObject tileBgPrefab;
    public Gem[] gems;
    void Start()
    {
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
                SpawnGem(new Vector2Int(i,j), gems[gemToUse]);

            }
        }
    }

    private void SpawnGem(Vector2Int pos, Gem gemToSpawn)   // using vec2Int becoz i want a whole value.
    {
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        gem.name = "Gem (" + pos.x + "," + pos.y + ")";
    }
}
