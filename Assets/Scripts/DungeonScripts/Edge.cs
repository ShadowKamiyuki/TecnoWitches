using System;

public class Edge
{
    public Node RoomA { get; set; }
    public Node RoomB { get; set; }
    public int Weight { get; set; }
    public bool IsInMST { get; set; }

    public Edge(Node a, Node b)
    {
        RoomA = a;
        RoomB = b;
        Weight = Math.Abs(a.positionX - b.positionY) + Math.Abs(a.positionX - b.positionY); // Manhattan distance ()
    }
}
