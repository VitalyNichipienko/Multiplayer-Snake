using System.Collections;
using UnityEngine;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        private const float FadingInterval = 0.03f;
        private const float FadingValue = 0.03f;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1;
        }

        public void Hide() =>
            StartCoroutine(FadeIn());

        private IEnumerator FadeIn()
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= FadingValue;
                yield return new WaitForSeconds(FadingInterval);
            }
            
            gameObject.SetActive(false);
        }
    }
}
