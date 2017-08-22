namespace Assets.Sources.Helpers.SystemDependencies.Attributes
{
	using System;
	using Phases;

	[AttributeUsage(AttributeTargets.Class)]
	public class TeardownPhaseAttribute : Attribute
	{
		public TeardownPhase Phase;

		public TeardownPhaseAttribute(TeardownPhase phase)
		{
			Phase = phase;
		}
	}
}
