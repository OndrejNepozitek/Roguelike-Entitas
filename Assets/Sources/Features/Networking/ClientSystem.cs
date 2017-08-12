using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Helpers.Networking;
using Entitas;
using UnityEngine;

public class ClientSystem : IExecuteSystem
{
	private readonly ActionsContext actionsContext;
	private readonly IGroup<ActionsEntity> group;

	public ClientSystem(Contexts contexts)
	{
		actionsContext = contexts.actions;
		group = actionsContext.GetGroup(ActionsMatcher.Action);
	}

	public void Execute()
	{
		var client = NetworkController.Instance.NetworkEntity;
		var actionsReceived = client.Actions;
		var actionsToBeSent = new List<IAction>();

		foreach (var entity in group.GetEntities())
		{
			if (entity.hasAction)
			{
				actionsToBeSent.Add(entity.action.Action);

				// Try basic move prediction on client
				if (!(entity.action.Action is BasicMoveAction))
				{
					entity.Destroy();
				}
			}

			if (actionsToBeSent.Count > 0)
			{
				client.SendActions(actionsToBeSent);
			}
		}

		if (actionsReceived != null)
		{
			Debug.LogFormat("Received {0} actions", actionsReceived.Count);
			foreach (var action in actionsReceived)
			{
				var entity = actionsContext.CreateEntity();

				if (action == null)
				{
					throw new ArgumentException("Actions must not be null");
				}
				entity.AddAction(action);
			}
		}
	}
}