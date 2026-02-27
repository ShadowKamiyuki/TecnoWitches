using System.Collections.Generic;

public class Stat
{
    private float _baseValue;
    private readonly List<StatModifier> _modifiers = new();

    public Stat(float baseValue)
    {
        _baseValue = baseValue;
    }

    public void SetBaseValue(float value)
    {
        _baseValue = value;
    }

    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        _modifiers.Remove(modifier);
    }

    public float Value
    {
        get
        {
            float finalValue = _baseValue;

            // Flat primero
            foreach (var mod in _modifiers)
                if (mod.Type == ModifierType.Flat)
                    finalValue += mod.Value;

            // Percent después
            foreach (var mod in _modifiers)
                if (mod.Type == ModifierType.Percent)
                    finalValue *= (1 + mod.Value);

            return finalValue;
        }
    }
}