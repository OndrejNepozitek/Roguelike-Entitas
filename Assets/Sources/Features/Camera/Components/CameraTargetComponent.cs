namespace Assets.Sources.Features.Camera.Components
{
	using Entitas;
	using Entitas.CodeGeneration.Attributes;

	[Unique]
	public sealed class CameraTargetComponent : IComponent
	{
		public EntityReference Target;
	}
}