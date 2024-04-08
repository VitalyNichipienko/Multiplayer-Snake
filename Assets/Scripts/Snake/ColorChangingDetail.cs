using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class ColorChangingDetail : MonoBehaviour
    {
        [SerializeField] private List<MeshRenderer> meshRenderers;

        public void Init(Color color)
        {
            for (int i = 0; i < meshRenderers.Count; i++) 
                meshRenderers[i].material.color = color;
        }
    }
}
