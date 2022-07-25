using UnityEngine;

namespace Ubavar.game.Level
{
    public class HexPopIt : MonoBehaviour
    {
        [SerializeField] private Transform transformBone;
        [SerializeField] private Renderer renderer;

        private MaterialPropertyBlock materialPropertyBlock;
        private bool isPressed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isPressed) return;

            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                transformBone.localPosition = Vector3.down;

                isPressed = true;
            }
        }

        public void SetMaterial(Material material)
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetColor("_Color", material.GetColor("_Color"));
            materialPropertyBlock.SetColor("_EmissionColor", material.GetColor("_EmissionColor"));
            renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}