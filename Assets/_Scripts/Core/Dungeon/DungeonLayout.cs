using System.Collections.Generic;
using UnityEngine;

public class DungeonLayout
{
    private Dictionary<Vector2Int, RoomNode> _occupied = new();
    private HashSet<RoomNode> _placedNodes = new();

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
        _placedNodes.Clear();

        // Colocar start en (0,0)
        startNode.SetPosition(Vector2Int.zero);
        RegisterRoom(startNode);

        var queue = new Queue<RoomNode>();
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbor in current.Connections)
            {
                if (_placedNodes.Contains(neighbor))
                    continue;

                bool placed = false;

                foreach (var dir in Directions)
                {
                    Vector2Int baseCandidate = GetCandidatePosition(current, neighbor, dir);

                    // Intentar pequeños offsets verticales u horizontales
                    for (int offset = -2; offset <= 2; offset++)
                    {
                        Vector2Int candidate = baseCandidate;

                        if (dir.x != 0)
                            candidate.y += offset;
                        else
                            candidate.x += offset;

                        if (TryPlaceRoom(neighbor, candidate))
                        {
                            queue.Enqueue(neighbor);
                            placed = true;
                            break;
                        }
                    }

                    if (placed)
                        break;
                }

                if (!placed)
                {
                    Debug.LogError($"Failed placing room {neighbor.Type}");
                    return false;
                }
            }
        }

        return true;
    }

    private Vector2Int GetCandidatePosition(RoomNode current, RoomNode neighbor, Vector2Int dir)
    {
        Vector2Int currentPos = current.GridPosition;

        if (dir == Vector2Int.right)
        {
            return new Vector2Int(
                currentPos.x + current.Size.x + 1,
                currentPos.y
            );
        }

        if (dir == Vector2Int.left)
        {
            return new Vector2Int(
                currentPos.x - neighbor.Size.x - 1,
                currentPos.y
            );
        }

        if (dir == Vector2Int.up)
        {
            return new Vector2Int(
                currentPos.x,
                currentPos.y + current.Size.y + 1
            );
        }

        if (dir == Vector2Int.down)
        {
            return new Vector2Int(
                currentPos.x,
                currentPos.y - neighbor.Size.y - 1
            );
        }

        return currentPos;
    }

    private bool TryPlaceRoom(RoomNode node, Vector2Int root)
    {
        // Verificar colisiones
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
        RegisterRoom(node);

        return true;
    }

    private void RegisterRoom(RoomNode node)
    {
        foreach (var cell in node.GetOccupiedCells())
        {
            _occupied[cell] = node;
        }

        _placedNodes.Add(node);
    }
}