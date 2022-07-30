using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu( fileName = "TargetScoreSO", menuName = "ScriptableObject/TargetScore/TargetScoreScriptableObject")] 
    public class TargetScoreSO : UnityEngine.ScriptableObject
    {
        [Header("Score Targets")]
        [Tooltip("Target score to earn stars")]
        public int star1ScoreTarget, star2ScoreTarget, star3ScoreTarget;
    }
}