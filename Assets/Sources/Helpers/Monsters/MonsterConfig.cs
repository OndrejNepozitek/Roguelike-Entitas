namespace Assets.Sources.Helpers.Monsters
{
	using Loot;

	public struct MonsterConfig
	{
		// Required
		public int Health;
		public int Attack;
		public int AttackSpeed;
		public int Defense;
		public int MovementSpeed;
		public int CriticalChance;
		public string Prefab;
		public bool IsAttackable;
		
		// Optional
		public bool Sheep;
		public LootGroupName? LootGroup;
		public bool Chest;
	}
}
