using System;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    public Board board;

    // private void Awake()
    // {
    //     // board = FindObjectOfType<Board>();
    // }
    public void FindAllGemMatches()
    {
        
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
                            }
                        }
                    }
                    // Vertical match
                    if (j > 0 && j < board.height - 1)
                    {
                        Gem aboveGem = board.allGems[i, j +1];
                        Gem belowGem = board.allGems[i, j -1];
                        if (aboveGem != null && belowGem != null)
                        {
                            if (aboveGem.type == currentGem.type && belowGem.type == currentGem.type)
                            {
                                currentGem.b_IsMatched = true;
                                aboveGem.b_IsMatched = true;
                                belowGem.b_IsMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
