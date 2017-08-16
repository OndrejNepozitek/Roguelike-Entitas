using System;

namespace Assets.Sources.Helpers.Entitas
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ExecutesBeforeAttribute : Attribute
	{
		public Type[] Systems;

		public ExecutesBeforeAttribute(params Type[] systems)
		{
			Systems = systems;
		}
	}
}