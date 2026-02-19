using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int MetaLevel;
    public int Currency;
    public float TotalPlayTime;
    public List<string> UnlockedCharacters = new();
}
