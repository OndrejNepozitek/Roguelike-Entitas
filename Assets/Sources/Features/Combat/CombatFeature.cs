namespace Assets.Sources.Features.Combat
{
	using Systems;
	using Helpers.SystemDependencies;

	public class CombatFeature : Feature
	{
		public CombatFeature(Contexts contexts) : base("Combat feature")
		{
			Add(new CombatInputSystem(contexts));
			Add(new BasicCombatSystem(contexts));
			Add(new CombatResultSystem(contexts));
		}
	}
}
