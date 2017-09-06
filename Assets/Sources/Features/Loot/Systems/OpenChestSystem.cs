namespace Assets.Sources.Features.Loot.Systems
{
	using System.Collections;
	using System.Collections.Generic;
	using Actions;
	using Entitas;
	using Helpers;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;

	[ExecutePhase(ExecutePhase.ReactToActions)]
	public class OpenChestSystem : ReactiveSystem<ActionsEntity>
	{
		public OpenChestSystem(Contexts contexts) : base(contexts.actions)
		{
		
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action.Added());
		}

		protected override bool Filter(ActionsEntity entity)
		{
			return entity.hasAction && entity.action.Action is OpenChestAction;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			foreach (var rawAction in entities)
			{
				var action = (OpenChestAction) rawAction.action.Action;
				var chest = action.Chest.GetEntity();

				if (chest == null) continue;

				chest.ReplaceHealth(-1);

				var source = action.Player.GetEntity();
				source.isActionInProgress = true;

				// TODO: handle better
				if (source.hasCoroutine)
				{
					source.RemoveCoroutine();
				}
				source.AddCoroutine(OpenChestMovement(source, (Vector3) chest.position.value), null);
			}
		}

		private IEnumerator OpenChestMovement(GameEntity source, Vector3 to)
		{
			var transform = source.view.gameObject.transform;
			var from = (Vector3)source.position.value;
			to = (to + 2 * from) / 3;

			foreach (var position in CoroutineHelpers.MoveTowardsAndBack(from, to, 0.4f))
			{
				transform.position = position;
				yield return null;
			}

			source.isActionInProgress = false;
			yield return null;
		}
	}
}