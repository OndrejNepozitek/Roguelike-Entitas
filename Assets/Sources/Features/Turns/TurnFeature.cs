using Entitas;

public sealed class TurnFeature : Feature
{
    public TurnFeature(Contexts contexts) : base("Turn Systems")
    {
        Add(new AddToQueueSystem(contexts));
        Add(new UpdateQueueSystem(contexts));
        Add(new ScheduleActionSystem(contexts));
    }
}