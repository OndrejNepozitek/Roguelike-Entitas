namespace Assets.Sources.Features.Combat.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Extensions;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Stats;
	using UnityEngine;
	using Random = System.Random;

	[ExecutePhase(ExecutePhase.ProcessActions)]
	[DependsOn(typeof(StatsFeature))]
	public class BasicCombatSystem : ReactiveSystem<ActionsEntity>
	{
		private readonly Random random = new Random();

		public BasicCombatSystem(Contexts contexts) : base(contexts.actions)
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

				// TODO: does this mean that they died?
				if (source == null || target == null)
				{
					rawAction.Destroy();
					continue;
				}

				var sourceStats = source.GetModifiedStats();
				var targetStats = target.GetModifiedStats();

				float damage = sourceStats.Attack;

				if (random.NextDouble() <= sourceStats.CriticalChance / 100f)
				{
					damage *= 1.5f;
				}

				damage = damage * (1 - targetStats.Defense / 100f);

				action.Value = damage;

				Debug.Log(damage);
			}
		}
	}
}