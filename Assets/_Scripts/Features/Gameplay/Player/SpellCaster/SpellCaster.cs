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

    private List<RuntimeSpell> runtimeSpells = new();
    private RuntimeSpell currentSpell; // selected spell
    private int currentSpellIndex; // index of the selected spell

    public event Action<SpellData> OnSpellChanged; // event to notify other systems (mainly UI)

    // getters for debug
    public RuntimeSpell CurrentSpell => currentSpell;
    public Vector3 CursorPosition => isoCursor != null ? isoCursor.CursorPosition : Vector3.zero;
    public Transform FirePoint => firePoint;

    private void Start()
    {
        // Crear instancias runtime
        foreach (var data in availableSpellData)
            runtimeSpells.Add(new RuntimeSpell(data));

        if (runtimeSpells.Count > 0)
            SetSpell(0);
    }

    public void Tick(float deltaTime)
    {
        foreach (RuntimeSpell spell in runtimeSpells)
        {
            spell.Tick(deltaTime);
        }
    }

    public void CastSpell()
    {
        if (currentSpell == null || !currentSpell.CanCast)
            return;

        SpellContext context = BuildContext(currentSpell.ContextType);
        currentSpell.StartCast(context);
    }

    private SpellContext BuildContext(SpellContextType type)
    {
        Vector3 targetPoint = isoCursor.CursorPosition;
        Vector3 direction = firePoint != null ? (targetPoint - firePoint.position).normalized : Vector3.zero;

        return type switch
        {
            SpellContextType.Base => new SpellContext(gameObject),

            SpellContextType.Position => new PositionSpellContext(gameObject, targetPoint),

            SpellContextType.Directional => new DirectionalSpellContext(gameObject, targetPoint, direction),

            SpellContextType.FirePoint => new FirePointSpellContext(gameObject, firePoint, targetPoint, direction),

            _ => null
        };
    }

    private void SetSpell(int index)
    {
        if (index < 0 || index >= runtimeSpells.Count)
            return;

        currentSpellIndex = index;
        currentSpell = runtimeSpells[index];
        //OnSpellChanged?.Invoke(currentSpell.data);
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
        SetSpell(runtimeSpells.Count - 1);
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
