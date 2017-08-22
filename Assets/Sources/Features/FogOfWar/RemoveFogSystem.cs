namespace Assets.Sources.Features.FogOfWar
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	[ExecutesAfter(typeof(RevealSystem))]
	public sealed class RemoveFogSystem : ReactiveSystem<GameEntity>
	{
		public RemoveFogSystem(Contexts contexts) : base(contexts.game)
		{

		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				entity.view.gameObject.SetActive(true);
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasView;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.InFog.Removed());
		}
	}
}