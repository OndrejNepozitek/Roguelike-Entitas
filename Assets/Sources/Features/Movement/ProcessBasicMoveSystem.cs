using System;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ProcessBasicMoveSystem : ReactiveSystem<ActionsEntity>
{
	public ProcessBasicMoveSystem(Contexts contexts) : base(contexts.actions)
	{
		
	}

	protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
	{
		return context.CreateCollector(ActionsMatcher.Action);
	}

	protected override bool Filter(ActionsEntity entity)
	{
		return entity.action.Action is BasicMoveAction && entity.action.Source.hasPosition && entity.action.Source.hasView && !entity.action.Source.isActionInProgress;
	}

	protected override void Execute(List<ActionsEntity> entities)
	{
		foreach (var actionEntity in entities)
		{
			var moveAction = actionEntity.action.Action as BasicMoveAction;
			Debug.Assert(moveAction != null, "moveAction != null");

			var entity = actionEntity.action.Source;

			var currentPosition = entity.position.value;
			var newPosition = currentPosition + moveAction.Direction;

			if (!Map.Instance.IsWalkable(newPosition))
			{
				continue;
			}

			entity.ReplacePosition(newPosition, true);
			entity.isActionInProgress = true;
		}
	}
}