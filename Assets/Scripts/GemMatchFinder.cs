
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

public class GemMatchFinder : MonoBehaviour
{
    public BoardManager boardManager;

    public List<Gem> currentMatches = new List<Gem>();
    
    public void FindAllGemMatches()
    {
        currentMatches.Clear();
        
        for (int i = 0; i < boardManager.width; i++)
        {
            for (int j = 0; j < boardManager.height; j++)
            {
                Gem currentGem = boardManager.allGems[i, j];
                
                
                if (currentGem != null)
                {
                    // Horizontal match
                    if (i > 0 && i < boardManager.width - 1)
                    {
                        Gem leftGem = boardManager.allGems[i - 1, j];
                        Gem rightGem = boardManager.allGems[i + 1, j];
                        if (leftGem != null && rightGem != null)
                        {
                            if (leftGem.type == currentGem.type && rightGem.type == currentGem.type)
                            {
                                currentGem.b_IsMatched = true;
                                leftGem.b_IsMatched = true;
                                rightGem.b_IsMatched = true;
                                
                                currentMatches.Add(currentGem);
                                currentMatches.Add(rightGem);
                                currentMatches.Add(leftGem);
                            }
                        }
                    }
                    
                    // Vertical match
                    if (j > 0 && j < boardManager.height - 1)
                    {
                        Gem aboveGem = boardManager.allGems[i, j +1];
                        Gem belowGem = boardManager.allGems[i, j -1];
                        if (aboveGem != null && belowGem != null)
                        {
                            if (aboveGem.type == currentGem.type && belowGem.type == currentGem.type)
                            {
                                currentGem.b_IsMatched = true;
                                aboveGem.b_IsMatched = true;
                                belowGem.b_IsMatched = true;
                                
                                currentMatches.Add(currentGem);
                                currentMatches.Add(aboveGem);
                                currentMatches.Add(belowGem);
                            }
                        }
                    }
                }
                
            }
        }
        // ensuring list only contains distinct values.
        if (currentMatches.Count > 0)
        {
            currentMatches = currentMatches.Distinct().ToList();
        }

        CheckForBombs();
    }

    private void CheckForBombs()
    {
        for (int i=0; i < currentMatches.Count; i++)
        {
            Gem gem = currentMatches[i];
            int x = gem.posIndex.x;
            int y = gem.posIndex.y;
            
            // Left Check
            if (gem.posIndex.x > 0)
            {
                if (boardManager.allGems[x - 1, y] != null)
                {
                    if (boardManager.allGems[x - 1, y].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x - 1, y), boardManager.allGems[x-1, y]);
                    }
                }
            }
            // Right Check
            if (gem.posIndex.x < boardManager.width - 1)
            {
                if (boardManager.allGems[x + 1, y] != null)
                {
                    if (boardManager.allGems[x + 1, y].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1, y), boardManager.allGems[x + 1, y]);
                    }
                }
            }
            // Down Check
            if (gem.posIndex.y > 0)
            {
                if (boardManager.allGems[x, y - 1] != null)
                {
                    if (boardManager.allGems[x, y - 1].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y - 1), boardManager.allGems[x, y - 1]);
                    }
                }
            }
            // Up Check
            if (gem.posIndex.y < boardManager.height - 1)
            {
                if (boardManager.allGems[x, y + 1] != null)
                {
                    if (boardManager.allGems[x, y + 1].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x ,y + 1), boardManager.allGems[x, y + 1 ]);
                    }
                }
            }
        }
    }

    private void MarkBombArea(Vector2Int bombPos, Gem theBomb)
    {
        
        for (int x = bombPos.x - theBomb.bombBlastRadius; x <= bombPos.x + theBomb.bombBlastRadius; x++)
        {
            for (int y = bombPos.y - theBomb.bombBlastRadius; y <= bombPos.y + theBomb.bombBlastRadius; y++)
            {
                if (x >= 0 && x < boardManager.width && y >= 0 && y < boardManager.height)
                {
                    if (boardManager.allGems[x, y] != null)
                    {
                        boardManager.allGems[x, y].b_IsMatched = true;
                        currentMatches.Add(boardManager.allGems[x, y]);
                    }
                }
            }
        }

        currentMatches = currentMatches.Distinct().ToList();
    }
}
