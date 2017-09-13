namespace Assets.Sources.Features.Loot.Systems
{
	using System.Linq;
	using Entitas;
	using Extensions;
	using Helpers;
	using Helpers.Map;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;

	[ExecutePhase(ExecutePhase.Input)]
	public class OpenChestInputSystem : IExecuteSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;

		public OpenChestInputSystem(Contexts contexts)
		{
			gameContext = contexts.game;
			actionsContext = contexts.actions;
		}

		public void Execute()
		{
			var player = gameContext.GetCurrentPlayer();

			if (player == null)
			{
				return;
			}

			var horizontal = (int)Input.GetAxisRaw("Horizontal");
			var vertical = (int)Input.GetAxisRaw("Vertical");
			var direction = IntVector2.GetGridDirection(horizontal, vertical);

			var chest = gameContext.GetService<EntityMap>().GetEntitiesOnTile(player.position.value + direction).SingleOrDefault(x => x.isChest);

			if (chest == null) return;

			actionsContext.OpenChest(player, chest);
		}
	}
}
