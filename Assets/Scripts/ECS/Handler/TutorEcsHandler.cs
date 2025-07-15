using Client;

public class TutorEcsHandler : EcsRunHandler
{
    public TutorEcsHandler() 
    { 
    } 

    public override EcsRunHandler Clone()
    {
        return new TutorEcsHandler();
    }
}
