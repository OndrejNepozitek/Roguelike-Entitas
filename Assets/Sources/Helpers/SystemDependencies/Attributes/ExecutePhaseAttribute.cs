namespace Assets.Sources.Helpers.SystemDependencies.Attributes
{
	using System;
	using Phases;

	[AttributeUsage(AttributeTargets.Class)]
	public class ExecutePhaseAttribute : Attribute
	{
		public ExecutePhase Phase;

		public ExecutePhaseAttribute(ExecutePhase phase)
		{
			Phase = phase;
		}
	}
}
