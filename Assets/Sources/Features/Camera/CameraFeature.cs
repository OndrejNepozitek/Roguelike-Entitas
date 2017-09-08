namespace Assets.Sources.Features.Camera
{
	using Helpers.SystemDependencies;

	public class CameraFeature : Feature
	{
		public CameraFeature(Contexts contexts) : base("Camera feature")
		{
			Add(new PlayerCentricCameraSystem(contexts));
		}
	}
}
