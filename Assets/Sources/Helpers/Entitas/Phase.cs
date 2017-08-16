namespace Assets.Sources.Helpers.Entitas
{
	public enum Phase
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
