using System.Collections.Generic;
using UnityEngine;

public class DungeonLayout
{
    private Dictionary<Vector2Int, RoomNode> _occupied = new();

    private static readonly Vector2Int[] Directions =
    {
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1)
    };

    public bool GenerateLayout(DungeonGraph graph, RoomNode startNode)
    {
        _occupied.Clear();

        startNode.SetPosition(Vector2Int.zero);
        foreach (var cell in startNode.GetOccupiedCells())
            _occupied[cell] = startNode;

        var queue = new Queue<RoomNode>();
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbor in current.Connections)
            {
                if (IsPlaced(neighbor))
                    continue;

                bool placed = false;

                for (int radius = 1; radius <= 5 && !placed; radius++)
                {
                    foreach (var dir in Directions)
                    {
                        Vector2Int candidate = current.GridPosition + dir * radius;

                        if (TryPlaceRoom(neighbor, candidate))
                        {
                            queue.Enqueue(neighbor);
                            placed = true;
                            break;
                        }
                    }
                }

                if (!placed)
                    return false; // falló el layout
            }
        }

        return true;
    }

    private bool IsPlaced(RoomNode node)
    {
        foreach (var cell in _occupied)
            if (cell.Value == node)
                return true;

        return false;
    }

    private bool TryPlaceRoom(RoomNode node, Vector2Int root)
    {
        for (int x = 0; x < node.Size.x; x++)
        {
            for (int y = 0; y < node.Size.y; y++)
            {
                Vector2Int cell = root + new Vector2Int(x, y);

                if (_occupied.ContainsKey(cell))
                    return false;
            }
        }

        node.SetPosition(root);

        foreach (var cell in node.GetOccupiedCells())
            _occupied[cell] = node;

        return true;
    }
}