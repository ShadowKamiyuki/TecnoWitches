using UnityEngine;

[System.Obsolete]
public class CharacterDataOld : ScriptableObject
{
    [SerializeField] private Sprite icon;
    public Sprite Icon { get => icon; set => icon = value; }

    [SerializeField] private new string name;
    public string Name { get => name; set => name = value; }

    [System.Serializable]
    public struct Stats
    {
        public float maxHealth, maxEnergy, energyRecovery, moveSpeed, critRate, critDamage;
        public float might, projectileSpeed, magnet;

        public Stats(float maxHealth = 1000, float maxEnergy = 100, float energyRecovery = 10f, float moveSpeed = 1f, float critRate = 1f, float critDamage = 1f, float might = 1f, float projectileSpeed = 1f, float magnet = 30f)
        {
            this.maxHealth = maxHealth;
            this.maxEnergy = maxEnergy;
            this.energyRecovery = energyRecovery;
            this.moveSpeed = moveSpeed;
            this.critRate = critRate;
            this.critDamage = critDamage;
            this.might = might;
            this.projectileSpeed = projectileSpeed;
            this.magnet = magnet;
        }

        public static Stats operator +(Stats s1, Stats s2)
        {
            s1.maxHealth += s2.maxHealth;
            s1.moveSpeed += s2.moveSpeed;
            s1.maxEnergy += s2.maxEnergy;
            s1.critRate += s2.critRate;
            s1.critDamage += s2.critDamage;
            s1.might += s2.might;
            s1.projectileSpeed += s2.projectileSpeed;
            s1.magnet += s2.magnet;
            return s1;
        }
    }

    public Stats stats = new Stats(1000);
}
