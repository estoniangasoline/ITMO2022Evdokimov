namespace Ubavar.core
{
    public static class Layers
    {
        public const int DEFAULT = 0;
        public const int TRANSPARENT_FX = 1;
        public const int IGNORE_RAYCAST = 2;
        public const int WATER = 4;
        public const int UI = 5;
        public const int CELL = 8;
        public const int CHARACTER = 9;
        public const int IGNORE_ALL = 31;


        public static int OnlyIncluding(params int[] layers)
        {
            int mask = 0;
            for (var i = 0; i < layers.Length; i++)
                mask |= (1 << layers[i]);

            return mask;
        }
    }
}