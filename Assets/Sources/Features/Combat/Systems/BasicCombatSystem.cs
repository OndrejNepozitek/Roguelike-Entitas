namespace Assets.Sources.Features.Combat.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Stats;

	[ExecutePhase(ExecutePhase.ProcessActions)]
	[DependsOn(typeof(StatsFeature))]
	public class BasicCombatSystem : ReactiveSystem<ActionsEntity>
	{
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

				action.Value = source.stats.Attack;
			}
		}
	}
}