namespace Assets.Sources.Features.Movement
{
	using System.Collections.Generic;
	using Actions;
	using Coroutines;
	using Entitas;
	using Helpers;
	using Helpers.Entitas;
	using Helpers.Map;
	using MapTracker;
	using View;
	using Debug = UnityEngine.Debug;

	/// <summary>
	/// Consumes movement actions and makes entites move.
	/// </summary>
	[SystemPhase(Phase.ReactToActions)]
	[DependsOn(typeof(CoroutinesFeature), typeof(ActionsFeature), typeof(ViewFeature), typeof(MapTrackerSystem))]
	public class ProcessBasicMoveSystem : ReactiveSystem<ActionsEntity>
	{
		public ProcessBasicMoveSystem(Contexts contexts) : base(contexts.actions)
		{
		
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action.Added());
		}

		protected override bool Filter(ActionsEntity entity)
		{
			if (!entity.hasAction)
			{
				return false; // TODO: why is this needed?
			}

			var action = entity.action.Action as BasicMoveAction;

			if (action == null) return false;

			var targetEntity = action.Entity.GetEntity();

			return targetEntity.hasPosition && targetEntity.hasView; // && !targetEntity.isActionInProgress;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			foreach (var actionEntity in entities)
			{
				var moveAction = actionEntity.action.Action as BasicMoveAction;
				Debug.Assert(moveAction != null, "moveAction != null");

				// TODO: this is dangerous a hard to debug if not done correctly
				// Position of entity is changes after the validation so it happened
				// that two or more entities moved onto the same tile
				if (!EntityMap.Instance.IsWalkable(moveAction.Position))
				{
					actionEntity.Destroy();
					Debug.Log("Destroying move action");
					continue;
				}

				Debug.Log("Moving entity to " + moveAction.Position);

				var entity = moveAction.Entity.GetEntity();
				entity.isActionInProgress = true;
				entity.ReplacePosition(moveAction.Position, true);
			}
		}
	}
}