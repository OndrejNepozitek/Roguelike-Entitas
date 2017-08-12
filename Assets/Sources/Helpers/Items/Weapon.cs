public class Weapon : Equipment
{
	public override InventorySlot Slot
	{
		get
		{
			return InventorySlot.Weapon;
		}
	}

	public float Attack { get; private set; }

	public Weapon(ItemName name, string prefab, float attack)
	{
		Name = name;
		Prefab = prefab;
		Attack = attack;
	}
}