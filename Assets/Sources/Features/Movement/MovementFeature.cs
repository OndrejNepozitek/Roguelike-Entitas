namespace Assets.Sources.Features.Movement
{
	using Helpers.Entitas;

	public class MovementFeature : Feature
	{
		public MovementFeature(Contexts contexts) : base("Movement feature")
		{
			Add(new ProcessBasicMoveSystem(contexts));
		}
	}
}
