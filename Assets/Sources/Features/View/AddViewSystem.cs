namespace Assets.Sources.Features.View
{
	using System;
	using System.Collections.Generic;
	using Entitas;
	using Entitas.Unity;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;

	/// <summary>
	/// Instantiate GameObjects when assets are added.
	/// </summary>
	[ExecutePhase(ExecutePhase.ReactToComponents)]
	public sealed class AddViewSystem : ReactiveSystem<GameEntity>, IInitializeSystem
	{
		private Transform viewContainer;
		private readonly GameContext context;

		public AddViewSystem(Contexts contexts) : base(contexts.game)
		{
			context = contexts.game;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				var asset = Resources.Load<GameObject>(entity.asset.name);
				GameObject gameObject = null;
				try
				{
					gameObject = UnityEngine.Object.Instantiate(asset);
				}
				catch (Exception)
				{
					Debug.Log("Cannot instantiate " + entity.asset.name);
				}

				if (gameObject != null)
				{
					gameObject.transform.SetParent(viewContainer, false);

					if (entity.hasView)
					{
						// TODO: how bad is to often initialize and destroy objects?
						gameObject.transform.position = entity.view.gameObject.transform.position;
						entity.view.gameObject.Unlink();
						UnityEngine.Object.Destroy(entity.view.gameObject);
						entity.ReplaceView(gameObject);
						gameObject.Link(entity, context);
					}
					else
					{
						entity.AddView(gameObject);
						gameObject.Link(entity, context);
					}
				}
				else
				{
					throw new NotSupportedException();
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasAsset;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Asset.Added());
		}

		public void Initialize()
		{
			viewContainer = new GameObject("Views").transform;
		}
	}
}