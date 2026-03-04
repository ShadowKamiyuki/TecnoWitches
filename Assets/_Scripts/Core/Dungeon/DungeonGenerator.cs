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

        // 1 Crear grafo usando tu sistema actual
        Graph = new DungeonGraph(seed);
        Graph.Generate(config.RoomCount, config.ExtraLoopPercentage);

        // 2 Asignar posiciones tipo Gungeon (grid-first placement)
        if (!AssignGridPositions())
            return false;

        // 3 Marcar Start y Boss
        AssignSpecialRooms();

        return true;
    }

    private bool AssignGridPositions()
    {
        var occupied = new Dictionary<Vector2Int, RoomNode>();
        var nodes = Graph.Nodes;

        if (nodes.Count == 0)
            return false;

        // START en (0,0)
        nodes[0].Size = Vector2Int.one;
        nodes[0].SetPosition(Vector2Int.zero);
        RegisterRoom(nodes[0], occupied);

        for (int i = 1; i < nodes.Count; i++)
        {
            var node = nodes[i];
            node.Size = Vector2Int.one; // por ahora todas 1x1, boss se ajusta después

            bool placed = false;

            for (int attempt = 0; attempt < 100; attempt++)
            {
                var baseNode = nodes[Random.Range(0, i)];
                var dir = Directions[Random.Range(0, Directions.Length)];

                Vector2Int candidate = GetPositionWithSeparation(baseNode, node, dir);

                if (CanPlace(node, candidate, occupied))
                {
                    node.SetPosition(candidate);
                    RegisterRoom(node, occupied);
                    placed = true;
                    break;
                }
            }

            if (!placed)
                return false;
        }

        return true;
    }

    private Vector2Int GetPositionWithSeparation(RoomNode from, RoomNode to, Vector2Int dir)
    {
        int separation = 1;

        if (dir == Vector2Int.right)
        {
            return new Vector2Int(
                from.GridPosition.x + from.Size.x + separation,
                from.GridPosition.y
            );
        }

        if (dir == Vector2Int.left)
        {
            return new Vector2Int(
                from.GridPosition.x - to.Size.x - separation,
                from.GridPosition.y
            );
        }

        if (dir == Vector2Int.up)
        {
            return new Vector2Int(
                from.GridPosition.x,
                from.GridPosition.y + from.Size.y + separation
            );
        }

        if (dir == Vector2Int.down)
        {
            return new Vector2Int(
                from.GridPosition.x,
                from.GridPosition.y - to.Size.y - separation
            );
        }

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

    private void AssignSpecialRooms()
    {
        var start = Graph.Nodes[0];
        start.Type = RoomType.Start;

        var boss = Graph.GetFarthest(start);
        boss.Type = RoomType.Boss;
        boss.Size = new Vector2Int(2, 2);
    }
}