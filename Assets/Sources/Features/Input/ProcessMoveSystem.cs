using System;
using System.Collections.Generic;
using Entitas;

public sealed class ProcessMoveSystem : ReactiveSystem<InputEntity>, IInitializeSystem
{
    InputContext inputContext;
    GameContext gameContext;
    GameEntity playerEntity;
    IGroup<GameEntity> aiEntities;

    public ProcessMoveSystem(Contexts contexts) : base(contexts.input)
    {
        inputContext = contexts.input;
        gameContext = contexts.game;
        aiEntities = gameContext.GetGroup(GameMatcher.AI);
    }

    public void Initialize()
    {
	    playerEntity = gameContext.currentPlayerEntity;
    }

    protected override void Execute(List<InputEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (!playerEntity.isActionInProgress && Map.Instance.IsWalkable(playerEntity.position.value + entity.moveInput.value))
            {
                playerEntity.isShouldAct = false;
                playerEntity.isActionInProgress = true;
                playerEntity.ReplacePosition(playerEntity.position.value + entity.moveInput.value, true);

                foreach (var e in aiEntities.GetEntities())
                {
                    e.isShouldAct = true;
                }
            }
        }
    }

    protected override bool Filter(InputEntity entity)
    {
        return true;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector(InputMatcher.MoveInput);
    }
}