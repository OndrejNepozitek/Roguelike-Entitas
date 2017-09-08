//using System;
//using System.Collections.Generic;
//using Entitas;

//public sealed class AddToQueueSystem : ReactiveSystem<GameEntity>
//{
//    GameContext context;
//    EventQueueComponent queueComponent;

//    public AddToQueueSystem(Contexts contexts) : base (contexts.game)
//    {
//        context = contexts.game;
//        queueComponent = context.eventQueue;
//    }

//    protected override void Execute(List<GameEntity> entities)
//    {
//        foreach (var entity in entities)
//        {
//            queueComponent.queue.Enqueue(entity, 0);
//        }
//    }

//    protected override bool Filter(GameEntity entity)
//    {
//        return entity.isInit && entity.isTurnBased;
//    }

//    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
//    {
//        return context.CreateCollector(GameMatcher.Init);
//    }
//}