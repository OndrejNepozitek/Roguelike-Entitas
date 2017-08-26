namespace Assets.Sources.Features.Loot.Systems
{
	using System.Collections.Generic;
	using Actions;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

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
			}
		}
	}
}