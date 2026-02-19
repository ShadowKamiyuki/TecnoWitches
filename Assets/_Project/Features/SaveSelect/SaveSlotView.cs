using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button deleteButton;

    public event Action OnSelected;
    public event Action OnDeleted;

    private void Awake()
    {
        selectButton.onClick.AddListener(() => OnSelected?.Invoke());
        deleteButton.onClick.AddListener(() => OnDeleted?.Invoke());
    }

    public void Setup(SaveSlotMetadata metadata)
    {
        if (!metadata.HasData)
        {
            titleText.text = $"Slot {metadata.SlotIndex + 1}";
            infoText.text = "Vacío";
            deleteButton.gameObject.SetActive(false);
        }
        else
        {
            titleText.text = $"Slot {metadata.SlotIndex + 1}";
            infoText.text = $"Nivel Meta: {metadata.MetaLevel}\n" +
                            $"Tiempo: {metadata.TotalPlayTime:F1}h";
            deleteButton.gameObject.SetActive(true);
        }
    }
}
