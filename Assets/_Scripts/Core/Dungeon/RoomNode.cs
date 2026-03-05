using System.Collections.Generic;
using UnityEngine;

public class RoomNode
{
    public int Id { get; }

    public List<RoomNode> Connections { get; } = new();
    public List<RoomNode> MSTConnections { get; } = new();

    public RoomType Type { get; set; } = RoomType.Normal;

    public Vector2Int Size { get; set; } = Vector2Int.one;

    public Vector2Int GridPosition { get; private set; }

    public bool IsPlaced { get; private set; }

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

    public void ConnectMST(RoomNode other)
    {
        Connect(other);

        MSTConnections.Add(other);
        other.MSTConnections.Add(this);
    }

    public void SetPosition(Vector2Int position)
    {
        GridPosition = position;
        IsPlaced = true;
    }

    public IEnumerable<Vector2Int> GetOccupiedCells()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                yield return GridPosition + new Vector2Int(x, y);
            }
        }
    }
}