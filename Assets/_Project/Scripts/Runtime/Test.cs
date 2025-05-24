using System;
using _Project.Scripts.Runtime;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Player player;
    public Person person;

    private void Start()
    {
        Debug.Log(player.Experience);
        player.PersonAge = 40;
        Debug.Log($"Age = {player.PersonAge}");

        player.ShowStat();
        person.ShowStat();
    }
}
