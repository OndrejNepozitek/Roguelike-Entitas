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
		private readonly GameContext gameContext;
		private Camera camera;
		private IntVector2 offset;

		public PlayerCentricCameraSystem(Contexts contexts)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			offset = new IntVector2();
			camera = gameContext.GetService<Camera>();
		}

		private void UpdatePosition()
		{
			Vector3 pos;
			var target = gameContext.cameraTarget.Target.GetEntity();

			if (target == null)
			{
				return;
			}

			if (target.hasView)
			{
				pos = (target.view.gameObject.transform.position + (Vector3)offset);
			} else
			{
				pos = (Vector3)(target.position.value + offset);
			}

			pos.z = -5;
			camera.transform.position = pos;
		}

		public void Cleanup()
		{
			UpdatePosition();
		}
	}
}