namespace Assets.Sources.Features.View
{
	using System.Collections;
	using System.Collections.Generic;
	using Coroutines;
	using Helpers.Entitas;
	using Entitas;
	using UnityEngine;

	/// <summary>
	/// Set the game object position when PositionComponent changes.
	/// </summary>
	[SystemPhase(Phase.ReactToComponents)]
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
					entity.AddCoroutine(SmoothMovement(entity), null);
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

		static IEnumerator SmoothMovement(GameEntity entity)
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
	}
}