namespace Assets.Sources.Features.AI
{
	using System;
	using System.Collections.Generic;
	using Helpers.Map;
	using Entitas;
	using Extensions;
	using Helpers;

	public sealed class JumpAISystem : ReactiveSystem<GameEntity>
	{
		private readonly GameContext gameContext;

		public JumpAISystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			if (entities.Count > 1)
				throw new InvalidOperationException();

			foreach (var entity in entities)
			{
				entity.isShouldAct = false;
				entity.isActionInProgress = true;

				var currentPos = entity.position.value;
				var moved = false;
				for (int i = 0; i < 10; i++)
				{
					var pos = currentPos + new IntVector2(3 * UnityEngine.Random.Range(-1, 2), 3 * UnityEngine.Random.Range(-1, 2));
					if (gameContext.GetService<EntityMap>().IsWalkable(pos))
					{
						entity.ReplacePosition(pos, true);
						moved = true;
						break;
					}
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isJumpAI;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.ShouldAct);
		}
	}
}