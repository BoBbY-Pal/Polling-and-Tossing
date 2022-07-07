using ScriptableObject;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class BoardManager : MonoBehaviour
    {
        #region variables
    
        [HideInInspector]
        public int width;
    
        [HideInInspector] 
        public int height;
    
        [HideInInspector]
        public GameObject tileBgPrefab;

        [HideInInspector]
        public float gemTransitionSpeed;
        [HideInInspector]
        public Gem bomb;
        
        [HideInInspector]
        public float bombChance = 2f;
        [HideInInspector]
        public int bombBlastRadius = 1;
        
        [HideInInspector]
        public Gem[] gems;
        public Gem[,] boardGrid;

        private BoardLayout _boardLayout;
        private Gem[,] _storedLayout;

        public LevelScriptableObject levelData;

        #endregion

        private void Awake()
        {
            _boardLayout = GetComponent<BoardLayout>();
            FetchData();
        }

        private void Start()
        {
            boardGrid = new Gem[width, height];
            SetupBoard();

            _storedLayout = new Gem[width, height];
        }

        private void FetchData()
        {
            width = levelData.boardWidth;
            height = levelData.boardHeight;
            tileBgPrefab = levelData.tileBgPrefab;
            gemTransitionSpeed = levelData.gemTransitionSpeed;
            bomb = levelData.bomb;
            bombChance = levelData.bombChance;
            gems = levelData.gems;
            bombBlastRadius = levelData.bombBlastRadius;
        }
        
        private void SetupBoard()
        {
            if (_boardLayout != null)
            {
                _storedLayout = _boardLayout.GetLayout();
            }
        
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Factory.Instance.SpawnTile(tileBgPrefab, new Vector2Int(i, j), this);

                    if (_storedLayout[i, j] != null)
                    {
                        Factory.Instance.SpawnGem(new Vector2Int(i, j), _storedLayout[i, j], this);
                    }
                    else
                    {
                        int gemToUse = GetUniqueGem(i, j);
                        Factory.Instance.SpawnGem(new Vector2Int(i,j), gems[gemToUse], this);
                    }
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
}
