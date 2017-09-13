namespace Assets.Sources.Features.Monsters
{
	using Systems;
	using Helpers.SystemDependencies;

	public class MonstersFeature : Feature
	{
		public MonstersFeature(Contexts contexts) : base("Monsters feature")
		{
			Add(new AddMonsterReferenceSystem(contexts));
			Add(new RegisterMonstersSystem(contexts));
			Add(new SpawnMonsterSystem(contexts));
		}
	}
}
