using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private void Update()
    {
        if (!player.isAlive) return;

        //Attack controlls
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            player.PerformAttack(false);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            player.PerformAttack(true);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            player.UseUltimate();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}