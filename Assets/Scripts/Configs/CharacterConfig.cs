using System.Collections;
using System.Collections.Generic;

using UnityEngine;
[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/Character")]
public class CharacterConfig : Config
{
    public List<CharacterBase> _characters;

    private Dictionary<string, CharacterBase> _characterConfigs;

    public override IEnumerator Init()
    {
        _characterConfigs = new Dictionary<string, CharacterBase> ();
        foreach (var character in _characters)
        {
            Debug.Log (character.KEY_ID + "Character");
            _characterConfigs.Add(character.KEY_ID, character);
        }

        yield return null;
    }

    public bool TryGetCharacter(string key, out CharacterBase character)
    {
        if (_characterConfigs.ContainsKey(key))
        {
            character = _characterConfigs[key];
            return true;
        }

        character = null;
        return false;
    }
}