public class RunData
{
    public int Seed { get; }
    public int Floor { get; private set; }
    public string CharacterID { get; }

    public RunData(int seed, string characterID)
    {
        Seed = seed;
        CharacterID = characterID;
        Floor = 1;
    }

    public void AdvanceFloor()
    {
        Floor++;
    }
}