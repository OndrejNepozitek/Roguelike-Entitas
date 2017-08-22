namespace Assets.Sources.Helpers.SystemDependencies
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Attributes;
	using Entitas;
	using NUnit.Framework;
	using Phases;
	using TopologicalOrder;
	using UnityEngine;

	public class SystemsRoot : Systems
	{
		private readonly Dictionary<Type, List<TopologicalOrder<ISystem>>> orderedSystems = new Dictionary<Type, List<TopologicalOrder<ISystem>>>();

		public new SystemsRoot Add(ISystem system)
		{
			var initializeSystem = system as IInitializeSystem;
			if (initializeSystem != null)
			{
				AddForOrdering<IInitializeSystem, InitializePhaseAttribute, InitializePhase>(initializeSystem, defaultValue: (int)InitializePhase.Execute);
			}

			var executeSystem = system as IExecuteSystem;
			if (executeSystem != null)
			{
				AddForOrdering<IExecuteSystem, ExecutePhaseAttribute, ExecutePhase>(executeSystem);
			}

			var cleanupSystem = system as ICleanupSystem;
			if (cleanupSystem != null)
			{
				AddForOrdering<ICleanupSystem, CleanupPhaseAttribute, CleanupPhase>(cleanupSystem, defaultValue: (int)CleanupPhase.Execute);
			}

			var tearDownSystem = system as ITearDownSystem;
			if (tearDownSystem != null)
			{
				AddForOrdering<ITearDownSystem, TeardownPhaseAttribute, TeardownPhase>(tearDownSystem, defaultValue: (int)TeardownPhase.Execute);
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
			Debug.Log("The initialize order is now:");
			if (orderedSystems.ContainsKey(typeof(IInitializeSystem)))
			{
				foreach (var systems in orderedSystems[typeof(IInitializeSystem)])
				{
					foreach (var system in systems.GetOrderedVertices())
					{
						Debug.Log(system.GetType().Name);
						_initializeSystems.Add((IInitializeSystem)system);
					}
				}
			}

			Debug.Log("The execute order is now:");
			if (orderedSystems.ContainsKey(typeof(IExecuteSystem)))
			{
				foreach (var systems in orderedSystems[typeof(IExecuteSystem)])
				{
					foreach (var system in systems.GetOrderedVertices())
					{
						Debug.Log(system.GetType().Name);
						_executeSystems.Add((IExecuteSystem) system);
					}
				}
			}

			Debug.Log("The clenaup order is now:");
			if (orderedSystems.ContainsKey(typeof(ICleanupSystem)))
			{
				foreach (var systems in orderedSystems[typeof(ICleanupSystem)])
				{
					foreach (var system in systems.GetOrderedVertices())
					{
						Debug.Log(system.GetType().Name);
						_cleanupSystems.Add((ICleanupSystem)system);
					}
				}
			}

			Debug.Log("The teardown order is now:");
			if (orderedSystems.ContainsKey(typeof(ITearDownSystem)))
			{
				foreach (var systems in orderedSystems[typeof(ITearDownSystem)])
				{
					foreach (var system in systems.GetOrderedVertices())
					{
						Debug.Log(system.GetType().Name);
						_tearDownSystems.Add((ITearDownSystem)system);
					}
				}
			}
		}

		protected void AddForOrdering<T, TAttr, TEnum>(ISystem system, bool required = false, int defaultValue = 0) 
			where TEnum : struct 
			where TAttr : class
		{
			List<TopologicalOrder<ISystem>> systemsByPhases;
			// Initialize the order
			if (!orderedSystems.TryGetValue(typeof(T), out systemsByPhases))
			{
				systemsByPhases = new List<TopologicalOrder<ISystem>>();
				orderedSystems.Add(typeof(T), systemsByPhases);

				systemsByPhases.AddRange(from object _ in Enum.GetValues(typeof(TEnum)) select new TopologicalOrder<ISystem>());
			}

			var attr = system.GetType().GetCustomAttribute<TAttr>();

			if (typeof(T) == typeof(IInitializeSystem))
			{
				var initAttr = attr as InitializePhaseAttribute;
				if (initAttr != null)
				{
					var topologicalOrder = systemsByPhases[(int)initAttr.Phase];
					topologicalOrder.AddVertex(system);
					return;
				}
				else
				{
					var topologicalOrder = systemsByPhases[defaultValue];
					topologicalOrder.AddVertex(system);
					return;
				}
			}

			if (typeof(T) == typeof(IExecuteSystem))
			{
				var execAttr = attr as ExecutePhaseAttribute;
				if (execAttr != null)
				{
					var topologicalOrder = systemsByPhases[(int)execAttr.Phase];
					topologicalOrder.AddVertex(system);

					var executesBefore = system.GetType()
						.GetCustomAttributeValue<ExecutesBeforeAttribute, Type[]>(x => x.Systems);
					if (executesBefore != null)
					{
						foreach (var systemType in executesBefore)
						{
							topologicalOrder.AddEdge(system.GetType(), systemType);
						}
					}

					var executesAfter = system.GetType()
						.GetCustomAttributeValue<ExecutesAfterAttribute, Type[]>(x => x.Systems);
					if (executesAfter != null)
					{
						foreach (var systemType in executesAfter)
						{
							topologicalOrder.AddEdge(systemType, system.GetType());
						}
					}

					return;
				}
				else
				{
					throw new InvalidOperationException("System must have ExecutePhaseAttribute.");
				}
			}

			if (typeof(T) == typeof(ICleanupSystem))
			{
				var cleanupAttr = attr as CleanupPhaseAttribute;
				if (cleanupAttr != null)
				{
					var topologicalOrder = systemsByPhases[(int)cleanupAttr.Phase];
					topologicalOrder.AddVertex(system);
					return;
				}
				else
				{
					var topologicalOrder = systemsByPhases[defaultValue];
					topologicalOrder.AddVertex(system);
					return;
				}
			}


			if (typeof(T) == typeof(ITearDownSystem))
			{
				var cleanupAttr = attr as TeardownPhaseAttribute;
				if (cleanupAttr != null)
				{
					var topologicalOrder = systemsByPhases[(int)cleanupAttr.Phase];
					topologicalOrder.AddVertex(system);
					return;
				}
				else
				{
					var topologicalOrder = systemsByPhases[defaultValue];
					topologicalOrder.AddVertex(system);
					return;
				}
			}
		}
	}
}
