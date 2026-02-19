public class CharacterSelectionService
{
    private CharacterData selectedCharacter;

    // Selecciona un personaje
    public void SelectCharacter(CharacterData character)
    {
        selectedCharacter = character;
    }

    // Obtiene el personaje seleccionado
    public CharacterData GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    // Limpia la selección (opcional)
    public void ClearSelection()
    {
        selectedCharacter = null;
    }
}
