using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterButtonView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject lockedOverlay;

    public event Action OnSelected;

    private void Awake()
    {
        selectButton.onClick.AddListener(() => OnSelected?.Invoke());
    }

    public void Setup(string name, bool unlocked)
    {
        nameText.text = name;
        lockedOverlay.SetActive(!unlocked);
        selectButton.interactable = unlocked;
    }
}
