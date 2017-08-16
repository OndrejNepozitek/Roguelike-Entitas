namespace Assets.Sources.Features.Camera
{
	using Helpers.Entitas;

	class CameraFeature : Feature
	{
		public CameraFeature(Contexts contexts) : base("Camera feature")
		{
			Add(new PlayerCentricCameraSystem(contexts));
		}
	}
}
