namespace Assets.Sources.Features.Actions
{
	using Helpers.Entitas;

	public class ActionsFeature : Feature
	{
		public ActionsFeature(Contexts contexts) : base("Actions feature")
		{
			Add(new ActionsCleanupSystem(contexts));
			Add(new ValidateActionsSystem(contexts));
		}
	}
}
