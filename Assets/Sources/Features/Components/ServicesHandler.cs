namespace Assets.Sources.Features.Components
{
	using Entitas;
	using Entitas.CodeGeneration.Attributes;

	[Unique]
	public class ServicesHandler : IComponent
	{
		public Helpers.ServicesHandler Services;
	}
}
