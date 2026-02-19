using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveService : ISaveService
{
    private const int MaxSlots = 3;
    private readonly string _savePath;

    public int ActiveSlot { get; private set; }

    public SaveService()
    {
        _savePath = Application.persistentDataPath;
    }

    public IReadOnlyList<SaveSlotMetadata> GetAllSlots()
    {
        var slots = new List<SaveSlotMetadata>();

        for (int i = 0; i < MaxSlots; i++)
        {
            string path = GetSlotPath(i);
            bool exists = File.Exists(path);

            var metadata = new SaveSlotMetadata
            {
                SlotIndex = i,
                HasData = exists
            };

            if (exists)
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<SaveData>(json);

                metadata.MetaLevel = data.MetaLevel;
                metadata.TotalPlayTime = data.TotalPlayTime;
                metadata.LastPlayedDate = File.GetLastWriteTime(path).ToString();
            }

            slots.Add(metadata);
        }

        return slots;
    }

    public SaveData LoadSlot(int slotIndex)
    {
        string path = GetSlotPath(slotIndex);

        if (!File.Exists(path))
            return null;

        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void SaveSlot(int slotIndex, SaveData data)
    {
        string path = GetSlotPath(slotIndex);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public void DeleteSlot(int slotIndex)
    {
        string path = GetSlotPath(slotIndex);

        if (File.Exists(path))
            File.Delete(path);
    }

    public void CreateNewSlot(int slotIndex)
    {
        var data = new SaveData
        {
            MetaLevel = 1,
            Currency = 0,
            TotalPlayTime = 0
        };

        SaveSlot(slotIndex, data);
    }

    public void SetActiveSlot(int index)
    {
        ActiveSlot = index;
    }

    private string GetSlotPath(int index)
    {
        return Path.Combine(_savePath, $"save_{index}.json");
    }
}
