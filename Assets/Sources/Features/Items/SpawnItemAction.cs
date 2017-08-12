using ProtoBuf;

[ProtoContract]
public class SpawnItemAction : IAction
{
	[ProtoMember(1)]
	public ItemName Item;

	[ProtoMember(2)]
	public IntVector2 Position;

	public bool Validate(GameContext context)
	{
		return true;
	}
}