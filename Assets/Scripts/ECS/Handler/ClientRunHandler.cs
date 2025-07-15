using Client;
using Leopotam.EcsLite.ExtendedSystems;

public class ClientRunHandler : EcsRunHandler
{
    public ClientRunHandler()
    {
        _requestSystems
            .Add(new RunRequestWrapperSystem<NetworkHealthUpdateEvent>())
            .Add(new RunRequestWrapperSystem<NetworkConfirmDamageEvent>())

            ;

        _receiveSystems
            .Add(new RunConfirmDamageUpdateSystem())
            .Add(new RunHealthUpdateSystem())

            .DelHere<ConfirmDamageUpdateEvent>()
            .DelHere<HealthUpdateEvent>()
            ;


        /*_syncUpdateSystems
            ;*/
    }

    public override EcsRunHandler Clone()
    {
        return new ClientRunHandler ();
    }
}
