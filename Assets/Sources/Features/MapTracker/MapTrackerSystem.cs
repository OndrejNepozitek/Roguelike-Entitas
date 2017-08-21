namespace Assets.Sources.Features.MapTracker
{
	using Entitas;
	using Helpers;
	using Helpers.Map;

	public sealed class MapTrackerSystem : ISystem
	{
		public MapTrackerSystem(Contexts contexts)
		{
			contexts.game.OnEntityCreated += Game_OnEntityCreated;
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

			EntityMap.Instance.RemoveEntity(entity as GameEntity, position.value);
		}

		private void Entity_OnComponentReplaced(IEntity entity, int index, IComponent previousComponent, IComponent newComponent)
		{
			var prevPos = previousComponent as PositionComponent;
			var newPos = newComponent as PositionComponent;

			if (prevPos == null)
				return;

			EntityMap.Instance.RemoveEntity(entity as GameEntity, prevPos.value);
			EntityMap.Instance.AddEntity(entity as GameEntity, newPos.value);
		}
		
		private void Entity_OnComponentAdded(IEntity entity, int index, IComponent component)
		{
			var position = component as PositionComponent;

			if (position == null)
				return;

			EntityMap.Instance.AddEntity(entity as GameEntity, position.value);
		}
	}
}