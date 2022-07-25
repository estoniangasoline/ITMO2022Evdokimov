using UnityEngine;

namespace Ubavar.game.HexUtils
{
    public class HexUtils
    {
        public static float gap = 0f;
        public static float hexWidth = 1.732f + gap;
        public static float hexHeight = 2.0f + gap;
        
        private static float sin60 = Mathf.Sqrt(3f) * 0.5f;
        
        public static Vector2 GetHex(Vector3 point, float radius)
        {
            int x = Mathf.FloorToInt (point.x/hexWidth);
            int y = Mathf.FloorToInt (point.z/hexHeight*0.75f);
            int col = x;
            int row = y + (x - (x % 2)) /2;

            return new Vector2(col, row);
        }

        public static Vector3 GetWorld(Vector2 point)
        {
            float offset = 0;
            
            if (point.y % 2 != 0)
                offset = hexWidth / 2;

            float x = point.x * hexWidth + offset;
            float y = 0;//transform.position.y;
            float z = point.y * hexHeight * 0.75f;

            return new Vector3(x, y, z);
            
        }
    }
}