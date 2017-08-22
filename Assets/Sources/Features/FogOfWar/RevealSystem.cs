

namespace Assets.Sources.Features.FogOfWar
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers;
	using Helpers.Map;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	/// <summary>
	/// This system should reveal all entities that are near a light.
	/// 
	/// TODO: Current implementation is pretty dump.
	/// </summary>
	[ExecutePhase(ExecutePhase.ReactToComponents)]
	public sealed class RevealSystem : ReactiveSystem<GameEntity>
	{
		private readonly IGroup<GameEntity> isLightGroup;
		private readonly GameContext gameContext;

		public RevealSystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
			isLightGroup = contexts.game.GetGroup(GameMatcher.RevealAround);
		}

		protected override void Execute(List<GameEntity> entities)
		{
			// This should be smarter
			foreach (var lightEntity in isLightGroup.GetEntities())
			{
				var mapEntities =
					gameContext.GetService<EntityMap>().GetRhombWithoutCorners(lightEntity.position.value, lightEntity.revealAround.radius);

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