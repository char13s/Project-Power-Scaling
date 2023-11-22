using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Game
{
    private Stats stats;
    Vector3 location;

    public Stats Stats { get => stats; set => stats = value; }
    public Vector3 Location { get => location; set => location = value; }

    public Game(Player player) {
        //Spawn = GameController.GetGameController().Spawn.transform.position;
        // Debug.Log(Position[0].ToString());
        location = player.transform.position;
        Stats = player.stats;
        Debug.Log("Game was saved");
    }
}
