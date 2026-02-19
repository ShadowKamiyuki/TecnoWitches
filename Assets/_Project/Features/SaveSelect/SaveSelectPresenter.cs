using System.Collections.Generic;

public class SaveSelectPresenter
{
    private readonly SaveSelectView _view;
    private readonly ISaveService _saveService;
    private readonly IAppStateMachine _stateMachine;

    private IReadOnlyList<SaveSlotMetadata> _slots;

    public SaveSelectPresenter(SaveSelectView view)
    {
        _view = view;
        _saveService = ServiceLocator.Get<ISaveService>();
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    public void Initialize()
    {
        _view.Show();
        RefreshSlots();
    }

    public void Dispose()
    {
        _view.Hide();
    }

    private void RefreshSlots()
    {
        _slots = _saveService.GetAllSlots();

        for (int i = 0; i < _view.Slots.Count; i++)
        {
            var slotView = _view.Slots[i];
            var metadata = _slots[i];

            slotView.Setup(metadata);

            int capturedIndex = metadata.SlotIndex;

            slotView.OnSelected += () => OnSlotSelected(capturedIndex);
            slotView.OnDeleted += () => OnSlotDeleted(capturedIndex);
        }
    }

    private void OnSlotSelected(int index)
    {
        var metadata = _slots[index];

        if (!metadata.HasData)
            _saveService.CreateNewSlot(index);

        _saveService.SetActiveSlot(index);

        var request = new LoadingRequest(
            load: new[] { "CharacterSelect" },
            unload: new[] { "SaveSelect" },
            nextState: AppState.CharacterSelect
        );

        _stateMachine.RequestSceneChange(request);
    }

    private void OnSlotDeleted(int index)
    {
        _saveService.DeleteSlot(index);
        RefreshSlots();
    }
}
