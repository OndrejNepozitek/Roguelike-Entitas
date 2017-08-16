namespace Assets.Sources.Features.Input
{
	using Helpers.Entitas;

	public sealed class InputFeature : Feature
	{
		public InputFeature(Contexts contexts) : base("Input feature")
		{
			Add(new InputSystem(contexts));
		}
	}
}