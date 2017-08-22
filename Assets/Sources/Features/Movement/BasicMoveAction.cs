namespace Assets.Sources.Features.Movement
{
	using Actions;
	using Helpers.Map;
	using ProtoBuf;

	[ProtoContract]
	public class BasicMoveAction : IAction
	{
		[ProtoMember(1)]
		public IntVector2 Position;

		[ProtoMember(2)]
		public EntityReference Entity;

		public bool Validate(GameContext context)
		{
			var entity = Entity.GetEntity();

			if (entity.position.value == Position) return false;
			if (!context.GetService<EntityMap>().IsWalkable(Position)) return false;

			// TODO: let player move when he is already moving for prediction
			// It should be somehow made to avoid cheating
			if (entity.isActionInProgress && (!entity.hasPlayer || entity.player.Focus)) return false;

			// TODO: Check if the move is valid from the current entity position

			return true;
		}
	}
}