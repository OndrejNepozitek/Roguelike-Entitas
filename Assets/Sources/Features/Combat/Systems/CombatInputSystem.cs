namespace Assets.Sources.Features.Combat.Systems
{
	using System.Linq;
	using Entitas;
	using Extensions;
	using Helpers.Map;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;

	[ExecutePhase(ExecutePhase.Input)]
	public class CombatInputSystem : IExecuteSystem, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;
		private EntityMap map;

		public CombatInputSystem(Contexts contexts)
		{
			gameContext = contexts.game;
			actionsContext = contexts.actions;
		}

		public void Initialize()
		{
			map = gameContext.GetService<EntityMap>();
		}

		public void Execute()
		{
			var player = gameContext.GetCurrentPlayer();
			var horizontal = (int)Input.GetAxisRaw("Horizontal");
			var vertical = (int)Input.GetAxisRaw("Vertical");

			var direction = IntVector2.GetGridDirection(horizontal, vertical);
			var position = player.position.value + direction;
			var target = map.GetEntitiesOnTile(position).FirstOrDefault(x => x.isAttackable);

			if (target == null) return;

			actionsContext.Attack(player, target, AttackType.Basic);
		}
	}
}
