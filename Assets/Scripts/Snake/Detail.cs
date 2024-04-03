using UnityEngine;

namespace Snake
{
    public class Detail : MonoBehaviour
    {
        [SerializeField] private MeshRenderer renderer;

        public MeshRenderer Renderer => renderer;
    }
}
