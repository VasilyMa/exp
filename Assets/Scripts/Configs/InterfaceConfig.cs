using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interface", menuName = "Config/Interface")]
public class InterfaceConfig : Config
{
    public Sprite AddCharacterIcon;

    public override IEnumerator Init()
    {
        yield return null;
    }
}
