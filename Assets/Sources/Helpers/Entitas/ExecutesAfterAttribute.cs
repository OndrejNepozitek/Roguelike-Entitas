using System;

namespace Assets.Sources.Helpers.Entitas
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ExecutesAfterAttribute : Attribute
	{
		public Type[] Systems;

		public ExecutesAfterAttribute(params Type[] systems)
		{
			Systems = systems;
		}
	}
}
