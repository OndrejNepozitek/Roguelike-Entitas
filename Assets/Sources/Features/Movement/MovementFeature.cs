namespace Assets.Sources.Features.Movement
{
	using Helpers.SystemDependencies;

	public class MovementFeature : Feature
	{
		public MovementFeature(Contexts contexts) : base("Movement feature")
		{
			Add(new ProcessBasicMoveSystem(contexts));
			Add(new MovementInputSystem(contexts));
		}
	}
}
