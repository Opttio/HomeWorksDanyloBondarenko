using System;
using UnityEngine;

namespace _Project.Scripts.Runtime
{
    public class Player : Person
    {
        public int Experience { get; private set; }
        
        public Player (int experience) => Experience = experience;

        public override void ShowStat()
        {
            Debug.Log($"Player name = {PersonName}. Experience = {Experience}");
        }
    }
}
