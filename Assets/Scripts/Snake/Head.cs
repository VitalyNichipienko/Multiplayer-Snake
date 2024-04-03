using UnityEngine;

namespace Snake
{
    public class Head : MonoBehaviour
    {
        [SerializeField] private MeshRenderer renderer;

        public MeshRenderer Renderer => renderer;
    }
}
