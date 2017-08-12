using ProtoBuf;

[ProtoContract]
public class SpawnMonsterAction : IAction
{
	[ProtoMember(1)]
	public IntVector2 Position;

	[ProtoMember(2)]
	public EntityReference Entity;

	[ProtoMember(3)]
	public MonsterType Type;

	public bool Validate(GameContext context)
	{
		if (!Map.Instance.IsWalkable(Position)) return false;

		return true;
	}
}