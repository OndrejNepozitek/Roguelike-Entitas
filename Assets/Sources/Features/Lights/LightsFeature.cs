namespace Assets.Sources.Features.Lights
{
	using Helpers.Entitas;

	public sealed class LightsFeature : Feature
	{
		public LightsFeature(Contexts contexts) : base("Lights feature")
		{
			Add(new AddShadowSystem(contexts));
			Add(new SetLightsSystem(contexts));
			Add(new SpriteShadowSystem(contexts));
		}
	}
}