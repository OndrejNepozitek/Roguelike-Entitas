namespace Assets.Sources.Features.Items.Systems
{
	using Entitas;
	using Extensions;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;

	[ExecutePhase(ExecutePhase.Input)]
	public class ItemsInputSystem : IExecuteSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;

		public ItemsInputSystem(Contexts contexts)
		{
			gameContext = contexts.game;
			actionsContext = contexts.actions;
		}

		public void Execute()
		{
			var player = gameContext.GetCurrentPlayer();

			if (Input.GetKeyDown(KeyCode.E))
			{
				actionsContext.PickAndEquip(player.position.value, player);
			}
		}
	}
}
