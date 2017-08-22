namespace Assets.Sources.Helpers.SystemDependencies.Attributes
{
	using System;
	using Phases;

	[AttributeUsage(AttributeTargets.Class)]
	public class InitializePhaseAttribute : Attribute
	{
		public InitializePhase Phase;

		public InitializePhaseAttribute(InitializePhase phase)
		{
			Phase = phase;
		}
	}
}
