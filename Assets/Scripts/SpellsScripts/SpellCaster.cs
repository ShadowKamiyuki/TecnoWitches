using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the cast of the spells without knowing the specific implementations of each spell.
/// We use methods to cast a spell, to change the spell and add new spells.
/// To cast we have the settings of the isometric cursor and a firing point from where the spell origins.
/// </summary>
public class SpellCaster : MonoBehaviour, IUpdatable
{
    [Header("Target objects")]
    [SerializeField] private IsometricCrosshair isoCursor; // target point
    [SerializeField] private Transform firePoint; // from where we shoot

    [Header("Spells Available")]
    [SerializeField] private List<SpellData> availableSpellData; // the spells the player have

    private List<RuntimeSpell> runtimeSpells;
    private RuntimeSpell currentSpell; // selected spell
    private int currentSpellIndex = 0; // index of the selected spell

    public event Action<SpellData> OnSpellChanged; // event to notify other systems (mainly UI)

    // getters for debug
    public RuntimeSpell CurrentSpell => currentSpell;
    public Vector3 CursorPosition => isoCursor != null ? isoCursor.CursorPosition : Vector3.zero;
    public Transform FirePoint => firePoint;

    private void Start()
    {
        // Crear instancias runtime
        runtimeSpells = new List<RuntimeSpell>();
        foreach (var data in availableSpellData)
            runtimeSpells.Add(new RuntimeSpell(data));

        if (runtimeSpells.Count > 0)
            SetSpell(0);
    }

    public void Tick(float deltaTime)
    {
        // we get the position of the cursor to set the direction to shoot to.
        Vector3 direction = (isoCursor.CursorPosition - firePoint.position).normalized;

        // Actualizar cooldowns y castTimers
        foreach (var spell in runtimeSpells)
        {
            spell.Tick(Time.deltaTime, new SpellContext(direction, firePoint, gameObject));
        }
    }

    public void CastSpell()
    {
        if (currentSpell == null || !currentSpell.CanCast)
            return;

        currentSpell.StartCast();
    }

    private void SetSpell(int index)
    {
        currentSpellIndex = index;
        currentSpell = runtimeSpells[index];
        OnSpellChanged?.Invoke(currentSpell.data);
    }

    public void SwitchSpell(int delta)
    {
        if (runtimeSpells.Count == 0)
            return;

        int newIndex = (currentSpellIndex + delta + runtimeSpells.Count) % runtimeSpells.Count;
        SetSpell(newIndex);
    }

    public void AddNewSpell(SpellData newData)
    {
        if (newData == null || availableSpellData.Contains(newData))
            return;

        availableSpellData.Add(newData);
        runtimeSpells.Add(new RuntimeSpell(newData));
    }

    private void OnEnable()
    {
        ServiceLocator.Get<CustomUpdateManager>().Register(this);
    }

    private void OnDisable()
    {
        CustomUpdateManager updateManager = ServiceLocator.Get<CustomUpdateManager>();

        if (updateManager != null)
        {
            updateManager.Unregister(this);
        }
    }
}
