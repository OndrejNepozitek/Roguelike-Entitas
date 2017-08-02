using Entitas;

/// <summary>
/// Register all items to ItemDatabase.
/// TODO: this could be done with a database file like xml or json
/// </summary>
public class RegisterItemsSystem : IInitializeSystem
{
	public void Initialize()
	{
		var items = ItemDatabase.Instance;

		items.RegisterItem(ItemName.IronAxe, new Weapon(Prefabs.IronAxe, 10));
	}
}