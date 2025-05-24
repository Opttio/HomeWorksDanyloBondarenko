using UnityEngine;

namespace _Project.Scripts.Runtime
{
    public class Test : MonoBehaviour
    {
        public Player player;
        // public Person person;

        private void Start()
        {
            Debug.Log(player.Experience);
            player.PersonAge = 40;
            Debug.Log($"Age = {player.PersonAge}");

            player.ShowStat();
            // person.ShowStat();
            // player.TakeDamage(40);
        }
    }
}
