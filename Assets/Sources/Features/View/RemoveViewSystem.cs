using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public sealed class RemoveViewSystem : ReactiveSystem<GameEntity>, ICleanupSystem, ITearDownSystem
{
    readonly GameContext context;
	private readonly IGroup<GameEntity> shouldBeDestroyed;

    public RemoveViewSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
	    shouldBeDestroyed = context.GetGroup(GameMatcher.ShouldBeDestroyed);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
	        entity.isShouldBeDestroyed = true;
        }
    }

    private void RemoveView(GameEntity entity)
    {
	    foreach (Transform transform in entity.view.gameObject.transform)
	    {
			UnityEngine.Object.Destroy(transform.gameObject);
	    }

	    entity.view.gameObject.Unlink();
	    UnityEngine.Object.Destroy(entity.view.gameObject);

		entity.RemoveView();
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        /*return new Collector<GameEntity>(
            new[] {
                context.GetGroup(GameMatcher.Asset),
                context.GetGroup(GameMatcher.Destroyed),
                context.GetGroup(GameMatcher.Dead)
            },
            new[] {
                GroupEvent.Removed,
                GroupEvent.Added,
                GroupEvent.Added
            }
        );*/
	    return context.CreateCollector(GameMatcher.Asset.Removed(), GameMatcher.Destroyed.Added(),
		    GameMatcher.Dead.Added());
    }

	public void Cleanup()
	{
		foreach (var entity in shouldBeDestroyed.GetEntities())
		{
			RemoveView(entity);
			entity.Destroy();
		}
	}

	public void TearDown()
	{
		foreach (var entity in context.GetEntities())
		{
			if (entity.hasView)
			{
				RemoveView(entity);
				entity.Destroy();
			}
		}
	}
}