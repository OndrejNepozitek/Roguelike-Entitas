namespace Assets.Sources.Features.Monsters
{
	using Actions;
	using Helpers.Map;
	using Helpers.Monsters;
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

		[ProtoMember(4)]
		public int LootSeed;

		public bool Validate(GameContext context)
		{
			if (!context.GetService<EntityMap>().IsWalkable(Position)) return false;

			return true;
		}
	}
}