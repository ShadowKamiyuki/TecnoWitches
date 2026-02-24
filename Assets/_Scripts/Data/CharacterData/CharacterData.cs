using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "TecnoWitches/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Character info")]
    [SerializeField] private string id;
    [SerializeField] private Sprite icon;
    [SerializeField] private string displayName;
    [SerializeField] private bool unlockedByDefault;

    [Header("Prefab")]
    [SerializeField] private GameObject characterPrefab;

    [Header("Starting Abilities")]
    [SerializeField] private AbilityData[] startingAbilities;

    // Getters
    public string Id => id;
    public Sprite Icon => icon;
    public string DisplayName => displayName;
    public bool UnlockedByDefault => unlockedByDefault;

    public GameObject CharacterPrefab => characterPrefab;

    public AbilityData[] StartingAbilities => startingAbilities;

    [System.Serializable]
    public struct Stats
    {
        public float maxHealth;
        public float moveSpeed;
        public float damage;
    }

    [SerializeField] private Stats baseStats;
    public Stats BaseStats => baseStats;
}
