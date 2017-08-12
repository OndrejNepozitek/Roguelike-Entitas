using Assets.Sources.Helpers.Monsters;
using Entitas;

public class RegisterMonstersSystem : IInitializeSystem
{
	public void Initialize()
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
}