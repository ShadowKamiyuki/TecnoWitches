using UnityEngine;

public interface IDungeonService
{
    void Generate(IDungeonGenerationConfig config);
    void Clear();

    DungeonGraph CurrentGraph { get; }
    Vector3 PlayerSpawn { get; }
}