namespace Assets.Sources.Features.Actions
{
	using Combat;
	using Monsters;
	using Items.Actions;
	using Loot.Actions;
	using Movement;
	using ProtoBuf;
	using Stats;

	[ProtoContract]
	[ProtoInclude(1, typeof(BasicMoveAction))]
	[ProtoInclude(2, typeof(SpawnItemAction))]
	[ProtoInclude(3, typeof(EquipAction))]
	[ProtoInclude(4, typeof(PickAndEquipAction))]
	[ProtoInclude(5, typeof(SpawnMonsterAction))]
	[ProtoInclude(6, typeof(AttackAction))]
	[ProtoInclude(7, typeof(OpenChestAction))]
	[ProtoInclude(8, typeof(RespawnAction))]
	public interface IAction
	{
		bool Validate(GameContext context);
	}
}