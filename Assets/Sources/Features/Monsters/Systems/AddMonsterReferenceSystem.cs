namespace Assets.Sources.Features.Monsters.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[ExecutePhase(ExecutePhase.ProcessActions)]
	public class AddMonsterReferenceSystem : ReactiveSystem<ActionsEntity>
	{
		public AddMonsterReferenceSystem(Contexts contexts) : base(contexts.actions)
		{
		
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action.Added());
		}

		protected override bool Filter(ActionsEntity entity)
		{
			return entity.hasAction && entity.action.Action is SpawnMonsterAction;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			foreach (var entity in entities)
			{
				var action = entity.action.Action as SpawnMonsterAction;
				if (action.Entity == null)
				{
					var id = EntityDatabase.Instance.GetNextId();
					action.Entity = new EntityReference(id);
				}
			}
		}
	}
}