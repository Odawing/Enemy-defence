using System.Collections.Generic;
using UnityEngine;

public class GameManagerScr : MonoBehaviour
{
    public static GameManagerScr Instance;

    public Player player;

    public List<Enemy> allEnemies = new List<Enemy>();

    public int enemiesKilled;

    private void Awake()
    {
        Instance = this;
    }

    public void AddKill()
    {
        enemiesKilled++;

        // add ultimate charge
        if (player.ultimateCharge < player.maxUltimateCharge)
            player.ultimateCharge++;

        GameInterface.Instance.RefreshEnemyCounter();
        GameInterface.Instance.RefreshUltimateBar();
    }
}