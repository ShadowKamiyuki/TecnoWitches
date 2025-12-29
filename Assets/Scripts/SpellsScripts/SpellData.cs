using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "TecnoWitches/SpellData")]
public class SpellData : ScriptableObject
{
    [Header("Spell Context")]
    public SpellContextType contextType = SpellContextType.Base; // default Base
    public bool selfCast = false; // opcional, para hechizos sobre uno mismo

    [Header("Spell Settings")]
    public Sprite icon;
    public string label;

    [Range(0.1f, 4f), SerializeField] private float castTime;
    public float CastTime => castTime;

    [SerializeField] private float cooldown;
    public float Cooldown => cooldown;

    [SerializeReference] public List<SpellEffect> effects;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(label))
            label = name;
        if (effects == null)
            effects = new List<SpellEffect>();
    }
}

public enum SpellContextType
{
    Base,        // solo caster y target
    Position,    // necesita un target point
    Directional, // necesita target point + dirección
    FirePoint    // necesita fire point + dirección + target point
}
