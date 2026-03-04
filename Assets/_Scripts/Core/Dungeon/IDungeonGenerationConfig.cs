public interface IDungeonGenerationConfig
{
    int RoomCount { get; }
    float ExtraLoopPercentage { get; }
    int MinBossDistance { get; }

    float RoomSpacing { get; }
    int MaxPlacementAttempts { get; }

    int TreasureRoomCount { get; }
    int ShopRoomCount { get; }
    int EliteRoomCount { get; }
    float SecretRoomChance { get; }

    bool UseRandomSeed { get; }
    int FixedSeed { get; }
}