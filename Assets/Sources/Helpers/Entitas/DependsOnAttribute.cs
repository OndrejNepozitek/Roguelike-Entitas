using System;

namespace Assets.Sources.Helpers.Entitas
{
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
