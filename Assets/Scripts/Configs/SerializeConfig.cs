using System.Collections;

using MemoryPack;

using UnityEngine;

[CreateAssetMenu(fileName = "SerializeConfig", menuName = "Config/SerializeConfig")]
public class SerializeConfig : Config
{
    public override IEnumerator Init()
    {
        yield return null;
    }
} 