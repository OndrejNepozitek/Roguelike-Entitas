namespace Assets.Sources.Helpers.SystemDependencies.Phases
{
	public enum ExecutePhase
	{
		Init,
		Input,
		ProcessActions,
		ValidateActions,
		Network,
		ReactToActions,
		ReactToComponents,
		Cleanup
	}
}
