using UnityEngine;

namespace Ubavar.game.Level
{
    public class Joystick : MonoBehaviour
    {
        [SerializeField] private RectTransform center;
        [SerializeField] private RectTransform knob;
        [SerializeField] private float range;
        [SerializeField] private bool fixedJoystick;
        [SerializeField] private Vector2 direction;
        
        private Canvas canvas;

        public Vector2 Direction => direction;


        private void Start()
        {
            canvas = GetComponentInParent<Canvas>();

            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");
        }

        private void Update()
        {
            Vector3 mousePosition = UnityEngine.Input.mousePosition;
            Vector2 pos = mousePosition;

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                knob.position = pos;
                center.position = pos;
            }
            else if (UnityEngine.Input.GetMouseButton(0))
            {
                knob.position = pos;
                knob.position = center.position + Vector3.ClampMagnitude(knob.position - center.position, center.sizeDelta.x * range * canvas.scaleFactor);

                if (knob.position != mousePosition && !fixedJoystick)
                {
                    Vector3 outsideBoundsVector = mousePosition - knob.position;
                    center.position += outsideBoundsVector;
                }

                direction = (knob.position - center.position).normalized;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                direction = Vector2.zero;
            }
        }
    }
}
