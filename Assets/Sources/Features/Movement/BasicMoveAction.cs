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
			if (entity.isActionInProgress)
			{
				// Current player and non-player moves cannot be predicted
				if (entity.isActionInProgress && (!entity.hasPlayer || entity == context.GetCurrentPlayer())) return false;

				// Moves of other players can be allowed with given progress treshold
				if (!entity.hasActionProgress || entity.actionProgress.Progress < 0.9) return false;
			}

			// TODO: Check if the move is valid from the current entity position

			return true;
		}
	}
}