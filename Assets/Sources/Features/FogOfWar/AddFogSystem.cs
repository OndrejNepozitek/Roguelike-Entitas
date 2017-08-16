namespace Assets.Sources.Features.FogOfWar
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.Entitas;
	using View;

	[SystemPhase(Phase.ReactToComponents)]
	[ExecutesAfter(typeof(AddViewSystem))]
	public sealed class AddFogSystem : ReactiveSystem<GameEntity>
	{
		public AddFogSystem(Contexts contexts) : base(contexts.game)
		{

		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				entity.isInFog = true;
				entity.view.gameObject.SetActive(false);
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasView; // Maybe apply only to specified entities?
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.View.Added());
		}
	}
}