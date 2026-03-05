using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonService : MonoBehaviour, IDungeonService
{
    [Header("Prefabs")]
    [SerializeField] private GameObject normalRoomPrefab;
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject bossRoomPrefab;
    [SerializeField] private GameObject corridorPrefab;

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
        Dictionary<RoomNode, RoomView> roomViews = new();

        // 1 Instanciar salas
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

            var view = instance.GetComponent<RoomView>();
            if (view != null)
                roomViews[node] = view;

            if (node.Type == RoomType.Start)
                PlayerSpawn = worldPos;
        }

        // 2 Abrir puertas correctamente
        foreach (var node in CurrentGraph.Nodes)
        {
            if (!roomViews.TryGetValue(node, out var viewA))
                continue;

            foreach (var neighbor in node.Connections)
            {
                DoorDirection dir = GetDirection(node, neighbor);
                viewA.OpenDoor(dir);
            }
        }

        // 3 Construir pasillos usando sockets
        BuildCorridors(roomViews);
    }

    private void BuildCorridors(Dictionary<RoomNode, RoomView> roomViews)
    {
        var processed = new HashSet<(RoomNode, RoomNode)>();

        foreach (var node in CurrentGraph.Nodes)
        {
            foreach (var neighbor in node.Connections)
            {
                if (processed.Contains((neighbor, node)))
                    continue;

                if (!roomViews.TryGetValue(node, out var viewA)) continue;
                if (!roomViews.TryGetValue(neighbor, out var viewB)) continue;
                if (!AreAligned(node, neighbor)) continue;

                DoorDirection dirA = GetDirection(node, neighbor);
                DoorDirection dirB = GetOpposite(dirA);

                Vector3 start = viewA.GetDoorPosition(dirA);
                Vector3 end = viewB.GetDoorPosition(dirB);

                SpawnCorridorBetween(start, end);

                processed.Add((node, neighbor));
            }
        }
    }

    private bool AreAligned(RoomNode a, RoomNode b)
    {
        return a.GridPosition.x == b.GridPosition.x ||
               a.GridPosition.y == b.GridPosition.y;
    }

    private DoorDirection GetDirection(RoomNode from, RoomNode to)
    {
        Vector2Int delta = to.GridPosition - from.GridPosition;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            return delta.x > 0 ? DoorDirection.East : DoorDirection.West;
        else
            return delta.y > 0 ? DoorDirection.North : DoorDirection.South;
    }

    private DoorDirection GetOpposite(DoorDirection dir)
    {
        return dir switch
        {
            DoorDirection.North => DoorDirection.South,
            DoorDirection.South => DoorDirection.North,
            DoorDirection.East => DoorDirection.West,
            DoorDirection.West => DoorDirection.East,
            _ => DoorDirection.North
        };
    }

    private void SpawnCorridorBetween(Vector3 start, Vector3 end)
    {
        Vector3 delta = end - start;

        bool horizontal = Mathf.Abs(delta.x) > Mathf.Abs(delta.z);

        float distance = horizontal ? Mathf.Abs(delta.x) : Mathf.Abs(delta.z);

        float segmentLength = 5f;

        int segmentCount = Mathf.RoundToInt(distance / segmentLength);

        Vector3 direction = (end - start).normalized;

        Quaternion rotation = horizontal
            ? Quaternion.Euler(0, 90, 0)
            : Quaternion.identity;

        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 pos = start + direction * (segmentLength * (i + 0.5f));

            var corridor = Instantiate(corridorPrefab, pos, rotation);

            _spawnedRooms.Add(corridor);
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