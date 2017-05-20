using System;
using System.Collections.Generic;
using Entitas;

public sealed class ProcessMoveSystem : ReactiveSystem<InputEntity>, IInitializeSystem
{
    InputContext inputContext;
    GameContext gameContext;
    GameEntity playerEntity;

    public ProcessMoveSystem(Contexts contexts) : base(contexts.input)
    {
        inputContext = contexts.input;
        gameContext = contexts.game;
    }

    public void Initialize()
    {
        playerEntity = gameContext.playerEntity;
    }

    protected override void Execute(List<InputEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (playerEntity.isShouldAct)
            {
                playerEntity.isShouldAct = false;
                playerEntity.isActionInProgress = true;
                playerEntity.ReplaceSmoothMovement(playerEntity.position.value + entity.moveInput.value, 0.5f);
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