using System;
using UnityEngine;

namespace StaticData
{
    [Serializable]
    public class SkinData
    {
        [SerializeField] private Color headColor;
        [SerializeField] private Color detailColor;
        [SerializeField] private Color tailColor;

        public Color HeadColor => headColor;
        public Color DetailColor => detailColor;
        public Color TailColor => tailColor;
    }
}