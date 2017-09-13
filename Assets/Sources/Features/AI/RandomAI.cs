namespace Assets.Sources.Features.AI
{
	using System;
	using System.Collections.Generic;
	using Helpers.Map;
	using Entitas;
	using Extensions;
	using Helpers;

	public sealed class RandomAISystem : ReactiveSystem<GameEntity>, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private EntityMap map;

		public RandomAISystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			map = gameContext.GetService<EntityMap>();
		}

		protected override void Execute(List<GameEntity> entities)
		{
			if (entities.Count > 1)
				throw new InvalidOperationException();

			foreach (var entity in entities)
			{
				entity.isShouldAct = false;
				entity.isActionInProgress = true;

				var pos = entity.position.value;
				List<IntVector2> moves = new List<IntVector2>();
				if (map.IsWalkable((int)pos.X + 1, (int)pos.Y))
					moves.Add(new IntVector2((int)pos.X + 1, (int)pos.Y));

				if (map.IsWalkable((int)pos.X, (int)pos.Y+1))
					moves.Add(new IntVector2((int)pos.X, (int)pos.Y+1));

				if (map.IsWalkable((int)pos.X - 1, (int)pos.Y))
					moves.Add(new IntVector2((int)pos.X - 1, (int)pos.Y));

				if (map.IsWalkable((int)pos.X, (int)pos.Y - 1))
					moves.Add(new IntVector2((int)pos.X, (int)pos.Y - 1));

				if (moves.Count == 0)
				{

				} else
				{
					var move = moves[UnityEngine.Random.Range(0, moves.Count)];
					entity.ReplacePosition(move, true);
				}

            
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isRandomAI;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.ShouldAct);
		}
	}
}