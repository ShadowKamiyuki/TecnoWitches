using System.Collections.Generic;
using UnityEngine;

public class RoomNode
{
    public int Id { get; }
    public List<RoomNode> Connections { get; } = new();

    public RoomType Type { get; set; } = RoomType.Normal;

    // Tamaño en celdas del grid
    public Vector2Int Size { get; set; } = Vector2Int.one;

    // Celda raíz (esquina inferior izquierda)
    public Vector2Int GridPosition { get; private set; }

    public RoomNode(int id)
    {
        Id = id;
    }

    public void Connect(RoomNode other)
    {
        if (Connections.Contains(other))
            return;

        Connections.Add(other);
        other.Connections.Add(this);
    }

    public void SetPosition(Vector2Int position)
    {
        GridPosition = position;
    }

    public IEnumerable<Vector2Int> GetOccupiedCells()
    {
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                yield return GridPosition + new Vector2Int(x, y);
    }
}