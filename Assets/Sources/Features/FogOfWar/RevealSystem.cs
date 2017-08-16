

namespace Assets.Sources.Features.FogOfWar
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.Entitas;

	/// <summary>
	/// This system should reveal all entities that are near a light.
	/// 
	/// TODO: Current implementation is pretty dump.
	/// </summary>
	[SystemPhase(Phase.ReactToComponents)]
	public sealed class RevealSystem : ReactiveSystem<GameEntity>
	{
		private readonly IGroup<GameEntity> isLightGroup;

		public RevealSystem(Contexts contexts) : base(contexts.game)
		{
			isLightGroup = contexts.game.GetGroup(GameMatcher.RevealAround);
		}

		protected override void Execute(List<GameEntity> entities)
		{
			// This should be smarter
			foreach (var lightEntity in isLightGroup.GetEntities())
			{
				var mapEntities =
					Map.Instance.GetRhombWithoutCorners(lightEntity.position.value, lightEntity.revealAround.radius);

				foreach (var revealEntity in mapEntities)
				{
					if (revealEntity.isInFog)
					{
						revealEntity.isInFog = false;
					}
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return true; // entity.hasRevealAround;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Position.Added());
		}
	}
}