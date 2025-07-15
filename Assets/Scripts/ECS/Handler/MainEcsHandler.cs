 
public class MainEcsHandler : EcsRunHandler
{ 
    public MainEcsHandler() : base()
    { 
    }
    public override EcsRunHandler Clone()
    {
        return new MainEcsHandler();
    }
    public override void Init() => _commonSystems?.Init(); 
    public override void Run() => _commonSystems?.Run(); 
    public override void FixedRun() => _commonSystems?.Run();
    public override void Dispose()
    { 
        _commonSystems.Destroy();
        World.Destroy();
        World = null;
    }
} 