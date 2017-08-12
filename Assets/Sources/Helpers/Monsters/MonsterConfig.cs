namespace Assets.Sources.Helpers.Monsters
{
	public struct MonsterConfig
	{
		// Required
		public int Health;
		public int Attack;
		public int AttackSpeed;
		public int Defense;
		public int MovementSpeed;
		public string Prefab;
		
		// Optional
		public bool Sheep;
	}
}
