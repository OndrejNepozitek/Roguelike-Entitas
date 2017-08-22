namespace Assets.Sources.Helpers.SystemDependencies.Attributes
{
	using System;

	[AttributeUsage(AttributeTargets.Class)]
	public class DependsOnAttribute : Attribute
	{
		public Type[] Features;

		public DependsOnAttribute(params Type[] features)
		{
			Features = features;
		}
	}
}
