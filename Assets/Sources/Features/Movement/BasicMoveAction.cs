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

		if (entity.isActionInProgress) return false;
		if (!Map.Instance.IsWalkable(Position)) return false;

		// TODO: Check if the move is valid from the current entity position

		return true;
	}
}