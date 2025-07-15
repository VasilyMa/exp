using Client;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using System.Collections.Generic;

public abstract class EcsRunHandler
{
    public EcsWorld World;
    public bool IsRun;
    /// <summary>
    /// Init systems use local on all clients
    /// </summary>
    protected EcsSystems _initSystems;
    /// <summary>
    /// Common systems is update local systems
    /// </summary>
    protected EcsSystems _commonSystems;
    /// <summary>
    /// Request systems is get from network event
    /// </summary>
    protected EcsSystems _requestSystems;
    /// <summary>
    /// Send systems is events from local to other or host
    /// </summary>
    protected EcsSystems _sendSystems;
    /// <summary>
    /// Receive systems is events from request 
    /// </summary>
    protected EcsSystems _receiveSystems;
    /// <summary>
    /// Sync update systems is update local on client with data from receive systems
    /// </summary>
    protected EcsSystems _syncUpdateSystems;
    /// <summary>
    /// Dispose any dispose systems for dispose components from entity it's not destroy entity only clean are components
    /// </summary>
    protected EcsSystems _disposeSystems;
    protected EcsData _data;
    protected List<EcsSystems> _allSystems;
    public EcsRunHandler()
    {
        World = new Leopotam.EcsLite.EcsWorld();
        _data = new EcsData();
        
        _initSystems = new EcsSystems(World, _data);
        _commonSystems = new EcsSystems(World, _data); 
        _requestSystems = new EcsSystems(World, _data);
        _syncUpdateSystems = new EcsSystems(World, _data);
        _receiveSystems = new EcsSystems(World, _data);
        _sendSystems = new EcsSystems(World, _data);
        _disposeSystems = new EcsSystems(World, _data);

        _initSystems
            .Add(new InitCameraSystem())
            ;

        _commonSystems
            .Add(new RunInputMovementSystem())
            .Add(new RunInputAimSystem())
            .Add(new RunAimDirectionSystem())
            .Add(new RunSelectTargetSystem())
            .Add(new RunShootSystem())
            .Add(new RunPlayerMovementSystem())
            .Add(new RunNPCMovementSystem())
            .Add(new RunCameraSwitchSystem())
            .DelHere<DirectionComponent>()
            .DelHere<AimDirectionComponent>()
            ;

        //_sendSystems ToDo this place to send systems

        _requestSystems
            .Add(new RunRequestWrapperSystem<NetworkUnitEntitySpawnEvent>())
            .Add(new RunRequestWrapperSystem<NetworkTransformEvent>())
            ;

        _receiveSystems 
            .Add(new RunReceiveEntitySpawnSystem())
            .Add(new RunReceiveTransformSystem())
            .DelHere<ReceiveSpawnEvent>()
            .DelHere<ReceiveTransformEvent>()
            ;

        _syncUpdateSystems
            .Add(new RunLerpNetworkTransformSystem())
            ;

#if UNITY_EDITOR
        _commonSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
        _allSystems = new List<EcsSystems>()
        {
            _initSystems, 
            _commonSystems,
            _requestSystems,
            _receiveSystems,
            _syncUpdateSystems,
            _sendSystems, 
            _disposeSystems
        };
    }
    public abstract EcsRunHandler Clone();

    public virtual void Init()
    {
        for (int i = 0; i < _allSystems.Count; i++)
        {
            var systems = _allSystems[i];
            systems.Inject(); 
            systems.Init();
        }
    }

    public virtual void Run()
    {
        for (int i = 0; i < _allSystems.Count; i++)
        {
            _allSystems[i].Run();
        } 
    }

    public virtual void FixedRun()
    {

    }

    public virtual void Dispose()
    {
        _allSystems.ForEach(_x => _x.Destroy());
        World.Destroy();
        World = null;
    } 
}

public class EcsData
{

}