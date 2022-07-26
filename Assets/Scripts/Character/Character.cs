using System;
using Ubavar.core.Data.Static;
using Ubavar.core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Ubavar.core;

namespace Ubavar.game.Level
{
    public class Character : MonoBehaviour
    {
        private static string IS_JUMPING = "IsJumping";
        private static string IS_WALKING = "IsWalking";
        private static string IS_GROUNDED = "IsGrounded";
        private static string IS_PUNCHING = "IsPunching";
        private static string IS_DANCING = "IsDancing";
        private static string IS_DEAD = "Dead";

        public bool Moveable = true;
        public GameObject DeathEffect;
        public GameObject StartEffect;

        [HideInInspector]
        public Vector3 moveDirection;

        [SerializeField] private float speed;
        [SerializeField] private float jumpSpeed = 8.0F;
        [SerializeField] private bool isBoxing = false;
        [SerializeField] private bool isEasyPunch = false;

        private bool lastIsGrounded = false;
        private float isGroundedDistance = 0.5f;
        private Vector3 customGravity = Vector3.negativeInfinity;
        private bool rotateJumpBooster = false;

        private float punchDistance = 1f;
        private float punchDelay = 0.5f;
        private float punchForce = 700;
        private float punchForceEasy = 175;
        private RaycastHit[] tempRaycastHits = new RaycastHit[2];
        private RaycastHit[] tempRaycastHit = new RaycastHit[1];

        private bool isDead = false;
        private float height;
        private int groundMask;
        private int characterMask;
        private Animator animator;
        private Rigidbody rigidbody;
        private Renderer renderer;
        private Collider collider;
        private string name;

        public event UnityAction<Character> CharacterDead;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;
                UpdateName();
            }
        }

        private ResourceStaticData skinResourceStaticData;

        public ResourceStaticData Skin
        {
            get
            {
                return skinResourceStaticData;
            }
            set
            {
                if (skinResourceStaticData == value)
                {
                    return;
                }

                if (GetComponent<InputHandler>() != null)
                    skinResourceStaticData = LevelSettings.Instance.GetPlayerSkin();
                else
                    skinResourceStaticData = value;

                UpdateSkin();
            }
        }

        private bool punchBoost = false;

        public bool PunchBoost
        {
            get
            {
                return punchBoost;
            }
            set
            {
                if (punchBoost == value)
                {
                    return;
                }

                punchBoost = value;

                if (punchBoost)
                {
                    ActivatePunchBooster();
                }
                else
                {
                    UpdateSkin();
                }
            }
        }

        private void Start()
        {
            if (isBoxing)
            {
                ActivatePunchBooster();
            }
        }

        public void Init()
        {
            animator = GetComponentInChildren<Animator>();
            height = GetComponent<CapsuleCollider>().height;
            groundMask = Layers.OnlyIncluding(Layers.CELL);
            characterMask = Layers.OnlyIncluding(Layers.CHARACTER);
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();

            animator.gameObject.AddComponent<PunchAnimationEventProxy>();

            renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (renderer == null)
                renderer = GetComponentInChildren<MeshRenderer>();


            Invoke(nameof(CreateStartEffect), 0.05f);
        }

        public void CreateStartEffect()
        {
            if (StartEffect != null && renderer.isVisible)
            {
                Instantiate(StartEffect, transform.position, StartEffect.transform.rotation);
            }
        }

        private void UpdateName()
        {
            GameObjectUtils.FindDescendentTransformByPath(gameObject.transform, "TextSkinContainer")
                .GetComponentInChildren<TextMeshPro>().text = name;
        }

        private void UpdateSkin()
        {
            GameObject skinContainer = GameObjectUtils.FindDescendentTransform(transform, "SkinContainer").gameObject;
            GameObjectUtils.ClearImmediate(skinContainer);
            Instantiate(skinResourceStaticData.Prefab, skinContainer.transform);

            animator = skinContainer.GetComponentInChildren<Animator>();
            animator.gameObject.AddComponent<PunchAnimationEventProxy>();
            renderer = skinContainer.GetComponentInChildren<SkinnedMeshRenderer>();

            if (PunchBoost)
            {
                ActivatePunchBooster();
            }

            Invoke(nameof(CreateStartEffect), 0.05f);
        }

        public void FixedUpdate()
        {
            if (isDead || rigidbody == null) return;

            Boolean isWalking = false;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                rigidbody.rotation = targetRotation;

                if (Moveable)
                {
                    rigidbody.MovePosition(rigidbody.position + transform.forward * speed * Time.fixedDeltaTime);
                    isWalking = true;
                }
            }

            Boolean isGrounded = IsGrounded();

            if (!isGrounded && lastIsGrounded)
            {
                rigidbody.AddForce(0, jumpSpeed, 0, ForceMode.VelocityChange);
                animator.SetTrigger(IS_JUMPING);
            }

            lastIsGrounded = isGrounded;

            animator.SetBool(IS_WALKING, isWalking);
            animator.SetBool(IS_GROUNDED, isGrounded);

            if (PunchBoost)
            {
                Rigidbody isPunching = GetPunching();

                if (isPunching)
                {
                    animator.SetTrigger(IS_PUNCHING);
                }
            }

            if (transform.position.y < 0)
            {
                if (LevelSettings.Instance.UseBloodEffect)
                {
                    DeathEffect = LevelSettings.Instance.BloodEffect;
                }

                if (DeathEffect != null && renderer.isVisible)
                {
                    Instantiate(DeathEffect, transform.position, DeathEffect.transform.rotation);
                }

                CharacterDead.Invoke(this);

                DestroyImmediate(gameObject);

                isDead = true;
            }

            if (!customGravity.Equals(Vector3.negativeInfinity))
            {
                if (rigidbody != null)
                {
                    rigidbody.AddForce(customGravity * rigidbody.mass);
                }
            }
        }

        private void Dead()
        {
            animator.SetTrigger(IS_DEAD);

            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = true;
        }

        public void StartDancing()
        {
            animator.SetBool(IS_DANCING, true);
        }

        public bool IsGrounded()
        {
            int count = Physics.RaycastNonAlloc(transform.position + transform.up * height, -transform.up, tempRaycastHit,
                height + isGroundedDistance, groundMask, QueryTriggerInteraction.Ignore);

            return count > 0;
        }

        private Rigidbody GetPunching()
        {
            int raycastsCount = Physics.SphereCastNonAlloc(transform.position + transform.up * height / 2 + transform.forward * 0.5f, 0.5f, transform.forward, tempRaycastHits, 0, characterMask);

            if (raycastsCount > 1)
            {
                for (int i = 0; i < raycastsCount; i++)
                {
                    RaycastHit raycastHit = tempRaycastHits[i];

                    if (raycastHit.transform != transform)
                    {
                        return raycastHit.rigidbody;
                    }
                }
            }

            return null;
        }

        public void Punch()
        {
            Rigidbody isPunching = GetPunching();

            if (isPunching)
            {
                isPunching.velocity = Vector3.zero;
                if (!isEasyPunch)
                {
                    isPunching.AddForce(transform.forward * punchForce, ForceMode.Impulse);
                }
                else isPunching.AddForce(transform.forward * punchForceEasy, ForceMode.Impulse);
            }
        }

        private void ActivatePunchBooster()
        {
            PunchBoost = true;

            Transform leftHandBone = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            GameObject boxingGloveLeft = Instantiate(Resources.Load<GameObject>("Prefabs/Boosters/BoxingGloveLeft"));
            boxingGloveLeft.transform.parent = leftHandBone;
            boxingGloveLeft.transform.localPosition = Vector3.zero;
            boxingGloveLeft.transform.LookAt(leftHandBone.transform.position + leftHandBone.up);

            Transform rightHandBone = animator.GetBoneTransform(HumanBodyBones.RightHand);
            GameObject boxingGloveRight = Instantiate(Resources.Load<GameObject>("Prefabs/Boosters/BoxingGloveRight"));
            boxingGloveRight.transform.parent = rightHandBone;
            boxingGloveRight.transform.localPosition = Vector3.zero;
            boxingGloveRight.transform.LookAt(rightHandBone.transform.position + rightHandBone.up);
        }
    }
}