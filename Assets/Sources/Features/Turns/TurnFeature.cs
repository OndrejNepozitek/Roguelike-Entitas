//using Entitas;

//public sealed class TurnFeature : Feature
//{
//    public TurnFeature(Contexts contexts) : base("Turn Systems")
//    {
//        Add(new AddToQueueSystem(contexts));
//        Add(new RemoveFromQueue(contexts));         // Must be before Schedule to remove disabled entities
//        Add(new ScheduleActionSystem(contexts));    
//        Add(new UpdateQueueSystem(contexts));
//    }
//}