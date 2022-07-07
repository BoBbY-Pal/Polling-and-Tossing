using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu( fileName = "TargetScoreSO", menuName = "ScriptableObject/TargetScore/TargetScoreScriptableObject")] 
    public class TargetScoreSO : UnityEngine.ScriptableObject
    {
        [Header("Round Properties")]
        [Tooltip("Amount of time in seconds after which round will end.")]
        public float roundTime = 60f;

        [Header("Score Targets")]
        [Tooltip("Target score to earn stars")]
        public int star1ScoreTarget, star2ScoreTarget, star3ScoreTarget;
    }
}