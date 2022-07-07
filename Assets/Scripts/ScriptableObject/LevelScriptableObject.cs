using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu( fileName = "LevelSO", menuName = "ScriptableObject/Level/LevelScriptableObject")] 
    public class LevelScriptableObject : UnityEngine.ScriptableObject
    {
        [Header("Board Properties")]
        [Tooltip("The Width of the board.")] 
        public int boardWidth;
    
        [Tooltip("The Height of the board.")] 
        public int boardHeight;
    
        [Tooltip("The Prefab of background tile.")] 
        public GameObject tileBgPrefab;

        
        [Header("Gem Properties")]
        [Tooltip("The transition speed in which gems move from one place to another.")] 
        public float gemTransitionSpeed;
        
        [Tooltip("Spawning chances of bomb.")] 
        public float bombChance = 5f;
        
        [Tooltip("Blast radius of the bomb! 1 means it will destroy 1 gem around the bomb from all of the four directions.")] 
        public int bombBlastRadius = 1;
        
        [Tooltip("Add different different gems here which you wants to have in the game.")]
        public Gem[] gems;
        
        [Tooltip("The Prefab of bomb.")] 
        public Gem bomb;
        
    }
}