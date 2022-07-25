using UnityEngine;

namespace Ubavar.game.Level
{
    public class PunchAnimationEventProxy : MonoBehaviour
    {
        public void Punch()
        {
            GetComponentInParent<Character>().Punch();
        }
    }
}