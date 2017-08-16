namespace Assets.Sources.Features.Coroutines
{
	using Helpers.Entitas;

	public class CoroutinesFeature : Feature
	{
		public CoroutinesFeature(Contexts contexts) : base("Coroutines feature")
		{
			Add(new CoroutineSystem(contexts));
		}
	}
}