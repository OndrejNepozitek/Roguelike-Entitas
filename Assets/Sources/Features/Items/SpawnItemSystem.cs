using System.Collections.Generic;
using System.Diagnostics;
using Entitas;

/// <summary>
/// This system reacts to SpawnItem actions and spawns the item.
/// </summary>
public class SpawnItemSystem : ReactiveSystem<ActionsEntity>
{
	private readonly GameContext gameContext;

	public SpawnItemSystem(Contexts contexts) : base(contexts.actions)
	{
		gameContext = contexts.game;
	}

	protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
	{
		return context.CreateCollector(ActionsMatcher.Action.Added());
	}

	protected override bool Filter(ActionsEntity entity)
	{
		return entity.action.Action is SpawnItemAction;
	}

	protected override void Execute(List<ActionsEntity> entities)
	{
		foreach (var entity in entities)
		{
			var action = entity.action.Action as SpawnItemAction;
			Debug.Assert(action != null, "action != null");

			gameContext.CreateItem(action.Item, action.Position);
		}
	}
}