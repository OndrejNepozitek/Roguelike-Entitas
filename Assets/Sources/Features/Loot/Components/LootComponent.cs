namespace Assets.Sources.Features.Loot.Components
{
	using Entitas;
	using Helpers.Loot;

	public class LootComponent : IComponent
	{
		public int Seed;
		public LootGroupName GroupName;
	}
}
