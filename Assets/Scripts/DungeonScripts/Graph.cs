using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph
{
    public List<Node> Rooms { get; private set; }
    public List<Edge> Edges { get; private set; }

    public Graph()
    {
        Rooms = new List<Node>();
        Edges = new List<Edge>();
    }

    public void AddRoom(Node room)
    {
        Rooms.Add(room);
    }

    public void AddEdges()
    {
        Edges = new List<Edge>();

        for (int i = 0; i < Rooms.Count; i++)
        {
            for (int j = i + 1; j < Rooms.Count; j++)
            {
                Edge edge = new Edge(Rooms[i], Rooms[j]);
                Edges.Add(edge);

                Rooms[i].Edges.Add(edge);
                Rooms[j].Edges.Add(edge);
            }
        }

        Edges = Edges.OrderBy(e => e.Weight).ToList();
    }

    public List<Edge> GenerateMST()
    {
        UnionFind uf = new UnionFind(Rooms.Count);
        List<Edge> mst = new List<Edge>();

        foreach (var edge in Edges)
        {
            int a = edge.RoomA.id;
            int b = edge.RoomB.id;

            if (uf.Find(a) != uf.Find(b))
            {
                uf.Union(a, b);
                edge.IsInMST = true;
                mst.Add(edge);
            }
        }

        return mst;
    }

    // Genera loops adicionales sobre el MST
    public void AddLoops(int extraLoops, int bestPossibleLoops)
    {
        // Lista de aristas que no forman parte del MST
        List<Edge> possibleLoops = Edges.Where(e => !e.IsInMST).OrderBy(e => e.Weight).ToList();

        // Selecciona aleatoriamente algunas aristas para crear loops
        for (int i = 0; i < extraLoops && i < possibleLoops.Count; i++)
        {
            Edge chosen = possibleLoops[Random.Range(0, Mathf.Min(possibleLoops.Count, bestPossibleLoops))];
            chosen.IsInMST = true;
            possibleLoops.Remove(chosen);
        }
    }
}
