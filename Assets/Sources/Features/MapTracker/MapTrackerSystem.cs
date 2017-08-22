namespace Assets.Sources.Features.MapTracker
{
	using Entitas;
	using Helpers.Map;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[InitializePhase(InitializePhase.RegisterDatabase)]
	public sealed class MapTrackerSystem : IInitializeSystem
	{
		private readonly GameContext gameContext;
		private EntityMap map;

		public MapTrackerSystem(Contexts contexts)
		{
			contexts.game.OnEntityCreated += Game_OnEntityCreated;
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			map = new EntityMap(100, 100);
			gameContext.AddService(map);
		}

		private void Game_OnEntityCreated(IContext context, IEntity rawEntity)
		{
			var entity = rawEntity as GameEntity;

			entity.isMapTracked = true;
			entity.OnComponentAdded += Entity_OnComponentAdded;
			entity.OnComponentReplaced += Entity_OnComponentReplaced;
			entity.OnComponentRemoved += Entity_OnComponentRemoved;
		}

		private void Entity_OnComponentRemoved(IEntity entity, int index, IComponent component)
		{
			var position = component as PositionComponent;

			if (position == null)
				return;

			map.RemoveEntity(entity as GameEntity, position.value);
		}

		private void Entity_OnComponentReplaced(IEntity entity, int index, IComponent previousComponent, IComponent newComponent)
		{
			var prevPos = previousComponent as PositionComponent;
			var newPos = newComponent as PositionComponent;

			if (prevPos == null)
				return;

			map.RemoveEntity(entity as GameEntity, prevPos.value);
			map.AddEntity(entity as GameEntity, newPos.value);
		}
		
		private void Entity_OnComponentAdded(IEntity entity, int index, IComponent component)
		{
			var position = component as PositionComponent;

			if (position == null)
				return;

			map.AddEntity(entity as GameEntity, position.value);
		}
	}
}