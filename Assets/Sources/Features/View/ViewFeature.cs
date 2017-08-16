namespace Assets.Sources.Features.View
{
	using Helpers.Entitas;

	public sealed class ViewFeature : Feature
	{

		public ViewFeature(Contexts contexts) : base("View Systems")
		{
			Add(new AddViewSystem(contexts));
			Add(new SetPositionSystem(contexts));
			Add(new RemoveViewSystem(contexts));
		}
	}
}