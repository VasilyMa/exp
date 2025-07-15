using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "EcsSetupConfig", menuName = "Config/EcsSetup")]
public class EcsSetupConfig : Config
{
    [SerializeReference] public EcsRunHandler EcsHandler; 

    private EcsRunHandler _ecsRunHandler;

    public override IEnumerator Init()
    {

        yield return null;
    }

    public void InitMainEcsHandler()
    {
        _ecsRunHandler = EcsHandler.Clone();
    }

    public EcsRunHandler GetMainEcsHandler()
    {
        return _ecsRunHandler; 
    } 
}
