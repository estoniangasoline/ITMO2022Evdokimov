using UnityEngine;

namespace Ubavar.game.Level
{
    public class CharacterShadowPlacer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0.01f, 0f);
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float maxDistance = 100f;

        private RaycastHit[] raycastHit = new RaycastHit[1];

        private void Update()
        {
            int count = Physics.RaycastNonAlloc(transform.position, -Vector3.up, raycastHit, maxDistance, layerMask,
                QueryTriggerInteraction.Ignore);

            if (count > 0)
            {
                target.gameObject.SetActive(true);

                target.position = raycastHit[0].point + offset;
            }
            else
            {
                target.gameObject.SetActive(false);
            }
        }
    }
}