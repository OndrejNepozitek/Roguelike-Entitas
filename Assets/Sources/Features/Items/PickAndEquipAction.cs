using Assets.Sources.Helpers;
using Assets.Sources.Helpers.Map;
using ProtoBuf;

[ProtoContract]
public class PickAndEquipAction : IAction
{
	[ProtoMember(1)]
	public IntVector2 Position;

	[ProtoMember(2)]
	public EntityReference Entity;

	/// <summary>
	/// Entity's position should be the same a the item's position
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public bool Validate(GameContext context)
	{
		if (Entity.GetEntity().position.value != Position) return false;
		if (EntityMap.Instance.GetItem(Position) == null) return false;

		return true;
	}
}