using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "SkinsConfig", menuName = "StaticData/SkinsConfig")]
    public class SkinsConfig : ScriptableObject
    {
        [SerializeField] private SkinData[] skinData;

        public SkinData[] SkinData => skinData;
    }
}