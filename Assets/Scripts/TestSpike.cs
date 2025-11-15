using UnityEngine;

public class TestSpike : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = GetComponent<PlayerHealth>();
            player.TakeDamage(10f);
        }
    }
}
