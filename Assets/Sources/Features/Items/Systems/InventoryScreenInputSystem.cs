namespace Assets.Sources.Features.Items.Systems
{
	using Entitas;
	using Extensions;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Scripts;
	using UnityEngine;

	[ExecutePhase(ExecutePhase.Input)]
	public class InventoryScreenInputSystem : IExecuteSystem, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private InventoryController inventoryController;

		public InventoryScreenInputSystem(Contexts contexts)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			inventoryController = gameContext.GetService<InventoryController>();
		}

		public void Execute()
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				if (!inventoryController.IsOpened)
				{
					inventoryController.SetHealth(gameContext.GetCurrentPlayer().health.Value);
					inventoryController.Open();
				}
				else
				{
					inventoryController.Close();
				}
			}
		}
	}
}
