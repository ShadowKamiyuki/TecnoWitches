using UnityEngine;
using UnityEngine.Pool;

public class TestSpell : MonoBehaviour, ISpell
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float speed = 15f;

    public void Attack(Vector3 direction, Transform firePoint)
    {
        if (prefab == null) return;

        GameObject fireball = Instantiate(prefab, firePoint.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody>().velocity = direction * speed;
        Debug.Log("Fireball casted!");
    }

    public string GetName() => "Fireball";
}
