public class UnitySeedGenerator : ISeedGenerator
{
    public int Generate()
    {
        return UnityEngine.Random.Range(0, int.MaxValue);
    }
}