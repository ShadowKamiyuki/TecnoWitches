using System.Collections.Generic;

public class DungeonGraph
{
    public List<RoomNode> Nodes { get; } = new();

    private System.Random _random;

    public DungeonGraph(int seed)
    {
        _random = new System.Random(seed);
    }

    public void Generate(int roomCount, float extraLoopPercentage)
    {
        CreateNodes(roomCount);
        GenerateMinimumSpanningTree();
        AddExtraLoops(extraLoopPercentage);
    }

    private void CreateNodes(int count)
    {
        Nodes.Clear();

        for (int i = 0; i < count; i++)
            Nodes.Add(new RoomNode(i));
    }

    private void GenerateMinimumSpanningTree()
    {
        var connected = new List<RoomNode>();
        var remaining = new List<RoomNode>(Nodes);

        var start = remaining[_random.Next(remaining.Count)];
        connected.Add(start);
        remaining.Remove(start);

        while (remaining.Count > 0)
        {
            var from = connected[_random.Next(connected.Count)];
            var to = remaining[_random.Next(remaining.Count)];

            from.Connect(to);

            connected.Add(to);
            remaining.Remove(to);
        }
    }

    private void AddExtraLoops(float percentage)
    {
        int loopCount = (int)(Nodes.Count * percentage);
        int attempts = 0;

        while (loopCount > 0 && attempts < 500)
        {
            var a = Nodes[_random.Next(Nodes.Count)];
            var b = Nodes[_random.Next(Nodes.Count)];

            if (a == b || a.Connections.Contains(b))
            {
                attempts++;
                continue;
            }

            var distances = BFS(a);

            // Evita ciclos cortos (triángulos)
            if (!distances.ContainsKey(b) || distances[b] < 3)
            {
                attempts++;
                continue;
            }

            a.Connect(b);
            loopCount--;
        }
    }

    public Dictionary<RoomNode, int> BFS(RoomNode start)
    {
        var distances = new Dictionary<RoomNode, int>();
        var queue = new Queue<RoomNode>();

        distances[start] = 0;
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbor in current.Connections)
            {
                if (distances.ContainsKey(neighbor))
                    continue;

                distances[neighbor] = distances[current] + 1;
                queue.Enqueue(neighbor);
            }
        }

        return distances;
    }

    public RoomNode GetFarthest(RoomNode start)
    {
        var distances = BFS(start);

        RoomNode farthest = start;
        int max = 0;

        foreach (var pair in distances)
        {
            if (pair.Value > max)
            {
                max = pair.Value;
                farthest = pair.Key;
            }
        }

        return farthest;
    }
}