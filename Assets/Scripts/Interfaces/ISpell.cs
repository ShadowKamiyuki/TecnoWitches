using UnityEngine;

public interface ISpell
{
    public void Attack(Vector3 direction, Transform firePoint);
    public string GetName();
}
