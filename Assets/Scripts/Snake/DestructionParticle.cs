using UnityEngine;

namespace Snake
{
    public class DestructionParticle : MonoBehaviour
    {
        [SerializeField] private GameObject destructionParticle;

        private void OnDestroy()
        {
            Instantiate(destructionParticle, transform.position, transform.rotation);
        }
    }
}
