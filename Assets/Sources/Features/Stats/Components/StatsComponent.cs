using Entitas;

namespace Assets.Sources.Features.Stats.Components
{
	public sealed class StatsComponent : IComponent
	{
		public int Attack;
		public int AttackSpeed;
		public int Defense;
		public int MovementSpeed;
	}
}