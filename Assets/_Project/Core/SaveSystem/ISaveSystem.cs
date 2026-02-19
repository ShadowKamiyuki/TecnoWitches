using System.Collections.Generic;

public interface ISaveService
{
    public int ActiveSlot { get; }

    IReadOnlyList<SaveSlotMetadata> GetAllSlots();

    SaveData LoadSlot(int slotIndex);

    void SaveSlot(int slotIndex, SaveData data);
    void DeleteSlot(int slotIndex);
    void CreateNewSlot(int slotIndex);
    void SetActiveSlot(int index);
}
