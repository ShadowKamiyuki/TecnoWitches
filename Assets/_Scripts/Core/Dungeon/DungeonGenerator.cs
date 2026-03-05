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
        Graph.Generate(config.RoomCount, config.ExtraLoopPercentage);

        AssignSpecialRooms();

        GenerateGridLayout();

        AddGridLoops(config.ExtraLoopPercentage);

        return true;
    }

    // GRID LAYOUT (MST expansion)
    private void GenerateGridLayout()
    {
        var occupied = new Dictionary<Vector2Int, RoomNode>();

        var start = Graph.Nodes[0];

        start.SetPosition(Vector2Int.zero);
        RegisterRoom(start, occupied);

        Queue<RoomNode> queue = new();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            foreach (var neighbor in node.MSTConnections)
            {
                if (neighbor.IsPlaced)
                    continue;

                List<Vector2Int> dirs = new(Directions);
                Shuffle(dirs);

                foreach (var dir in dirs)
                {
                    Vector2Int candidate = GetPosition(node, neighbor, dir);

                    if (!CanPlace(neighbor, candidate, occupied))
                        continue;

                    neighbor.SetPosition(candidate);
                    RegisterRoom(neighbor, occupied);

                    queue.Enqueue(neighbor);
                    break;
                }
            }
        }
    }

    // LOOP CREATION (only if grid-adjacent)
    private void AddGridLoops(float percentage)
    {
        int attempts = 200;
        int loopsToAdd = Mathf.RoundToInt(Graph.Nodes.Count * percentage);

        while (loopsToAdd > 0 && attempts-- > 0)
        {
            var a = Graph.Nodes[Random.Range(0, Graph.Nodes.Count)];
            var b = Graph.Nodes[Random.Range(0, Graph.Nodes.Count)];

            if (a == b)
                continue;

            if (a.Connections.Contains(b))
                continue;

            if (!AreAdjacent(a, b))
                continue;

            a.Connect(b);

            loopsToAdd--;
        }
    }

    // HELPERS
    private Vector2Int GetPosition(RoomNode from, RoomNode to, Vector2Int dir)
    {
        int separation = 1;

        if (dir == Vector2Int.right)
            return new Vector2Int(from.GridPosition.x + from.Size.x + separation, from.GridPosition.y);

        if (dir == Vector2Int.left)
            return new Vector2Int(from.GridPosition.x - to.Size.x - separation, from.GridPosition.y);

        if (dir == Vector2Int.up)
            return new Vector2Int(from.GridPosition.x, from.GridPosition.y + from.Size.y + separation);

        if (dir == Vector2Int.down)
            return new Vector2Int(from.GridPosition.x, from.GridPosition.y - to.Size.y - separation);

        return from.GridPosition;
    }

    private bool CanPlace(RoomNode node, Vector2Int root, Dictionary<Vector2Int, RoomNode> occupied)
    {
        for (int x = 0; x < node.Size.x; x++)
        {
            for (int y = 0; y < node.Size.y; y++)
            {
                Vector2Int cell = root + new Vector2Int(x, y);

                if (occupied.ContainsKey(cell))
                    return false;
            }
        }

        return true;
    }

    private void RegisterRoom(RoomNode node, Dictionary<Vector2Int, RoomNode> occupied)
    {
        foreach (var cell in node.GetOccupiedCells())
            occupied[cell] = node;
    }

    private bool AreAdjacent(RoomNode a, RoomNode b)
    {
        Vector2Int delta = b.GridPosition - a.GridPosition;

        int dx = Mathf.Abs(delta.x);
        int dy = Mathf.Abs(delta.y);

        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);

            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    // SPECIAL ROOMS
    private void AssignSpecialRooms()
    {
        var start = Graph.Nodes[0];
        start.Type = RoomType.Start;
        start.Size = Vector2Int.one;

        var boss = Graph.GetFarthest(start);

        boss.Type = RoomType.Boss;
        boss.Size = new Vector2Int(2, 2);
    }
}