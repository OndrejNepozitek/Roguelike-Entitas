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
	public GameEntity Entity;
	public Player Player;
}