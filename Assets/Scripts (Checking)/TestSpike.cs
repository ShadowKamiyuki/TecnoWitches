using UnityEngine;

public class TestSpike : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

            if (player != null )
            {
                player.TakeDamage(100f);
            }
        }
    }
}
