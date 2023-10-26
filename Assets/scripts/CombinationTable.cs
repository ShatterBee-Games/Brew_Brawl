using UnityEngine;

namespace zoe 
{
    [CreateAssetMenu(fileName = "CombinationTable", menuName = "Game/Combination Table", order = 1)]
    public class CombinationTable : ScriptableObject
    {
        [System.Serializable]
        public class CombinationEffect
        {
            public string itemA;
            public string itemB;
            public string resultEffect;
        }

        public CombinationEffect[] combinations;
    }
}
