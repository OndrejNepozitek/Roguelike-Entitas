using System;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ValidateActionsSystem : ReactiveSystem<ActionsEntity>
{
	private readonly GameContext gameContext;

	public ValidateActionsSystem(Contexts contexts) : base(contexts.actions)
	{
		gameContext = contexts.game;
	}

	protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
	{
		return context.CreateCollector(ActionsMatcher.Action.Added());
	}

	protected override bool Filter(ActionsEntity entity)
	{
		return entity.hasAction;
	}

	protected override void Execute(List<ActionsEntity> entities)
	{
		foreach (var entity in entities)
		{
			if (!entity.action.Action.Validate(gameContext))
			{
				Debug.Log("Destroying entity because of validation - " + entity.action.Action.GetType().Name);
				entity.Destroy();
			}
		}
	}
}