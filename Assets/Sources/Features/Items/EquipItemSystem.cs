namespace Assets.Sources.Features.Items
{
	using System.Collections.Generic;
	using Actions;
	using Entitas;
	using Helpers;
	using Helpers.Items;
	using Helpers.Map;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using MapTracker;
	using UnityEngine;
	using View;

	[ExecutePhase(ExecutePhase.ReactToActions)]
	[DependsOn(typeof(ActionsFeature), typeof(MapTrackerSystem), typeof(ViewFeature))]
	public class EquipItemSystem : ReactiveSystem<ActionsEntity>, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private ItemDatabase items;

		public EquipItemSystem(Contexts contexts) : base(contexts.actions)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			items = gameContext.GetService<ItemDatabase>();
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action.Added());
		}

		protected override bool Filter(ActionsEntity entity)
		{
			return entity.hasAction && (entity.action.Action is EquipAction || entity.action.Action is PickAndEquipAction);
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			foreach (var entity in entities)
			{
				IItem item;
				GameEntity target;

				if (!entity.hasAction)
				{
					continue;
				}

				if (entity.action.Action is EquipAction)
				{
					var action = (EquipAction)entity.action.Action;
					item = items.GetItem(action.Item);
					target = action.Entity.GetEntity();
				}
				else
				{
					var action = (PickAndEquipAction)entity.action.Action;
					var itemEntity = gameContext.GetService<EntityMap>().GetItem(action.Position);

					itemEntity.isDestroyed = true;
					item = itemEntity.item.Item;
					target = action.Entity.GetEntity();
				}

				// TODO: should old game object be destroyed while replacing?
				var asset = Resources.Load<GameObject>(item.Prefab);
				var go = Object.Instantiate(asset);
				var parentGo = target.view.gameObject;
				go.transform.SetParent(parentGo.transform, false);
				go.transform.SetSiblingIndex((int)item.Slot);

				var sprite = go.GetComponent<SpriteRenderer>();
				sprite.sortingLayerName = "Characters"; // TODO: should not be hardcoded

				target.SetInventoryItem(item.Slot, item); // TODO: should slot be handled this way?
			}
		}
	}
}