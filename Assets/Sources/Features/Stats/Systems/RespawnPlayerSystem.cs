namespace Assets.Sources.Features.Stats.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Extensions;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Scripts;

	[ExecutePhase(ExecutePhase.ReactToActions)]
	public class RespawnPlayerSystem : ReactiveSystem<ActionsEntity>
	{
		private readonly GameContext gameContext;
		private GameGUIController guiController;

		public RespawnPlayerSystem(Contexts contexts) : base(contexts.actions)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			guiController = gameContext.GetService<GameGUIController>();
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action);
		}

		protected override bool Filter(ActionsEntity entity)
		{
			return entity.hasAction && entity.action.Action is RespawnAction;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			guiController = gameContext.GetService<GameGUIController>(); // TODO: remove

			foreach (var rawAction in entities)
			{
				var action = (RespawnAction) rawAction.action.Action;

				var player = gameContext.CreatePlayer(action.Position, action.Player);
				var id = EntityDatabase.Instance.GetNextId(); // TODO: remove singleton
				player.ReplaceNetworkTracked(new EntityReference(id));
				EntityDatabase.Instance.AddEntity(id, player);

				if (action.Player.Equals(gameContext.currentPlayer.Player))
				{
					gameContext.cameraTarget.Target = player.GetReference();
					gameContext.currentPlayer.Entity = player.GetReference();
					guiController.HidePlayerRespawning();
				}
			}
		}
	}
}