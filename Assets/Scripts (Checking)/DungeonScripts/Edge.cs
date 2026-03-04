using System;

public class Edge
{
    public RoomNode RoomA { get; set; }
    public RoomNode RoomB { get; set; }
    public int Weight { get; set; }
    public bool IsInMST { get; set; }

    public Edge(RoomNode a, RoomNode b)
    {
        RoomA = a;
        RoomB = b;
        //Weight = Math.Abs(a.positionX - b.positionY) + Math.Abs(a.positionX - b.positionY); // Manhattan distance ()
    }
}
