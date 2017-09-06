namespace Assets.Sources.Features.Stats.Systems
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Scripts;
	using UnityEngine;
	using Extensions;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	public class ShouldDieSystem : ReactiveSystem<GameEntity>, ICleanupSystem, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;
		private readonly IGroup<GameEntity> deadEntities;
		private readonly IGroup<GameEntity> players;
		private GameGUIController guiController;

		public ShouldDieSystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
			actionsContext = contexts.actions;
			deadEntities = gameContext.GetGroup(GameMatcher.Dead);
			players = gameContext.GetGroup(GameMatcher.Player);
		}

		public void Cleanup()
		{
			foreach (var entity in deadEntities.GetEntities())
			{
				if (entity.isDead)
				{
					entity.Destroy();
				}
			}
		}

		protected override void Execute(List<GameEntity> entities)
		{
			var playerDied = false;
			foreach (var entity in entities)
			{
				if (entity.health.Value <= 0)
				{
					if (entity.hasPlayer)
					{
						if (NetworkController.Instance.IsMultiplayer)
						{
							playerDied = true;
						}
						else
						{
							gameContext.GetService<GameController>().GameOver();
						}
					}

					entity.isDead = true;
				}
			}

			if (playerDied)
			{
				HandleMultiplayerDead();
			}
		}

		private void HandleMultiplayerDead()
		{
			var alivePlayer = players.GetEntities().FirstOrDefault(x => !x.isDead);

			if (alivePlayer != null)
			{
				gameContext.cameraTarget.Target = alivePlayer.GetReference();

				if (gameContext.GetCurrentPlayer().isDead)
				{
					gameContext.CreateEntity().AddCoroutine(RespawnCoroutine(gameContext.GetCurrentPlayer()), null);
				}
			}
			else
			{
				guiController.HidePlayerRespawning();
				gameContext.GetService<GameController>().GameOver();
			}
		}

		private IEnumerator RespawnCoroutine(GameEntity playerEntity)
		{
			guiController.ShowPlayerRespawning();
			var player = gameContext.currentPlayer.Player;

			for (var i = 5; i > 0; i--)
			{
				var totalTime = 1f;
				var currentTime = 0f;
				guiController.SetRespawnTime(i);

				while (currentTime < totalTime)
				{
					currentTime += Time.deltaTime;
					yield return null;
				}
			}

			actionsContext.RespawnPlayer(player);

			yield return null;
		}

		protected override bool Filter(GameEntity entity)
		{
			return true;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Health);
		}

		public void Initialize()
		{
			guiController = gameContext.GetService<GameGUIController>();
		}
	}
}