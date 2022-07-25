using UnityEngine;
using DG.Tweening;

namespace Ubavar.game.UI
{
    public class ResultWindow : MonoBehaviour
    {
        [SerializeField] private float fadeDuration;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject win;
        [SerializeField] private GameObject lose;

        public void Win()
        {
            win.SetActive(true);
            lose.SetActive(false);

            SmoothFade(1);
        }

        public void Lose()
        {
            lose.SetActive(true);
            win.SetActive(false);

            SmoothFade(1);
        }

        private void SmoothFade(int targetAlpha)
        {
            canvasGroup.DOFade(targetAlpha, fadeDuration);
        }
    }
}