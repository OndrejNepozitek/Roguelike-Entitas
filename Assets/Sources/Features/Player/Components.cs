using Assets.Sources.Helpers.Networking;
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class PlayerComponent : IComponent
{
	public bool Focus;
}

[Unique]
public sealed class CurrentPlayer : IComponent
{
	public EntityReference Entity;
	public Player Player;
}