

namespace Assets.Sources.Features.FogOfWar
{
	using System.Collections.Generic;
	using System.Linq;
	using Entitas;
	using Helpers;
	using Helpers.Map;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	/// <summary>
	/// This system should reveal all entities that are near a light.
	/// 
	/// TODO: Current implementation is pretty dumb.
	/// </summary>
	[ExecutePhase(ExecutePhase.ReactToComponents)]
	public sealed class RevealSystem : ReactiveSystem<GameEntity>, IInitializeSystem
	{
		private readonly IGroup<GameEntity> isLightGroup;
		private readonly GameContext gameContext;
		private EntityMap map;

		public RevealSystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
			isLightGroup = contexts.game.GetGroup(GameMatcher.RevealAround);
		}

		public void Initialize()
		{
			map = gameContext.GetService<EntityMap>();
		}

		protected override void Execute(List<GameEntity> entities)
		{
			// This should be smarter
			// Code below reveals entities that are close to entities with RevealAround
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

			foreach (var entity in entities)
			{
				var floor = map.GetEntitiesOnTile(entity.position.value).FirstOrDefault(x => x.isFloor);
				if (floor != null && entity.isInFog != floor.isInFog)
				{
					entity.isInFog = floor.isInFog;
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return true;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Position.Added());
		}
	}
}