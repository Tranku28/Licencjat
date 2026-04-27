using UnityEngine;

namespace GameMechanics.UI
{
    [CreateAssetMenu(fileName = "DiaryHarmonyLevel", menuName = "SO/Diary Harmony Level")]
    public class DiaryHarmonyLevel : ScriptableObject
    {
        [Range(25, 75)]
        public int WorksOnHarmonyLevel;
        public string LinkTag;
        public int MinGap;
        public int MaxGap;
        public int MinLength;
        public int MaxLength;
        public int Seed;
    }
}