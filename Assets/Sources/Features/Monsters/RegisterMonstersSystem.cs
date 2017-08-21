namespace Assets.Sources.Features.Monsters
{
	using Helpers.Monsters;
	using Entitas;
	using Helpers;

	public class RegisterMonstersSystem : IInitializeSystem
	{
		// TODO: bad
		public RegisterMonstersSystem(Contexts contexts)
		{
			var monsters = MonsterDatabase.Instance;
			monsters.Reset();

			monsters.RegisterMonster(MonsterType.NakedMan, new MonsterConfig
			{
				Health = 100,
				Attack = 30,
				AttackSpeed = 100,
				Defense = 10,
				MovementSpeed = 70,
				Prefab = Prefabs.BodyWhite,
				// Sheep = true
			});
		}

		public void Initialize()
		{

		}
	}
}