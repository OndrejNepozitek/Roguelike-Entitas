namespace Assets.Sources.Features.Lights
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.Map;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	public class SetLightsSystem : ReactiveSystem<GameEntity>, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private readonly IGroup<GameEntity> inLightGroup;
		private readonly IGroup<GameEntity> isLightGroup;
		private EntityMap map;

		public SetLightsSystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
			inLightGroup = gameContext.GetGroup(GameMatcher.InLight);
			isLightGroup = gameContext.GetGroup(GameMatcher.Light);
		}

		public void Initialize()
		{
			map = gameContext.GetService<EntityMap>();
		}

		protected override void Execute(List<GameEntity> entities)
		{
			// TODO: Should be optimized later
			foreach (var le in inLightGroup.GetEntities())
			{
				le.RemoveInLight();
			}

			foreach (var entity in isLightGroup.GetEntities())
			{
				if (entity.hasLight)
				{
					EditNearbyLights(entity);
				}
				else
				{
					var floor = map.TileHasAny(entity.position.value, e => e.isFloor);

					if (entity.hasInLight)
					{
						entity.ReplaceInLight(floor.inLight.value);
					}
					else
					{
						entity.AddInLight(floor.inLight.value);
					}
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return true;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Position.Added(), GameMatcher.Light.Added());
		}

		private void EditNearbyLights(GameEntity entity)
		{
			var pos = entity.position.value;
			var entitiesToChange = map.GetRhombWithoutCorners(pos, entity.light.radius);

			foreach (var le in entitiesToChange)
			{
				var distance = IntVector2.MaxDistance(pos, le.position.value);
				if (distance == 0) distance = 1;
				var newVal = 100 - distance * 10;
            
				if (le.hasInLight)
				{
					if (le.inLight.value < newVal)
					{
						le.inLight.value = newVal;
					}
				} else
				{
					le.AddInLight(newVal);
				}
			}
		}
	}
}