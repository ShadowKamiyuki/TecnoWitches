using UnityEngine;

public class DungeonGenerator
{
    public DungeonGraph Graph { get; private set; }

    private System.Random _random;

    public bool Generate(IDungeonGenerationConfig config, int seed)
    {
        //int seed = config.UseRandomSeed
        //    ? Random.Range(int.MinValue, int.MaxValue)
        //    : config.FixedSeed;

        _random = new System.Random(seed);

        Graph = new DungeonGraph(seed);
        Graph.Generate(config.RoomCount, config.ExtraLoopPercentage);

        // Elegir start aleatorio
        var start = Graph.Nodes[_random.Next(Graph.Nodes.Count)];
        start.Type = RoomType.Start;

        // Elegir boss lejano lógicamente
        var boss = Graph.GetFarthest(start);
        boss.Type = RoomType.Boss;

        AssignRoomSizes(config);

        var layout = new DungeonLayout();

        bool success = layout.GenerateLayout(Graph, start);

        return success;
    }

    private void AssignRoomSizes(IDungeonGenerationConfig config)
    {
        foreach (var node in Graph.Nodes)
        {
            switch (node.Type)
            {
                case RoomType.Start:
                case RoomType.Normal:
                    node.Size = Vector2Int.one;
                    break;

                case RoomType.Boss:
                    node.Size = new Vector2Int(2, 2); // por ahora hardcoded
                    break;

                case RoomType.Treasure:
                case RoomType.Shop:
                case RoomType.Elite:
                case RoomType.Secret:
                    node.Size = Vector2Int.one;
                    break;

                default:
                    node.Size = Vector2Int.one;
                    break;
            }
        }
    }
}