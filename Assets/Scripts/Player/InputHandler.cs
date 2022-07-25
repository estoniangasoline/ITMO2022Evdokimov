using UnityEngine;

namespace Ubavar.game.Level
{
    [RequireComponent(typeof(Character))]
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Joystick joystick;

        private Character character;
        private Vector3 direction;

        public void OnEnable()
        {
            character = GetComponent<Character>();
            direction = new Vector3();
        }

        public void FixedUpdate()
        {
            Vector2 direction = joystick.Direction;
            this.direction.x = direction.x;
            this.direction.z = direction.y;
            character.moveDirection = this.direction;
        }
    }
}