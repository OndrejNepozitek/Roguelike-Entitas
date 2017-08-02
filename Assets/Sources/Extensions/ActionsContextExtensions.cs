public static class ActionsContextExtensions
{
	public static ActionsEntity BasicMove(this ActionsContext context, GameEntity targetEntity, IntVector2 direction)
	{
		var entity = context.CreateEntity();

		entity.AddAction(new BasicMoveAction() { Direction = direction }, targetEntity);

		return entity;
	}

	public static ActionsEntity SpawnItem(this ActionsContext context, IItem item, IntVector2 position)
	{
		var entity = context.CreateEntity();

		entity.AddAction(new SpawnItemAction() { Item = item, Position = position }, null);

		return entity;
	}

	public static ActionsEntity SpawnItem(this ActionsContext context, ItemName name, IntVector2 position)
	{
		var item = ItemDatabase.Instance.GetItem(name);
		return context.SpawnItem(item, position);
	}
}