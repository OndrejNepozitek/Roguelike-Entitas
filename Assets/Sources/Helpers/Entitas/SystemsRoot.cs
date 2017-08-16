namespace Assets.Sources.Helpers.Entitas
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using global::Entitas;
	using TopologicalOrder;
	using UnityEngine;

	public class SystemsRoot : Systems
	{
		private readonly List<TopologicalOrder<IExecuteSystem>> orderedSystems = new List<TopologicalOrder<IExecuteSystem>>();

		public SystemsRoot()
		{
			var phases = Enum.GetValues(typeof(Phase)).Cast<Phase>();

			foreach (var _ in phases)
			{
				orderedSystems.Add(new TopologicalOrder<IExecuteSystem>());
			}
		}

		public new SystemsRoot Add(ISystem system)
		{
			var initializeSystem = system as IInitializeSystem;
			if (initializeSystem != null)
			{
				_initializeSystems.Add(initializeSystem);
			}

			var executeSystem = system as IExecuteSystem;
			if (executeSystem != null)
			{
				var phase = executeSystem.GetType().GetPhase();
				var topologicalOrder = orderedSystems[(int)phase];
				topologicalOrder.AddVertex(executeSystem);

				var executesBefore = executeSystem.GetType()
					.GetCustomAttributeValue<ExecutesBeforeAttribute, Type[]>(attr => attr.Systems);
				if (executesBefore != null)
				{
					foreach (var systemType in executesBefore)
					{
						topologicalOrder.AddEdge(executeSystem.GetType(), systemType);
					}
				}

				var executesAfter = executeSystem.GetType()
					.GetCustomAttributeValue<ExecutesAfterAttribute, Type[]>(attr => attr.Systems);
				if (executesAfter != null)
				{
					foreach (var systemType in executesAfter)
					{
						topologicalOrder.AddEdge(systemType, executeSystem.GetType());
					}
				}
			}

			var cleanupSystem = system as ICleanupSystem;
			if (cleanupSystem != null)
			{
				_cleanupSystems.Add(cleanupSystem);
			}

			var tearDownSystem = system as ITearDownSystem;
			if (tearDownSystem != null)
			{
				_tearDownSystems.Add(tearDownSystem);
			}

			return this;
		}

		public SystemsRoot Add(Feature feature)
		{
			foreach (var system in feature.Systems)
			{
				Add(system);
			}

			return this;
		}

		public void SetupOrder()
		{
			Debug.Log("The order is now:");
			foreach (var systems in orderedSystems)
			{
				foreach (var system in systems.GetOrderedVertices())
				{
					Debug.Log(system.GetType().Name);
					_executeSystems.Add(system);
				}
			}
		}
	}
}
