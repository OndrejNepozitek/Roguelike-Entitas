using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public sealed class AddViewSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
	private Transform _viewContainer;
    readonly GameContext _context;

    public AddViewSystem(Contexts contexts) : base(contexts.game)
    {
        _context = contexts.game;
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
		        gameObject.transform.SetParent(_viewContainer, false);
		        entity.AddView(gameObject);
		        gameObject.Link(entity, _context);
	        }
	        else
	        {
		        throw new NotSupportedException();
	        }
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAsset && !entity.hasView;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Asset);
    }

	public void Initialize()
	{
		_viewContainer = new GameObject("Views").transform;
	}
}