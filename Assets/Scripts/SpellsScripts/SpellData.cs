using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "TecnoWitches/SpellData")]
public class SpellData : ScriptableObject
{
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
