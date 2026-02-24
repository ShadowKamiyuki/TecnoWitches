using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public string AbilityName;
    public Sprite Icon;

    public abstract IAbility CreateInstance();
}