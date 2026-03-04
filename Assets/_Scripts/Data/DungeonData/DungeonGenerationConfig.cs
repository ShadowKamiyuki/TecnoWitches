using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon Generation Config", menuName = "TecnoWitches/Dungeon Generation Config")]
public class DungeonGenerationConfig : ScriptableObject, IDungeonGenerationConfig
{
    [Header("General")]
    [SerializeField] private int roomCount = 20;
    [Range(0f, 0.5f)]
    [SerializeField] private float extraLoopPercentage = 0.15f;
    [SerializeField] private int minBossDistance = 6;

    [Header("Layout")]
    [SerializeField] private float roomSpacing = 25f;
    [SerializeField] private int maxPlacementAttempts = 50;

    [Header("Special Rooms")]
    [SerializeField] private int treasureRoomCount = 2;
    [SerializeField] private int shopRoomCount = 1;
    [SerializeField] private int eliteRoomCount = 1;
    [Range(0f, 1f)]
    [SerializeField] private float secretRoomChance = 0.1f;

    [Header("Random")]
    [SerializeField] private bool useRandomSeed = true;
    [SerializeField] private int fixedSeed = 12345;

    public int RoomCount => roomCount;
    public float ExtraLoopPercentage => extraLoopPercentage;
    public int MinBossDistance => minBossDistance;
    public float RoomSpacing => roomSpacing;
    public int MaxPlacementAttempts => maxPlacementAttempts;
    public int TreasureRoomCount => treasureRoomCount;
    public int ShopRoomCount => shopRoomCount;
    public int EliteRoomCount => eliteRoomCount;
    public float SecretRoomChance => secretRoomChance;
    public bool UseRandomSeed => useRandomSeed;
    public int FixedSeed => fixedSeed;
}