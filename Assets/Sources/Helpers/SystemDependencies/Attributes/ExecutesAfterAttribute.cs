namespace Assets.Sources.Helpers.SystemDependencies.Attributes
{
	using System;

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
