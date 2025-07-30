using UnityEngine;

public class SpikeFloor : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Player collidedPlayer = collision.gameObject.GetComponent<Player>();

        if (collidedPlayer != null)
        {
            collidedPlayer.Die();
        }
    }
}
