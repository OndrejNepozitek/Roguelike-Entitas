namespace Assets.Sources.Helpers.Entitas
{
	using System;

	[AttributeUsage(AttributeTargets.Class)]
	public class SystemPhaseAttribute : Attribute
	{
		public Phase Phase;

		public SystemPhaseAttribute(Phase phase)
		{
			Phase = phase;
		}
	}
}
