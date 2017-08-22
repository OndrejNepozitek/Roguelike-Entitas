namespace Assets.Sources.Features.Camera
{
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using UnityEngine;
	using View;

	/// <summary>
	/// Centers camera on the current player.
	/// </summary>
	[DependsOn(typeof(ViewFeature))]
	public sealed class PlayerCentricCameraSystem : ICleanupSystem, IInitializeSystem
	{
		private readonly GameContext context;
		private readonly GameEntity camera;
		private IntVector2 offset;

		public PlayerCentricCameraSystem(Contexts contexts)
		{
			context = contexts.game;
			camera = context.cameraEntity;
		}

		public void Initialize()
		{
			offset = new IntVector2();
		}

		private void UpdatePosition()
		{
			Vector3 pos;
			var player = context.GetCurrentPlayer();

			if (player.hasView)
			{
				pos = (player.view.gameObject.transform.position + (Vector3)offset);
			} else
			{
				pos = (Vector3)(player.position.value + offset);
			}

			pos.z = -5;
			camera.camera.value.transform.position = pos;
		}

		public void Cleanup()
		{
			UpdatePosition();
		}
	}
}