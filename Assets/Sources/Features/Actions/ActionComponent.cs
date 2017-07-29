using Entitas;

[Actions]
public class ActionComponent : IComponent
{
	public IAction Action;
	public GameEntity Source;
}