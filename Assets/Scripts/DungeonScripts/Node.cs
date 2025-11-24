using System.Collections.Generic;

public class Node
{
    public int id { get; set; }
    public int positionX { get; set; }
    public int positionY { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    // usar un enumerator para los tipos de sala a futuro (esto solo para testing)
    public bool IsStart { get; set; }
    public List<Edge> Edges { get; set; }

    public Node(int id, int x, int y)
    {
        this.id = id;
        positionX = x;
        positionY = y;
        Edges = new List<Edge>();
    }
}
