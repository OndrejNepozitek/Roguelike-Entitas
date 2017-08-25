namespace Assets.Sources.Features.Networking.Systems
{
	using System.Linq;
	using Entitas;
	using Helpers.Networking;
	using Helpers.Networking.ControlMessages;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[ExecutePhase(ExecutePhase.Network)]
	public class PlayerDisconnectSystem : IExecuteSystem, IInitializeSystem
	{
		private readonly IGroup<GameEntity> players;
		private Player playerToRemove;

		public PlayerDisconnectSystem(Contexts contexts)
		{
			players = contexts.game.GetGroup(GameMatcher.Player);
		}

		public void Execute()
		{
			if (playerToRemove == null) return;

			var playerEntity = players.GetEntities().FirstOrDefault(x => x.player.Id == playerToRemove.Id);

			if (playerEntity != null)
			{
				playerEntity.isDestroyed = true;
			}

			playerToRemove = null;
		}

		public void Initialize()
		{
			if (NetworkController.Instance.IsMultiplayer) // TODO: should be better
			{
				NetworkController.Instance.NetworkEntity.RegisterHandler<DisconnectedMessage>(OnPlayerDisconnected);
			}
		}

		private void OnPlayerDisconnected(DisconnectedMessage message, Player player)
		{
			playerToRemove = message.Player;
		}
	}
}
