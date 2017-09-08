namespace Assets.Sources.Features.Coroutines
{
	using System.Collections;
	using Entitas;

	public class CoroutineComponent : IComponent
	{
		public IEnumerator Value;
		public System.Action<GameEntity> Callback;
	}
}
