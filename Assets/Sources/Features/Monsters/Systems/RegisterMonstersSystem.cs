namespace Assets.Sources.Features.Monsters.Systems
{
	using Entitas;
	using Extensions;
	using Helpers;
	using Helpers.Loot;
	using Helpers.Monsters;

	public class RegisterMonstersSystem : IInitializeSystem
	{
		private readonly GameContext gameContext;

		public RegisterMonstersSystem(Contexts contexts)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			var monsters = new MonsterDatabase();
			gameContext.AddService(monsters);

			monsters.RegisterItem(MonsterType.NakedMan, new MonsterConfig
			{
				Health = 100,
				Attack = 5,
				AttackSpeed = 100,
				Defense = 10,
				MovementSpeed = 45,
				Prefab = Prefabs.BodyWhite,
				Sheep = true,
				IsAttackable = true,
				LootGroup = LootGroupName.Global
			});

			monsters.RegisterItem(MonsterType.MonsterGreen, new MonsterConfig
			{
				Health = 100,
				Attack = 5,
				AttackSpeed = 100,
				Defense = 10,
				MovementSpeed = 45,
				CriticalChance = 5,
				Prefab = Prefabs.MonsterGreen,
				IsAttackable = true,
				IsTwoFace = true,
				LootGroup = LootGroupName.Global
			});

			monsters.RegisterItem(MonsterType.BasicChest, new MonsterConfig
			{
				Health = 1,
				Prefab = Prefabs.BasicChest,
				Chest = true,
				LootGroup = LootGroupName.BasicChest
			});
		}
	}
}