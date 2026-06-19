using UnityEngine;

public class ChangeSize : MonoBehaviour
{
    

    private bool player_normal = true;
    [SerializeField] private GameObject player;

    public void changer_player_size()
    {
        if (player_normal)
        {
            player.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            player_normal = false;
        }
        else
        {
            player.transform.localScale = new Vector3(1f, 1f, 1f);
            player_normal = true;
        }
    }

}
