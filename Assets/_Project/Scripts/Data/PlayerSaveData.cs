using System;

namespace _Project.Scripts.Data
{
    [Serializable]
    public class PlayerSaveData
    {
        public float posX = 0;
        public float posY = 0;
        public int distance = 0;
        public int attempt = 3;
        public float highestY = 0;
    }
}