namespace Assets.Sources.Features.Stats.Components
{
	using Entitas;

	public class StatsComponent : IComponent
	{
		public int MaxHealth;
		public int Attack;
		public int AttackSpeed;
		public int Defense;
		public int MovementSpeed;
		public int CriticalChance;

		public StatsComponent Clone()
		{
			return (StatsComponent) MemberwiseClone();
		}
	}
}