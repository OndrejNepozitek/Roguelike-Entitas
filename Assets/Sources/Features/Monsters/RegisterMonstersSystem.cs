namespace Assets.Sources.Features.Monsters
{
	using Helpers.Monsters;
	using Entitas;
	using Helpers;

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
				Attack = 30,
				AttackSpeed = 100,
				Defense = 10,
				MovementSpeed = 70,
				Prefab = Prefabs.BodyWhite,
				// Sheep = true
			});
		}
	}
}