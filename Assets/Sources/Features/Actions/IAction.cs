using ProtoBuf;

[ProtoContract]
[ProtoInclude(1, typeof(BasicMoveAction))]
[ProtoInclude(2, typeof(SpawnItemAction))]
[ProtoInclude(3, typeof(EquipAction))]
[ProtoInclude(4, typeof(PickAndEquipAction))]
public interface IAction
{
	bool Validate(GameContext context);
}