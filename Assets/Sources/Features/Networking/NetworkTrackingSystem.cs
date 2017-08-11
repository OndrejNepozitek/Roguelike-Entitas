using System;
using System.Collections.Generic;
using Entitas;

public class NetworkTrackingSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{

	public NetworkTrackingSystem(Contexts contexts) : base(contexts.game)
	{

	}

	public void Initialize()
	{
		EntityDatabase.Instance.Reset();
	}

	protected override void Execute(List<GameEntity> entities)
	{
		foreach (var entity in entities)
		{
			if (entity.networkTracked.Reference == null)
			{
				var id = EntityDatabase.Instance.AddEntity(entity);
				entity.networkTracked.Reference = new EntityReference() {Id = id};
			}
			else
			{
				var reference = entity.networkTracked.Reference;
				EntityDatabase.Instance.AddEntity(reference.Id, entity);
			}

			entity.OnDestroyEntity += Entity_OnDestroy;
		}
	}

	private static void Entity_OnDestroy(IEntity entity)
	{
		var gameEntity = entity as GameEntity;
		EntityDatabase.Instance.RemoveEntity(gameEntity.networkTracked.Reference.Id);
	}

	protected override bool Filter(GameEntity entity)
	{
		return true;
	}

	protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.NetworkTracked.Added());
	}
}