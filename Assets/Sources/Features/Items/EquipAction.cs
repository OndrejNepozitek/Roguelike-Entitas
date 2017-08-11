public class EquipAction : IAction
{
	public IItem Item;
	public GameEntity Target;

	public bool Validate(GameContext context)
	{
		throw new System.NotImplementedException();
	}
}