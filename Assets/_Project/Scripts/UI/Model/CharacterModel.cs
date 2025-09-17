using UnityEngine;

namespace _Project.Scripts.UI.Model
{
    public static class CharacterModel
    {
        public static int Distance;
        public static int Attempt;
        public static float GameSpeed;
        public static Vector2 Position;
        public static float HighestY;
        public static bool IsLoaded;
        public static void Reset()
        {
            Distance = 0;
            Attempt = 3;
            HighestY = 0;
            Position = Vector2.zero;
            GameSpeed = 1f;
        }
    }
}