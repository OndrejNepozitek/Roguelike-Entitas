namespace Assets.Sources.Features.FogOfWar.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using View;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	[ExecutesAfter(typeof(RevealSystem), typeof(AddViewSystem))]
	public sealed class AddOrRemoveFogSystem : ReactiveSystem<GameEntity>
	{
		public AddOrRemoveFogSystem(Contexts contexts) : base(contexts.game)
		{

		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				entity.view.gameObject.SetActive(!entity.isInFog);
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasView;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.InFog.AddedOrRemoved());
		}
	}
}