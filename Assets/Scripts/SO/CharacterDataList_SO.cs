using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataList_SO", menuName = "Custom/CharacterDataList_SO", order = 1)]
public class CharacterDataList_SO : ScriptableObject
{
    public Character[] characters;
    public Character GetCharByIndex(int index)
    {
        return characters[index];
    }
}
