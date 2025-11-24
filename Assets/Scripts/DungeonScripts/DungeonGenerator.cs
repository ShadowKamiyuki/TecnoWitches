using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Settings")]
    [Tooltip("Maximum number of rooms for the dungeon (including special rooms)")]
    [SerializeField] private int maxRooms;
    [Tooltip("How many extra loops we want for backtracking")]
    [SerializeField] private int extraLoops;
    [Tooltip("How many edges we want to take into account to make loops")]
    [SerializeField] private int bestPossibleLoops;

    private void Start()
    {
        GenerateDungeonGraph();
    }

    private void GenerateDungeonGraph()
    {
        Graph graph = new Graph();

        // generate rooms
        for (int i = 0; i < maxRooms; i++)
        {
            int x = Random.Range(0, 100);
            int y = Random.Range(0, 100);

            Node room = new Node(i, x, y);
            graph.AddRoom(room);
        }

        graph.AddEdges();
        List<Edge> mst = graph.GenerateMST();
        graph.AddLoops(extraLoops, bestPossibleLoops);

        Debug.Log("MST generado con " + mst.Count + " conexiones.");

        Debug.Log("Conexiones del MST:");
        foreach (var e in mst)
        {
            Debug.Log($"Room {e.RoomA.id} <-> Room {e.RoomB.id}  weight={e.Weight}");
        }
    }
}
