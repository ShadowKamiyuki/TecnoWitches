using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TecnoWitches/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [SerializeField] private List<CharacterData> characters;

    public IReadOnlyList<CharacterData> Characters => characters;

    public CharacterData GetById(string id)
    {
        return characters.Find(character => character.Id == id);
    }
}