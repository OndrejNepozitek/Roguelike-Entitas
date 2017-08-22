namespace Assets.Sources.Helpers.SystemDependencies.Attributes
{
	using System;
	using Phases;

	[AttributeUsage(AttributeTargets.Class)]
	public class CleanupPhaseAttribute : Attribute
	{
		public CleanupPhase Phase;

		public CleanupPhaseAttribute(CleanupPhase phase)
		{
			Phase = phase;
		}
	}
}
