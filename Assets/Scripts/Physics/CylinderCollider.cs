using UnityEngine;

namespace Ubavar.core.Utils
{
    public class CylinderCollider : MonoBehaviour
    {
        [SerializeField] private float radius = 0.5f;
        [SerializeField] private float height = 1.0f;
        [SerializeField] private Vector3 center = new Vector3(0, 0, 0);
        [Range(3, 64)]
        [SerializeField] private int boxCount = 5;
        [Range(0.1f, 3.0f)]
        [SerializeField] private float widthScale = 1.0f;
        [SerializeField] private bool capTop;
        [SerializeField] private bool capBottom;
        [Space]
        [SerializeField] private bool isTrigger;
        [SerializeField] private PhysicMaterial material;
        [SerializeField] private Transform colliderParent;

        private void Awake()
        {
            CreateColliders(colliderParent == null ? transform : colliderParent);
            enabled = false;
        }

        public void CreateColliders(Transform parent)
        {
            Vector3 boxSize = CalculateBoxSize();
            float radiusStep = CalculateRotationStep();
            Transform colliderTransform;

            for (int i = 0; i < boxCount; i++)
            {
                colliderTransform = CreateBoxCollider(i, boxSize, radiusStep * i).transform;
                colliderTransform.SetParent(transform, false);
                colliderTransform.SetParent(parent, true);
            }

            if (capTop)
            {
                colliderTransform = CreateCapCollider(true).transform;
                colliderTransform.SetParent(transform, false);
                colliderTransform.SetParent(parent, true);
            }

            if (capBottom)
            {
                colliderTransform = CreateCapCollider(false).transform;
                colliderTransform.SetParent(transform, false);
                colliderTransform.SetParent(parent, true);
            }
        }

        private BoxCollider CreateBoxCollider(int index, Vector3 size, float rotationY)
        {
            var collider = new GameObject("Cylinder_Box_" + index).AddComponent<BoxCollider>();
            collider.size = size;
            collider.transform.localPosition = center;
            collider.transform.localRotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
            collider.isTrigger = isTrigger;
            collider.material = material;
            collider.tag = gameObject.tag;
            GameObjectUtils.MoveToLayer(collider.gameObject, gameObject.layer);
            return collider;
        }

        private SphereCollider CreateCapCollider(bool isTop)
        {
            var collider = new GameObject("Cylinder_Cap_" + (isTop ? "Top" : "Bottom")).AddComponent<SphereCollider>();
            collider.radius = radius;
            collider.center = center + new Vector3(0.0f, height * (isTop ? 0.5f : -0.5f), 0.0f);
            collider.isTrigger = isTrigger;
            collider.material = material;
            collider.tag = gameObject.tag;
            GameObjectUtils.MoveToLayer(collider.gameObject, gameObject.layer);
            return collider;
        }

        private Vector3 CalculateBoxSize()
        {
            float circumference = radius * 2.0f * Mathf.PI;

            float width = circumference / boxCount;
            width = radius / boxCount * 2.0f * widthScale;

            return new Vector3(width, height, radius * 2.0f);
        }

        private float CalculateRotationStep()
        {
            return 360.0f / boxCount;
        }
    }
}