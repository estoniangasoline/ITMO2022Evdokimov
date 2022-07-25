using DG.Tweening;
using Ubavar.core.Utils;
using UnityEngine;

namespace Ubavar.game.Level
{
    public class Cell : MonoBehaviour
    {
        public float DestroyDelay = 2f;
        public float WhiteColorDelay = 1.5f;
        public bool Destroyable = true;

        private MaterialPropertyBlock materialPropertyBlock;
        private Renderer renderer;
        private Tween tween;

        public void Awake()
        {
            renderer = GameObjectUtils.FindDescendentTransform(gameObject.transform, "Body").GetComponent<Renderer>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (renderer.isVisible)
            {
                renderer.transform.DOLocalMoveY(-0.25f, 0.25f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine);
            }

            GetComponent<CylinderCollider>().enabled = true;

            if (Destroyable)
            {
                StartDestroy();
            }
        }

        public void StartDestroy(bool immediate = false)
        {
            float destroyDelay = immediate ? DestroyDelay - WhiteColorDelay : DestroyDelay;
            float whiteDelay = immediate ? 0 : WhiteColorDelay;

            tween = DOVirtual.DelayedCall(destroyDelay, () => Destroy(gameObject));

            DOTween.To(() => materialPropertyBlock.GetColor("_EmissionColor"),
                value =>
                {
                    materialPropertyBlock.SetColor("_EmissionColor", value);
                    renderer.SetPropertyBlock(materialPropertyBlock);
                },
                Color.white,
                DestroyDelay - WhiteColorDelay).SetDelay(whiteDelay);

        }

        public void SetMaterial(Material material)
        {
            HexPopIt popItController = GetComponent<HexPopIt>();
            if (popItController != null)
            {
                popItController.SetMaterial(material);
            }
            else
            {
                materialPropertyBlock = new MaterialPropertyBlock();
                materialPropertyBlock.SetColor("_Color", material.GetColor("_Color"));
                materialPropertyBlock.SetColor("_EmissionColor", material.GetColor("_EmissionColor"));
                renderer.SetPropertyBlock(materialPropertyBlock);
            }
        }

        public void StopDestroy()
        {
            tween.Kill();
            Destroyable = false;
        }
    }
}