public static class ActionsContextExtensions
{
	public static ActionsEntity CreateBasicMove(this ActionsContext context, GameEntity targetEntity, IntVector2 direction)
	{
		var entity = context.CreateEntity();

		entity.AddAction(new BasicMoveAction() { Direction = direction }, targetEntity);

		return entity;
	}
}