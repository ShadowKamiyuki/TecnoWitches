public class RunStartRequest
{
    public string CharacterID { get; }

    public RunStartRequest(string characterID)
    {
        CharacterID = characterID;
    }
}