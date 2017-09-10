namespace Assets.Sources.Features.Lights.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;
	using View;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	[ExecutesAfter(typeof(SetLightsSystem), typeof(AddShadowSystem), typeof(AddViewSystem))]
	public sealed class SpriteShadowSystem : ReactiveSystem<GameEntity>
	{
		public SpriteShadowSystem(Contexts contexts) : base(contexts.game)
		{

		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				Color color;

				if (entity.hasInLight)
				{
					color = GetColorForLight(entity.inLight.Value);
				} else
				{
					color = GetColorForShadow(entity.shadow.Value); 
				}

				foreach (var renderer in entity.view.gameObject.GetComponentsInChildren<SpriteRenderer>())
				{
					renderer.color = color;
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasView;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.InLight.AddedOrRemoved(), GameMatcher.View.Added(), GameMatcher.Inventory.AddedOrRemoved());
		}

		private static Color GetColorForShadow(int shadow)
		{
			var val = shadow / 100f;
			return new Color(val, val, val);
		}

		private static Color GetColorForLight(int light)
		{
			return new Color(light / 100f, light / 108f, light / 118f);
		}
	}
}