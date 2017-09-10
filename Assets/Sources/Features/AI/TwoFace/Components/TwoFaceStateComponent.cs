namespace Assets.Sources.Features.AI.TwoFace.Components
{
	using Entitas;
	using Entitas.CodeGeneration.Attributes;

	[Unique]
	public class TwoFaceStateComponent : IComponent
	{
		public float TimeElapsed;
		public bool IsAngry;
	}
}
