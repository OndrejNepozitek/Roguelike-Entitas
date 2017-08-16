namespace Assets.Sources.Features.Items
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using Actions;
	using Entitas;
	using Helpers.Entitas;

	/// <summary>
	/// This system reacts to SpawnItem actions and spawns the item.
	/// </summary>
	[SystemPhase(Phase.ReactToActions)]
	[DependsOn(typeof(ActionsFeature))]
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
			return entity.hasAction && entity.action.Action is SpawnItemAction;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			foreach (var entity in entities)
			{
				if (entity.hasAction)
				{
					var action = entity.action.Action as SpawnItemAction;
					Debug.Assert(action != null, "action != null");

					gameContext.CreateItem(action.Item, action.Position);
				}

			}
		}
	}
}