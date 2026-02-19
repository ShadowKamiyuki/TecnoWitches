public class RunData
{
    public int Seed { get; private set; }
    public int Floor { get; private set; }
    public int Gold { get; private set; }

    public RunData(int seed)
    {
        Seed = seed;
        Floor = 1;
        Gold = 0;
    }

    public void AdvanceFloor()
    {
        Floor++;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }
}
