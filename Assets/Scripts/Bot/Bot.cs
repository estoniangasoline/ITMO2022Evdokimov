using System.Collections.Generic;
using Ubavar.core;
using Ubavar.game.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ubavar.game.Level
{
    public class Bot : MonoBehaviour
    {
        private static Dictionary<BotDifficult, int> SeekRadius = new Dictionary<BotDifficult, int>
        {
            {BotDifficult.Easy, 2},
            {BotDifficult.Normal, 2},
            {BotDifficult.Hard, 4},
        };

        public BotDifficult Difficult = BotDifficult.Easy;

        private GameObject nextTargetGameObject = null;
        private Vector3 nextTarget = Vector3.zero;
        private float maxSeekRadius;
        private int groundMask;
        private RaycastHit[] tempRaycastHits = new RaycastHit[9];
        private Character character;

        public void OnEnable()
        {
            maxSeekRadius = SeekRadius[Difficult];
            character = GetComponent<Character>();
            groundMask = Layers.OnlyIncluding(Layers.CELL);
        }

        public void OnDisable()
        {
            character.moveDirection = Vector3.zero;
        }

        public void FixedUpdate()
        {
            if (Difficult == BotDifficult.Easy && !character.IsGrounded())
            {
                return;
            }

            Vector3 transformPosition = transform.position;
            Vector3 transformForward = transform.forward;

            if (nextTarget == Vector3.zero || nextTargetGameObject == null)
            {
                int raycastHitsCount = 0;

                float seekRadius;

                if (Difficult == BotDifficult.Easy)
                {
                    seekRadius = maxSeekRadius;
                    raycastHitsCount = Physics.SphereCastNonAlloc(transformPosition, seekRadius, transformForward, tempRaycastHits, 0,
                        groundMask, QueryTriggerInteraction.Ignore);
                }
                else
                {
                    seekRadius = 1;

                    do
                    {
                        raycastHitsCount = Physics.SphereCastNonAlloc(transformPosition, seekRadius, transformForward, tempRaycastHits, 0,
                            groundMask, QueryTriggerInteraction.Ignore);

                        seekRadius += 1;
                    } while (raycastHitsCount == 0 && seekRadius <= maxSeekRadius);
                }

                if (raycastHitsCount > 0)
                {
                    RaycastHit raycastHit = tempRaycastHits[Random.Range(0, raycastHitsCount)];
                    nextTarget = raycastHit.transform.position;
                    nextTargetGameObject = raycastHit.transform.gameObject;
                }
            }

            Vector3 direction = (nextTarget - transformPosition);
            direction.y = 0;
            float magnitude = direction.magnitude;

            if (magnitude > 0.5f && magnitude < maxSeekRadius)
            {
                direction.Normalize();
            }
            else
            {
                nextTarget = Vector3.zero;
                nextTargetGameObject = null;
                direction = Vector3.zero;
            }

            character.moveDirection = direction;
        }
    }
}