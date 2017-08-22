namespace Assets.Sources.Helpers.SystemDependencies.Attributes
{
	using System;

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