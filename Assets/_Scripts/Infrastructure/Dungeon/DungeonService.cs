using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonService : MonoBehaviour, IDungeonService
{
    [Header("Prefabs")]
    [SerializeField] private GameObject normalRoomPrefab;
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject bossRoomPrefab;

    [Header("Settings")]
    [SerializeField] private float roomSpacing = 25f;

    private DungeonGenerator _generator;
    private List<GameObject> _spawnedRooms = new();

    public DungeonGraph CurrentGraph { get; private set; }
    public Vector3 PlayerSpawn { get; private set; }

    public void Generate(IDungeonGenerationConfig config)
    {
        Clear();

        IRunManager runManager = ServiceLocator.Get<IRunManager>();

        int baseSeed = runManager.CurrentSeed;
        int floor = runManager.CurrentFloor;

        int floorSeed = HashCode.Combine(baseSeed, floor);

        _generator = new DungeonGenerator();

        if (!_generator.Generate(config, floorSeed))
        {
            Debug.LogError("Dungeon generation failed.");
            return;
        }

        CurrentGraph = _generator.Graph;

        BuildVisualDungeon();
    }

    private void BuildVisualDungeon()
    {
        foreach (var node in CurrentGraph.Nodes)
        {
            Vector3 worldPos = new Vector3(
                node.GridPosition.x * roomSpacing,
                0f,
                node.GridPosition.y * roomSpacing
            );

            GameObject prefab = GetPrefabForRoom(node.Type);

            if (prefab == null)
                continue;

            var instance = Instantiate(prefab, worldPos, Quaternion.identity);
            _spawnedRooms.Add(instance);

            if (node.Type == RoomType.Start)
                PlayerSpawn = worldPos;
        }
    }

    private GameObject GetPrefabForRoom(RoomType type)
    {
        return type switch
        {
            RoomType.Start => startRoomPrefab,
            RoomType.Boss => bossRoomPrefab,
            _ => normalRoomPrefab
        };
    }

    public void Clear()
    {
        foreach (var room in _spawnedRooms)
        {
            if (room != null)
                Destroy(room);
        }

        _spawnedRooms.Clear();
        CurrentGraph = null;
    }
}