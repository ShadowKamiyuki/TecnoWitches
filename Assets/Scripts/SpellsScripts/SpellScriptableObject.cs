using UnityEngine;

public abstract class SpellScriptableObject : ScriptableObject, ISpell
{
    [Header("General Settings")]
    public string spellName;
    public float cooldown = 0.2f;
    public Sprite icon;

    [TextArea]
    public string description;

    private float lastCastTime;

    public abstract void Cast(SpellContext context);

    public bool CanCast() => Time.time >= lastCastTime + cooldown;

    protected void RegisterCast() => lastCastTime = Time.time;

    public string GetName() => spellName;
}
