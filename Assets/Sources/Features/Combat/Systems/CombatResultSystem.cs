namespace Assets.Sources.Features.Combat.Systems
{
	using System.Collections;
	using System.Collections.Generic;
	using Entitas;
	using Helpers;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Stats;
	using UnityEngine;

	/// <summary>
	/// Apply damage from AttackAction to target entity's health.
	/// </summary>
	[ExecutePhase(ExecutePhase.ReactToActions)]
	[DependsOn(typeof(StatsFeature))]
	public class CombatResultSystem : ReactiveSystem<ActionsEntity>
	{
		public CombatResultSystem(Contexts contexts) : base(contexts.actions)
		{
		
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action);
		}

		protected override bool Filter(ActionsEntity entity)
		{
			return entity.hasAction && entity.action.Action is AttackAction;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			foreach (var rawAction in entities)
			{
				var action = (AttackAction) rawAction.action.Action;

				var source = action.Source.GetEntity();
				var target = action.Target.GetEntity();
				var to = (Vector3)target.position.value;

				target.ReplaceHealth(target.health.Value - (int)action.Value);

				source.isActionInProgress = true;

				// TODO: handle better
				if (source.hasCoroutine)
				{
					source.RemoveCoroutine();
				}
				source.AddCoroutine(AttackMovement(source, to), null);
			}
		}

		private IEnumerator AttackMovement(GameEntity source, Vector3 to)
		{
			var from = (Vector3) source.position.value;
			to = (to + 2 * from) / 3;

			foreach (var position in CoroutineHelpers.MoveTowardsAndBack(from, to, 0.4f))
			{
				source.view.gameObject.transform.position = position;
				yield return null;
			}

			source.isActionInProgress = false;
			yield return null;
		}
	}
}