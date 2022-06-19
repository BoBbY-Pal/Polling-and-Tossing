
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

public class GemMatchFinder : MonoBehaviour
{
    public BoardManager board;

    public List<Gem> currentMatches = new List<Gem>();
    
    public void FindAllGemMatches()
    {
        currentMatches.Clear();
        
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                Gem currentGem = board.allGems[i, j];
                
                
                if (currentGem != null)
                {
                    // Horizontal match
                    if (i > 0 && i < board.width - 1)
                    {
                        Gem leftGem = board.allGems[i - 1, j];
                        Gem rightGem = board.allGems[i + 1, j];
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
                    if (j > 0 && j < board.height - 1)
                    {
                        Gem aboveGem = board.allGems[i, j + 1];
                        Gem belowGem = board.allGems[i, j - 1];
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
                if (board.allGems[x - 1, y] != null)
                {
                    if (board.allGems[x - 1, y].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x - 1, y), board.allGems[x-1, y]);
                    }
                }
            }
            // Right Check
            if (gem.posIndex.x < board.width - 1)
            {
                if (board.allGems[x + 1, y] != null)
                {
                    if (board.allGems[x + 1, y].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1, y), board.allGems[x + 1, y]);
                    }
                }
            }
            // Down Check
            if (gem.posIndex.y > 0)
            {
                if (board.allGems[x, y - 1] != null)
                {
                    if (board.allGems[x, y - 1].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y - 1), board.allGems[x, y - 1]);
                    }
                }
            }
            // Up Check
            if (gem.posIndex.y < board.height - 1)
            {
                if (board.allGems[x, y + 1] != null)
                {
                    if (board.allGems[x, y + 1].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x ,y + 1), board.allGems[x, y + 1 ]);
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
                if (x >= 0 && x < board.width && y >= 0 && y < board.height)
                {
                    if (board.allGems[x, y] != null)
                    {
                        board.allGems[x, y].b_IsMatched = true;
                        currentMatches.Add(board.allGems[x, y]);
                    }
                }
            }
        }

        currentMatches = currentMatches.Distinct().ToList();
    }
}
