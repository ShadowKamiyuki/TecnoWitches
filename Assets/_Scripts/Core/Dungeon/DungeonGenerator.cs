using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator
{
    public DungeonGraph Graph { get; private set; }

    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.down
    };

    public bool Generate(IDungeonGenerationConfig config, int seed)
    {
        Random.InitState(seed);

        Graph = new DungeonGraph(seed);

        GenerateGrid(config.RoomCount);

        AssignSpecialRooms();

        AddLoops(config.ExtraLoopPercentage);

        return true;
    }

    // ================================
    // GRID GENERATION
    // ================================

    private void GenerateGrid(int roomCount)
    {
        Dictionary<Vector2Int, RoomNode> grid = new();

        Queue<RoomNode> frontier = new();

        RoomNode start = new RoomNode(0);
        start.Type = RoomType.Start;
        start.Size = Vector2Int.one;

        start.SetPosition(Vector2Int.zero);

        Graph.Nodes.Add(start);

        grid[start.GridPosition] = start;

        frontier.Enqueue(start);

        int id = 1;

        while (Graph.Nodes.Count < roomCount && frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            List<Vector2Int> dirs = new(Directions);
            Shuffle(dirs);

            foreach (var dir in dirs)
            {
                if (Graph.Nodes.Count >= roomCount)
                    break;

                Vector2Int pos = current.GridPosition + dir * 2;

                if (grid.ContainsKey(pos))
                    continue;

                RoomNode room = new RoomNode(id++);
                room.Size = Vector2Int.one;

                room.SetPosition(pos);

                Graph.Nodes.Add(room);

                grid[pos] = room;

                current.Connect(room);

                frontier.Enqueue(room);
            }
        }
    }

    // ================================
    // LOOPS
    // ================================

    private void AddLoops(float percentage)
    {
        int loops = Mathf.RoundToInt(Graph.Nodes.Count * percentage);

        for (int i = 0; i < loops; i++)
        {
            var a = Graph.Nodes[Random.Range(0, Graph.Nodes.Count)];

            foreach (var dir in Directions)
            {
                Vector2Int target = a.GridPosition + dir * 2;

                var b = Graph.Nodes.Find(n => n.GridPosition == target);

                if (b == null)
                    continue;

                if (a.Connections.Contains(b))
                    continue;

                a.Connect(b);

                break;
            }
        }
    }

    // ================================
    // SPECIAL ROOMS
    // ================================

    private void AssignSpecialRooms()
    {
        var start = Graph.Nodes[0];

        var boss = GetFarthest(start);

        boss.Type = RoomType.Boss;
        boss.Size = new Vector2Int(2, 2);
    }

    private RoomNode GetFarthest(RoomNode start)
    {
        Dictionary<RoomNode, int> dist = new();
        Queue<RoomNode> q = new();

        dist[start] = 0;
        q.Enqueue(start);

        RoomNode farthest = start;

        while (q.Count > 0)
        {
            var n = q.Dequeue();

            foreach (var c in n.Connections)
            {
                if (dist.ContainsKey(c))
                    continue;

                dist[c] = dist[n] + 1;

                if (dist[c] > dist[farthest])
                    farthest = c;

                q.Enqueue(c);
            }
        }

        return farthest;
    }

    // ================================
    // UTILS
    // ================================

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}