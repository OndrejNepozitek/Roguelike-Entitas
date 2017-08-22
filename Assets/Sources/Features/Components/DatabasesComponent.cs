namespace Assets.Sources.Features.Components
{
	using Entitas;
	using Entitas.CodeGeneration.Attributes;
	using Helpers;

	[Unique]
	public class DatabasesComponent : IComponent
	{
		public DatabasesHandler Databases;
	}
}
