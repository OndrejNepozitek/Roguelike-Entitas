namespace Assets.Sources.Features.AI.TwoFace
{
	using Systems;
	using Helpers.SystemDependencies;

	public class TwoFaceFeature : Feature
	{
		public TwoFaceFeature(Contexts contexts) : base("TwoFace feature")
		{
			Add(new TwoFaceTimerSystem(contexts));
			Add(new TwoFaceAssetSystem(contexts));
			Add(new TwoFaceAISystem(contexts));
		}
	}
}
