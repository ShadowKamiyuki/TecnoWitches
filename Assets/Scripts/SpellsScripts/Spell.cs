using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Spell", menuName = "TecnoWitches/Spell")]
public class Spell : ScriptableObject
{
    public string id;
    public SpellStrategy strategy;

    public void Cast(SpellContext ctx)
    {
        strategy.Cast(ctx);
    }
}
