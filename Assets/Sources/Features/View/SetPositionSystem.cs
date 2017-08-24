namespace Assets.Sources.Features.View
{
	using System.Collections;
	using System.Collections.Generic;
	using Coroutines;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;
	using UnityEngine.Assertions;

	/// <summary>
	/// Set the game object position when PositionComponent changes.
	/// </summary>
	[ExecutePhase(ExecutePhase.ReactToComponents)]
	[DependsOn(typeof(CoroutinesFeature))]
	[ExecutesAfter(typeof(AddViewSystem))]
	public sealed class SetPositionSystem : ReactiveSystem<GameEntity>
	{
		public SetPositionSystem(Contexts contexts) : base(contexts.game)
		{
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				var pos = entity.position.value;

				if (entity.position.smooth)
				{
					if (entity.hasCoroutine)
					{
						entity.RemoveCoroutine(); // TODO: dangerous - you can replace coroutine with another coroutine
						Debug.Log("Coroutine was replaced - movement");
					}
					entity.AddCoroutine(EasedSmoothMovement(entity), null);
				} else
				{
					entity.view.gameObject.transform.position = (Vector2)pos;
				}
            
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasView && entity.hasPosition;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Position);
		}

		private static IEnumerator SmoothMovement(GameEntity entity)
		{
			var gameObject = entity.view.gameObject;
			var transform = gameObject.transform;
			var end = entity.position.value;
			var endVector3 = (Vector3)((Vector2)end);
			var inverseMoveTime = 1f / 0.1f;
			var sqrRemainingDistance = (transform.position - endVector3).sqrMagnitude;

			while (sqrRemainingDistance > float.Epsilon)
			{
				Vector2 newPostion = Vector2.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
				transform.position = newPostion;
				sqrRemainingDistance = (transform.position - endVector3).sqrMagnitude;
				yield return null;
			}

			entity.isActionInProgress = false;
			entity.view.gameObject.transform.position = (Vector2)end;
		}

		private static IEnumerator EasedSmoothMovement(GameEntity entity)
		{
			var gameObject = entity.view.gameObject;
			var transform = gameObject.transform;

			var start = transform.position;
			var end = (Vector3) (Vector2) entity.position.value;
			var totalTime = 0.15f;
			var currentTime = 0f;
			entity.AddActionProgress(0);

			while (currentTime < totalTime)
			{
				currentTime += Time.deltaTime;
				var t = currentTime / totalTime; // Progress percentage
				entity.ReplaceActionProgress(t);

				t = t * t * t * (t * (6f * t - 15f) + 10f);

				transform.position = Vector3.Lerp(start, end, t);
				yield return null;
			}

			entity.isActionInProgress = false;
			entity.RemoveActionProgress();
			entity.view.gameObject.transform.position = (Vector2)end;
			yield return null;
		}
	}
}