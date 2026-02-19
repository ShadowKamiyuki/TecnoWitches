using System;

[Serializable]
public class SaveSlotMetadata
{
    public int SlotIndex;
    public bool HasData;
    public string LastPlayedDate;
    public int MetaLevel;
    public float TotalPlayTime;
}
