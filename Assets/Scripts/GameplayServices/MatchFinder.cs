using System.Collections.Generic;
using System.Linq;
using Enums;
using Managers;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private BoardManager _board;
    public List<Gem> currentMatches = new List<Gem>();

    private void Start()
    {
        _board = GameManager.Instance.GetBoardRef();
    }

    
    public void FindAllGemMatches()
    {
        currentMatches.Clear();
        
        for (int i = 0; i < _board.width; i++)
        {
            for (int j = 0; j < _board.height; j++)
            {
                Gem currentGem = _board.boardGrid[i, j];
                
                
                if (currentGem != null)
                {
                    // Horizontal match
                    if (i > 0 && i < _board.width - 1)
                    {
                        Gem leftGem = _board.boardGrid[i - 1, j];
                        Gem rightGem = _board.boardGrid[i + 1, j];
                        if (leftGem != null && rightGem != null)
                        {
                            if (leftGem.type == currentGem.type && rightGem.type == currentGem.type && currentGem.type != GemType.Stone)
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
                    if (j > 0 && j < _board.height - 1)
                    {
                        Gem aboveGem = _board.boardGrid[i, j + 1];
                        Gem belowGem = _board.boardGrid[i, j - 1];
                        if (aboveGem != null && belowGem != null)
                        {
                            if (aboveGem.type == currentGem.type && belowGem.type == currentGem.type && currentGem.type != GemType.Stone)
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
                if (_board.boardGrid[x - 1, y] != null)
                {
                    if (_board.boardGrid[x - 1, y].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x - 1, y));
                    }
                }
            }
            // Right Check
            if (gem.posIndex.x < _board.width - 1)
            {
                if (_board.boardGrid[x + 1, y] != null)
                {
                    if (_board.boardGrid[x + 1, y].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1, y));
                    }
                }
            }
            // Down Check
            if (gem.posIndex.y > 0)
            {
                if (_board.boardGrid[x, y - 1] != null)
                {
                    if (_board.boardGrid[x, y - 1].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y - 1));
                    }
                }
            }
            // Up Check
            if (gem.posIndex.y < _board.height - 1)
            {
                if (_board.boardGrid[x, y + 1] != null)
                {
                    if (_board.boardGrid[x, y + 1].type == GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y + 1));
                    }
                }
            }
        }
    }

    private void MarkBombArea(Vector2Int bombPos)
    {
        
        for (int x = bombPos.x - _board.bombBlastRadius; x <= bombPos.x + _board.bombBlastRadius; x++)
        {
            for (int y = bombPos.y - _board.bombBlastRadius; y <= bombPos.y + _board.bombBlastRadius; y++)
            {
                if (x >= 0 && x < _board.width && y >= 0 && y < _board.height)
                {
                    if (_board.boardGrid[x, y] != null)
                    {
                        _board.boardGrid[x, y].b_IsMatched = true;
                        currentMatches.Add(_board.boardGrid[x, y]);
                    }
                }
            }
        }

        currentMatches = currentMatches.Distinct().ToList();
    }
}
