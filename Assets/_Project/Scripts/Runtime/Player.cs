using UnityEngine;

namespace _Project.Scripts.Runtime
{
    public class Player : Person
    {
        public int Experience { get; private set; } = 30;
        public int Health { get; private set; } = 100;     
        
        /*public Player (int experience, int health)
        {
            Experience = experience;
            if (health > 0)
                Health = health;
        }*/

        private void Start()
        {
            TakeDamage(40);
        }

        public override void ShowStat()
        {
            Debug.Log($"Player name = {PersonName}. Experience = {Experience}");
            Debug.Log(PersonName);
        }

        public override void TakeDamage(int damageValue)
        {
            if (Health > damageValue)
            {
                Health -= damageValue;
                Debug.Log($"My name is {PersonName}. After hitting with the force {damageValue} I have health {Health}");
            }
            else
            {
                Health = 0;
                Debug.Log($"My name is {PersonName}. After hitting with the force {damageValue} i am died.");
            }
        }
    }
}
